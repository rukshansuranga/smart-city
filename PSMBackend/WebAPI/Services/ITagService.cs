using PSMModel.Models;
using PSMWebAPI.DTOs.Tag;

namespace PSMWebAPI.Services;

public interface ITagService
{
    // Tag CRUD operations
    Task<IEnumerable<TagDto>> GetAllTagsAsync();
    Task<TagDto?> GetTagByIdAsync(int tagId);
    Task<TagDto> CreateTagAsync(CreateTagRequest request);
    Task<TagDto?> UpdateTagAsync(int tagId, UpdateTagRequest request);
    Task<bool> DeleteTagAsync(int tagId);
    
    // Entity tag operations
    Task<IEnumerable<TagDto>> GetEntityTagsAsync(string entityType, int entityId);
    Task<bool> AssignTagsToEntityAsync(string entityType, int entityId, List<int> tagIds, string? assignedBy = null);
    Task<bool> RemoveTagsFromEntityAsync(string entityType, int entityId, List<int> tagIds);
    Task<bool> RemoveAllTagsFromEntityAsync(string entityType, int entityId);
    
    // Search and filter operations
    Task<IEnumerable<TagDto>> SearchTagsAsync(string searchTerm);
    Task<Dictionary<string, IEnumerable<TagDto>>> GetTagsByEntityTypeAsync();
}
