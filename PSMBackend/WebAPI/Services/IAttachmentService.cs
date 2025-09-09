using PSMModel.Models;
using PSMWebAPI.Controllers;

namespace PSMWebAPI.Services;

public interface IAttachmentService
{
    Task<IEnumerable<Attachment>> GetAttachmentsAsync(string entityType, int entityId);
    Task<Attachment?> GetAttachmentByIdAsync(int attachmentId);
    Task<Attachment> AddAttachmentAsync(IFormFile file, string entityType, int entityId, string? description = null, string? category = null, string? attachmentType = null);
    Task<IEnumerable<Attachment>> AddMultipleAttachmentsAsync(IEnumerable<IFormFile> files, string entityType, int entityId, string? category = null, string? attachmentType = null);
    Task<BulkUploadResult> BulkUploadAttachmentsAsync(BulkUploadRequest request, string entityType, int entityId);
    Task<bool> DeleteAttachmentAsync(int attachmentId);
    Task<bool> DeleteAttachmentsByEntityAsync(string entityType, int entityId);
    Task<(Stream fileStream, string contentType, string fileName)> DownloadAttachmentAsync(int attachmentId);
    Task<bool> UpdateAttachmentMetadataAsync(int attachmentId, string? description = null, string? category = null, int? orderIndex = null);
}
