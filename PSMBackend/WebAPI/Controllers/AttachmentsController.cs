using Microsoft.AspNetCore.Mvc;
using PSMWebAPI.Services;
using PSMModel.Models;
using PSMWebAPI.DTOs.Common;
using PSMWebAPI.Extensions;

namespace PSMWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttachmentsController : ControllerBase
{
    private readonly IAttachmentService _attachmentService;

    public AttachmentsController(IAttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    /// <summary>
    /// Get all attachments for a specific entity
    /// </summary>
    /// <param name="entityType">The type of entity (e.g., "Complain", "Ticket", "Project")</param>
    /// <param name="entityId">The ID of the entity</param>
    /// <returns>List of attachments</returns>
    [HttpGet("{entityType}/{entityId}")]
    public async Task<IActionResult> GetAttachments(string entityType, int entityId)
    {
        try
        {
            var attachments = await _attachmentService.GetAttachmentsAsync(entityType, entityId);
            return this.ApiSuccess(attachments, "Attachments retrieved successfully");
        }
        catch (Exception ex)
        {
            return this.ApiFailure<IEnumerable<Attachment>>("Error retrieving attachments", ex.Message);
        }
    }

    /// <summary>
    /// Get a specific attachment by ID
    /// </summary>
    /// <param name="id">Attachment ID</param>
    /// <returns>Attachment details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAttachment(int id)
    {
        try
        {
            var attachment = await _attachmentService.GetAttachmentByIdAsync(id);
            if (attachment == null)
                return this.ApiFailure<Attachment>("Attachment not found");

            return this.ApiSuccess(attachment, "Attachment retrieved successfully");
        }
        catch (Exception ex)
        {
            return this.ApiFailure<Attachment>("Error retrieving attachment", ex.Message);
        }
    }

    /// <summary>
    /// Upload a single attachment
    /// </summary>
    /// <param name="entityType">The type of entity</param>
    /// <param name="entityId">The ID of the entity</param>
    /// <param name="file">The file to upload</param>
    /// <param name="description">Optional description</param>
    /// <param name="category">Optional category</param>
    /// <param name="attachmentType">Optional attachment type</param>
    /// <returns>Created attachment</returns>
    [HttpPost("{entityType}/{entityId}")]
    public async Task<IActionResult> UploadAttachment(
        string entityType, 
        int entityId, 
        IFormFile file,
        [FromForm] string? description = null,
        [FromForm] string? category = null,
        [FromForm] string? attachmentType = null)
    {
        try
        {
            var attachment = await _attachmentService.AddAttachmentAsync(
                file, entityType, entityId, description, category, attachmentType);
            
            return this.ApiSuccess(attachment, "Attachment uploaded successfully");
        }
        catch (Exception ex)
        {
            return this.ApiFailure<Attachment>("Error uploading attachment", ex.Message);
        }
    }

    /// <summary>
    /// Upload multiple attachments
    /// </summary>
    /// <param name="entityType">The type of entity</param>
    /// <param name="entityId">The ID of the entity</param>
    /// <param name="files">The files to upload</param>
    /// <param name="category">Optional category for all files</param>
    /// <param name="attachmentType">Optional attachment type for all files</param>
    /// <returns>List of created attachments</returns>
    [HttpPost("{entityType}/{entityId}/multiple")]
    public async Task<IActionResult> UploadMultipleAttachments(
        string entityType, 
        int entityId, 
        [FromForm] IEnumerable<IFormFile> files,
        [FromForm] string? category = null,
        [FromForm] string? attachmentType = null)
    {
        try
        {
            var attachments = await _attachmentService.AddMultipleAttachmentsAsync(
                files, entityType, entityId, category, attachmentType);
            
            return this.ApiSuccess(attachments, "Multiple attachments uploaded successfully");
        }
        catch (Exception ex)
        {
            return this.ApiFailure<IEnumerable<Attachment>>("Error uploading multiple attachments", ex.Message);
        }
    }

    /// <summary>
    /// Bulk upload attachments with detailed configuration for each file
    /// </summary>
    /// <param name="entityType">The type of entity</param>
    /// <param name="entityId">The ID of the entity</param>
    /// <param name="request">Bulk upload request with file configurations</param>
    /// <returns>Bulk upload result with success and failure details</returns>
    [HttpPost("{entityType}/{entityId}/bulk")]
    public async Task<IActionResult> BulkUploadAttachments(
        string entityType, 
        int entityId, 
        [FromForm] BulkUploadRequest request)
    {
        try
        {
            if (request.Files == null || !request.Files.Any())
            {
                return this.ApiFailure<BulkUploadResult>("No files provided for upload");
            }

            var result = await _attachmentService.BulkUploadAttachmentsAsync(
                request, entityType, entityId);
            
            if (result.SuccessfulUploads.Any())
            {
                return this.ApiSuccess(result, result.Summary);
            }
            else
            {
                return this.ApiFailure<BulkUploadResult>("Bulk upload failed", "All file uploads failed");
            }
        }
        catch (Exception ex)
        {
            return this.ApiFailure<BulkUploadResult>("Error during bulk upload", ex.Message);
        }
    }

    /// <summary>
    /// Download an attachment
    /// </summary>
    /// <param name="id">Attachment ID</param>
    /// <returns>File download</returns>
    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadAttachment(int id)
    {
        try
        {
            var (fileStream, contentType, fileName) = await _attachmentService.DownloadAttachmentAsync(id);
            return File(fileStream, contentType, fileName);
        }
        catch (FileNotFoundException)
        {
            return this.ApiFailure("Attachment file not found");
        }
        catch (Exception ex)
        {
            return this.ApiFailure("Error downloading attachment", ex.Message);
        }
    }

    /// <summary>
    /// Update attachment metadata
    /// </summary>
    /// <param name="id">Attachment ID</param>
    /// <param name="request">Update attachment request</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAttachment(
        int id, 
        [FromBody] UpdateAttachmentRequest request)
    {
        try
        {
            var success = await _attachmentService.UpdateAttachmentMetadataAsync(
                id, request.Description, request.Category, request.OrderIndex);
            
            if (!success)
                return this.ApiFailure("Attachment not found");

            return this.ApiSuccess("Attachment updated successfully");
        }
        catch (Exception ex)
        {
            return this.ApiFailure("Error updating attachment", ex.Message);
        }
    }

    /// <summary>
    /// Delete an attachment
    /// </summary>
    /// <param name="id">Attachment ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttachment(int id)
    {
        try
        {
            var success = await _attachmentService.DeleteAttachmentAsync(id);
            if (!success)
                return this.ApiFailure("Attachment not found");

            return this.ApiSuccess("Attachment deleted successfully");
        }
        catch (Exception ex)
        {
            return this.ApiFailure("Error deleting attachment", ex.Message);
        }
    }

    /// <summary>
    /// Delete all attachments for an entity
    /// </summary>
    /// <param name="entityType">The type of entity</param>
    /// <param name="entityId">The ID of the entity</param>
    /// <returns>Success status</returns>
    [HttpDelete("{entityType}/{entityId}")]
    public async Task<IActionResult> DeleteEntityAttachments(string entityType, int entityId)
    {
        try
        {
            var success = await _attachmentService.DeleteAttachmentsByEntityAsync(entityType, entityId);
            return this.ApiSuccess("Entity attachments deleted successfully");
        }
        catch (Exception ex)
        {
            return this.ApiFailure("Error deleting entity attachments", ex.Message);
        }
    }
}

public class UpdateAttachmentRequest
{
    public string? Description { get; set; }
    public string? Category { get; set; }
    public int? OrderIndex { get; set; }
}

public class  BulkUploadRequest
{
    public IEnumerable<IFormFile> Files { get; set; } = new List<IFormFile>();
    public IEnumerable<string>? Descriptions { get; set; }
    public IEnumerable<string>? Categories { get; set; }
    public IEnumerable<string>? AttachmentTypes { get; set; }
    public string? DefaultCategory { get; set; }
    public string? DefaultAttachmentType { get; set; }
    public bool ContinueOnError { get; set; } = true;
}

public class BulkUploadResult
{
    public int TotalFiles { get; set; }
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public IEnumerable<Attachment> SuccessfulUploads { get; set; } = new List<Attachment>();
    public IEnumerable<BulkUploadError> Errors { get; set; } = new List<BulkUploadError>();
    public bool HasErrors => Errors.Any();
    public string Summary => $"Successfully uploaded {SuccessCount} of {TotalFiles} files";
}

public class BulkUploadError
{
    public string FileName { get; set; } = string.Empty;
    public int FileIndex { get; set; }
    public string Error { get; set; } = string.Empty;
    public string? Details { get; set; }
}
