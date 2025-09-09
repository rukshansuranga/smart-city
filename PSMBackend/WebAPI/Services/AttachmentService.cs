using Microsoft.EntityFrameworkCore;
using NodaTime;
using PSMDataAccess;
using PSMModel.Models;
using PSMWebAPI.Controllers;

namespace PSMWebAPI.Services;

public class AttachmentService : IAttachmentService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly string _uploadPath;

    public AttachmentService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _uploadPath = _configuration["FileStorage:UploadPath"] ?? "uploads/attachments";
        
        // Ensure upload directory exists
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public async Task<IEnumerable<Attachment>> GetAttachmentsAsync(string entityType, int entityId)
    {
        return await _context.Attachments
            .Where(a => a.EntityType == entityType && a.EntityId == entityId && a.IsActive)
            .OrderBy(a => a.OrderIndex ?? 0)
            .ThenBy(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<Attachment?> GetAttachmentByIdAsync(int attachmentId)
    {
        return await _context.Attachments
            .FirstOrDefaultAsync(a => a.AttachmentId == attachmentId && a.IsActive);
    }

    public async Task<Attachment> AddAttachmentAsync(IFormFile file, string entityType, int entityId, 
        string? description = null, string? category = null, string? attachmentType = null)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is required and must not be empty");

        // Validate file size (e.g., max 10MB)
        const long maxFileSize = 10 * 1024 * 1024; // 10MB
        if (file.Length > maxFileSize)
            throw new ArgumentException($"File size cannot exceed {maxFileSize / (1024 * 1024)}MB");

        // Generate unique filename
        var fileExtension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var entityFolder = Path.Combine(_uploadPath, entityType.ToLower());
        
        if (!Directory.Exists(entityFolder))
        {
            Directory.CreateDirectory(entityFolder);
        }
        
        var filePath = Path.Combine(entityFolder, uniqueFileName);

        // Save file to disk
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Create attachment record
        var attachment = new Attachment
        {
            FileName = uniqueFileName,
            OriginalFileName = file.FileName,
            FilePath = filePath,
            ContentType = file.ContentType,
            FileSize = file.Length,
            Description = description,
            EntityType = entityType,
            EntityId = entityId,
            AttachmentType = attachmentType ?? DetermineAttachmentType(file.ContentType),
            Category = category,
            CreatedAt = SystemClock.Instance.GetCurrentInstant().InUtc().LocalDateTime,
            IsActive = true
        };

        _context.Attachments.Add(attachment);
        await _context.SaveChangesAsync();

        return attachment;
    }

    public async Task<IEnumerable<Attachment>> AddMultipleAttachmentsAsync(IEnumerable<IFormFile> files, 
        string entityType, int entityId, string? category = null, string? attachmentType = null)
    {
        var attachments = new List<Attachment>();
        
        foreach (var file in files)
        {
            if (file != null && file.Length > 0)
            {
                var attachment = await AddAttachmentAsync(file, entityType, entityId, null, category, attachmentType);
                attachments.Add(attachment);
            }
        }
        
        return attachments;
    }

    public async Task<BulkUploadResult> BulkUploadAttachmentsAsync(BulkUploadRequest request, string entityType, int entityId)
    {
        var result = new BulkUploadResult
        {
            TotalFiles = request.Files.Count()
        };

        var successfulUploads = new List<Attachment>();
        var errors = new List<BulkUploadError>();

        var filesList = request.Files.ToList();
        var descriptionsList = request.Descriptions?.ToList() ?? new List<string>();
        var categoriesList = request.Categories?.ToList() ?? new List<string>();
        var attachmentTypesList = request.AttachmentTypes?.ToList() ?? new List<string>();

        // Use database transaction for atomicity if ContinueOnError is false
        using var transaction = request.ContinueOnError ? null : await _context.Database.BeginTransactionAsync();

        try
        {
            for (int i = 0; i < filesList.Count; i++)
            {
                var file = filesList[i];
                
                if (file == null || file.Length == 0)
                {
                    errors.Add(new BulkUploadError
                    {
                        FileName = file?.FileName ?? $"File_{i + 1}",
                        FileIndex = i,
                        Error = "File is null or empty"
                    });
                    continue;
                }

                try
                {
                    // Get individual file configuration or use defaults
                    var description = i < descriptionsList.Count ? descriptionsList[i] : null;
                    var category = i < categoriesList.Count ? categoriesList[i] : request.DefaultCategory;
                    var attachmentType = i < attachmentTypesList.Count ? attachmentTypesList[i] : request.DefaultAttachmentType;

                    var attachment = await AddAttachmentAsync(
                        file, entityType, entityId, description, category, attachmentType);
                    
                    successfulUploads.Add(attachment);
                }
                catch (Exception ex)
                {
                    var error = new BulkUploadError
                    {
                        FileName = file.FileName,
                        FileIndex = i,
                        Error = ex.Message,
                        Details = ex.InnerException?.Message
                    };
                    errors.Add(error);

                    // If not continuing on error, break and rollback
                    if (!request.ContinueOnError)
                    {
                        if (transaction != null)
                        {
                            await transaction.RollbackAsync();
                        }
                        
                        result.SuccessfulUploads = new List<Attachment>();
                        result.Errors = errors;
                        result.SuccessCount = 0;
                        result.FailureCount = errors.Count;
                        
                        return result;
                    }
                }
            }

            // Commit transaction if using one
            if (transaction != null)
            {
                await transaction.CommitAsync();
            }

            result.SuccessfulUploads = successfulUploads;
            result.Errors = errors;
            result.SuccessCount = successfulUploads.Count;
            result.FailureCount = errors.Count;

            return result;
        }
        catch (Exception ex)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
            }
            
            throw new Exception($"Bulk upload failed: {ex.Message}", ex);
        }
    }

    public async Task<bool> DeleteAttachmentAsync(int attachmentId)
    {
        var attachment = await GetAttachmentByIdAsync(attachmentId);
        if (attachment == null)
            return false;

        // Soft delete
        attachment.IsActive = false;
        attachment.UpdatedAt = SystemClock.Instance.GetCurrentInstant().InUtc().LocalDateTime;
        
        await _context.SaveChangesAsync();

        // Optionally, delete physical file
        try
        {
            if (File.Exists(attachment.FilePath))
            {
                File.Delete(attachment.FilePath);
            }
        }
        catch
        {
            // Log error but don't fail the operation
        }

        return true;
    }

    public async Task<bool> DeleteAttachmentsByEntityAsync(string entityType, int entityId)
    {
        var attachments = await _context.Attachments
            .Where(a => a.EntityType == entityType && a.EntityId == entityId && a.IsActive)
            .ToListAsync();

        foreach (var attachment in attachments)
        {
            attachment.IsActive = false;
            attachment.UpdatedAt = SystemClock.Instance.GetCurrentInstant().InUtc().LocalDateTime;
            
            // Optionally, delete physical file
            try
            {
                if (File.Exists(attachment.FilePath))
                {
                    File.Delete(attachment.FilePath);
                }
            }
            catch
            {
                // Log error but don't fail the operation
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(Stream fileStream, string contentType, string fileName)> DownloadAttachmentAsync(int attachmentId)
    {
        var attachment = await GetAttachmentByIdAsync(attachmentId);
        if (attachment == null)
            throw new FileNotFoundException("Attachment not found");

        if (!File.Exists(attachment.FilePath))
            throw new FileNotFoundException("Physical file not found");

        var fileStream = new FileStream(attachment.FilePath, FileMode.Open, FileAccess.Read);
        return (fileStream, attachment.ContentType, attachment.OriginalFileName);
    }

    public async Task<bool> UpdateAttachmentMetadataAsync(int attachmentId, string? description = null, 
        string? category = null, int? orderIndex = null)
    {
        var attachment = await GetAttachmentByIdAsync(attachmentId);
        if (attachment == null)
            return false;

        if (description != null)
            attachment.Description = description;
        
        if (category != null)
            attachment.Category = category;
            
        if (orderIndex.HasValue)
            attachment.OrderIndex = orderIndex.Value;

        attachment.UpdatedAt = SystemClock.Instance.GetCurrentInstant().InUtc().LocalDateTime;
        
        await _context.SaveChangesAsync();
        return true;
    }

    private static string DetermineAttachmentType(string contentType)
    {
        return contentType.ToLower() switch
        {
            var ct when ct.StartsWith("image/") => "Image",
            var ct when ct.StartsWith("video/") => "Video",
            var ct when ct.StartsWith("audio/") => "Audio",
            "application/pdf" => "Document",
            var ct when ct.Contains("word") || ct.Contains("document") => "Document",
            var ct when ct.Contains("excel") || ct.Contains("spreadsheet") => "Document",
            var ct when ct.Contains("powerpoint") || ct.Contains("presentation") => "Document",
            _ => "File"
        };
    }
}
