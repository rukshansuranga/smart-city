using Microsoft.AspNetCore.Mvc;
using PSMWebAPI.DTOs.Common;
using PSMWebAPI.DTOs.Tag;
using PSMWebAPI.Extensions;
using PSMWebAPI.Services;

namespace PSMWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    /// <summary>
    /// Get all tags
    /// </summary>
    /// <returns>List of all tags</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllTags()
    {
        try
        {
            var tags = await _tagService.GetAllTagsAsync();
            return this.ApiSuccess(tags, "Tags retrieved successfully");
        }
        catch (Exception ex)
        {
            return this.ApiFailure<IEnumerable<TagDto>>("Error retrieving tags", ex.Message);
        }
    }

    /// <summary>
    /// Get a specific tag by ID
    /// </summary>
    /// <param name="id">Tag ID</param>
    /// <returns>Tag details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTag(int id)
    {
        try
        {
            var tag = await _tagService.GetTagByIdAsync(id);
            if (tag == null)
                return this.ApiFailure<TagDto>("Tag not found");

            return this.ApiSuccess(tag, "Tag retrieved successfully");
        }
        catch (Exception ex)
        {
            return this.ApiFailure<TagDto>("Error retrieving tag", ex.Message);
        }
    }

    /// <summary>
    /// Create a new tag
    /// </summary>
    /// <param name="request">Create tag request</param>
    /// <returns>Created tag</returns>
    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagRequest request)
    {
        try
        {
            var tag = await _tagService.CreateTagAsync(request);
            return this.ApiSuccess(tag, "Tag created successfully");
        }
        catch (Exception ex)
        {
            return this.ApiFailure<TagDto>("Error creating tag", ex.Message);
        }
    }

    /// <summary>
    /// Update an existing tag
    /// </summary>
    /// <param name="id">Tag ID</param>
    /// <param name="request">Update tag request</param>
    /// <returns>Updated tag</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTag(int id, [FromBody] UpdateTagRequest request)
    {
        try
        {
            var tag = await _tagService.UpdateTagAsync(id, request);
            if (tag == null)
                return this.ApiFailure<TagDto>("Tag not found");

            return this.ApiSuccess(tag, "Tag updated successfully");
        }
        catch (Exception ex)
        {
            return this.ApiFailure<TagDto>("Error updating tag", ex.Message);
        }
    }

    /// <summary>
    /// Delete a tag (soft delete)
    /// </summary>
    /// <param name="id">Tag ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(int id)
    {
        try
        {
            var success = await _tagService.DeleteTagAsync(id);
            if (!success)
                return this.ApiFailure("Tag not found");

            return this.ApiSuccess("Tag deleted successfully");
        }
        catch (Exception ex)
        {
            return this.ApiFailure("Error deleting tag", ex.Message);
        }
    }

    /// <summary>
    /// Search tags by name
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <returns>Matching tags</returns>
    [HttpGet("search")]
    public async Task<IActionResult> SearchTags([FromQuery] string searchTerm)
    {
        try
        {
            var tags = await _tagService.SearchTagsAsync(searchTerm);
            return this.ApiSuccess(tags, "Tags search completed");
        }
        catch (Exception ex)
        {
            return this.ApiFailure<IEnumerable<TagDto>>("Error searching tags", ex.Message);
        }
    }

    /// <summary>
    /// Get tags by entity type (shows which tags are used by each entity type)
    /// </summary>
    /// <returns>Tags grouped by entity type</returns>
    [HttpGet("by-entity-type")]
    public async Task<IActionResult> GetTagsByEntityType()
    {
        try
        {
            var tags = await _tagService.GetTagsByEntityTypeAsync();
            return this.ApiSuccess(tags, "Tags by entity type retrieved successfully");
        }
        catch (Exception ex)
        {
            return this.ApiFailure<Dictionary<string, IEnumerable<TagDto>>>("Error retrieving tags by entity type", ex.Message);
        }
    }

    /// <summary>
    /// Get all tags for a specific entity
    /// </summary>
    /// <param name="entityType">Entity type (e.g., "Ticket", "Complain", "Project")</param>
    /// <param name="entityId">Entity ID</param>
    /// <returns>Tags assigned to the entity</returns>
    [HttpGet("entity/{entityType}/{entityId}")]
    public async Task<IActionResult> GetEntityTags(string entityType, int entityId)
    {
        try
        {
            var tags = await _tagService.GetEntityTagsAsync(entityType, entityId);
            return this.ApiSuccess(tags, $"Tags for {entityType} retrieved successfully");
        }
        catch (ArgumentException ex)
        {
            return this.ApiFailure<IEnumerable<TagDto>>(ex.Message);
        }
        catch (Exception ex)
        {
            return this.ApiFailure<IEnumerable<TagDto>>("Error retrieving entity tags", ex.Message);
        }
    }

    /// <summary>
    /// Assign tags to an entity
    /// </summary>
    /// <param name="entityType">Entity type</param>
    /// <param name="entityId">Entity ID</param>
    /// <param name="request">Tag assignment request</param>
    /// <returns>Success status</returns>
    [HttpPost("entity/{entityType}/{entityId}")]
    public async Task<IActionResult> AssignTagsToEntity(string entityType, int entityId, [FromBody] List<int> tagIds)
    {
        try
        {
            // You might want to get the current user ID from the authentication context
            var assignedBy = HttpContext.User?.Identity?.Name;
            
            var success = await _tagService.AssignTagsToEntityAsync(entityType, entityId, tagIds, assignedBy);
            if (!success)
                return this.ApiFailure("Failed to assign tags to entity");

            return this.ApiSuccess($"Tags assigned to {entityType} successfully");
        }
        catch (ArgumentException ex)
        {
            return this.ApiFailure(ex.Message);
        }
        catch (Exception ex)
        {
            return this.ApiFailure("Error assigning tags to entity", ex.Message);
        }
    }

    /// <summary>
    /// Remove specific tags from an entity
    /// </summary>
    /// <param name="entityType">Entity type</param>
    /// <param name="entityId">Entity ID</param>
    /// <param name="tagIds">List of tag IDs to remove</param>
    /// <returns>Success status</returns>
    [HttpDelete("entity/{entityType}/{entityId}")]
    public async Task<IActionResult> RemoveTagsFromEntity(string entityType, int entityId, [FromBody] List<int> tagIds)
    {
        try
        {
            var success = await _tagService.RemoveTagsFromEntityAsync(entityType, entityId, tagIds);
            if (!success)
                return this.ApiFailure("Failed to remove tags from entity");

            return this.ApiSuccess($"Tags removed from {entityType} successfully");
        }
        catch (ArgumentException ex)
        {
            return this.ApiFailure(ex.Message);
        }
        catch (Exception ex)
        {
            return this.ApiFailure("Error removing tags from entity", ex.Message);
        }
    }

    /// <summary>
    /// Remove all tags from an entity
    /// </summary>
    /// <param name="entityType">Entity type</param>
    /// <param name="entityId">Entity ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("entity/{entityType}/{entityId}/all")]
    public async Task<IActionResult> RemoveAllTagsFromEntity(string entityType, int entityId)
    {
        try
        {
            var success = await _tagService.RemoveAllTagsFromEntityAsync(entityType, entityId);
            if (!success)
                return this.ApiFailure("Failed to remove all tags from entity");

            return this.ApiSuccess($"All tags removed from {entityType} successfully");
        }
        catch (ArgumentException ex)
        {
            return this.ApiFailure(ex.Message);
        }
        catch (Exception ex)
        {
            return this.ApiFailure("Error removing all tags from entity", ex.Message);
        }
    }
}
