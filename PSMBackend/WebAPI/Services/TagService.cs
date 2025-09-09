using Microsoft.EntityFrameworkCore;
using PSMDataAccess;
using PSMModel.Models;
using PSMWebAPI.DTOs.Tag;

namespace PSMWebAPI.Services;

public class TagService : ITagService
{
    private readonly ApplicationDbContext _context;

    public TagService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
    {
        return await _context.Tags
            .Where(t => t.IsActive)
            .Select(t => new TagDto
            {
                TagId = t.TagId,
                Name = t.Name,
                Description = t.Description,
                Color = t.Color,
                IsActive = t.IsActive
            })
            .ToListAsync();
    }

    public async Task<TagDto?> GetTagByIdAsync(int tagId)
    {
        var tag = await _context.Tags.FindAsync(tagId);
        if (tag == null) return null;

        return new TagDto
        {
            TagId = tag.TagId,
            Name = tag.Name,
            Description = tag.Description,
            Color = tag.Color,
            IsActive = tag.IsActive
        };
    }

    public async Task<TagDto> CreateTagAsync(CreateTagRequest request)
    {
        var tag = new Tag
        {
            Name = request.Name,
            Description = request.Description,
            Color = request.Color,
            IsActive = request.IsActive
        };

        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();

        return new TagDto
        {
            TagId = tag.TagId,
            Name = tag.Name,
            Description = tag.Description,
            Color = tag.Color,
            IsActive = tag.IsActive
        };
    }

    public async Task<TagDto?> UpdateTagAsync(int tagId, UpdateTagRequest request)
    {
        var tag = await _context.Tags.FindAsync(tagId);
        if (tag == null) return null;

        if (request.Name != null) tag.Name = request.Name;
        if (request.Description != null) tag.Description = request.Description;
        if (request.Color != null) tag.Color = request.Color;
        if (request.IsActive.HasValue) tag.IsActive = request.IsActive.Value;

        await _context.SaveChangesAsync();

        return new TagDto
        {
            TagId = tag.TagId,
            Name = tag.Name,
            Description = tag.Description,
            Color = tag.Color,
            IsActive = tag.IsActive
        };
    }

    public async Task<bool> DeleteTagAsync(int tagId)
    {
        var tag = await _context.Tags.FindAsync(tagId);
        if (tag == null) return false;

        tag.IsActive = false; // Soft delete
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<TagDto>> GetEntityTagsAsync(string entityType, int entityId)
    {
        return entityType.ToLower() switch
        {
            "ticket" => await GetTicketTagsAsync(entityId),
            "complain" => await GetComplainTagsAsync(entityId),
            "project" => await GetProjectTagsAsync(entityId),
            _ => throw new ArgumentException($"Unsupported entity type: {entityType}")
        };
    }

    public async Task<bool> AssignTagsToEntityAsync(string entityType, int entityId, List<int> tagIds, string? assignedBy = null)
    {
        return entityType.ToLower() switch
        {
            "ticket" => await AssignTagsToTicketAsync(entityId, tagIds, assignedBy),
            "complain" => await AssignTagsToComplainAsync(entityId, tagIds, assignedBy),
            "project" => await AssignTagsToProjectAsync(entityId, tagIds, assignedBy),
            _ => throw new ArgumentException($"Unsupported entity type: {entityType}")
        };
    }

    public async Task<bool> RemoveTagsFromEntityAsync(string entityType, int entityId, List<int> tagIds)
    {
        return entityType.ToLower() switch
        {
            "ticket" => await RemoveTagsFromTicketAsync(entityId, tagIds),
            "complain" => await RemoveTagsFromComplainAsync(entityId, tagIds),
            "project" => await RemoveTagsFromProjectAsync(entityId, tagIds),
            _ => throw new ArgumentException($"Unsupported entity type: {entityType}")
        };
    }

    public async Task<bool> RemoveAllTagsFromEntityAsync(string entityType, int entityId)
    {
        return entityType.ToLower() switch
        {
            "ticket" => await RemoveAllTagsFromTicketAsync(entityId),
            "complain" => await RemoveAllTagsFromComplainAsync(entityId),
            "project" => await RemoveAllTagsFromProjectAsync(entityId),
            _ => throw new ArgumentException($"Unsupported entity type: {entityType}")
        };
    }

    public async Task<IEnumerable<TagDto>> SearchTagsAsync(string searchTerm)
    {
        return await _context.Tags
            .Where(t => t.IsActive && t.Name.Contains(searchTerm))
            .Select(t => new TagDto
            {
                TagId = t.TagId,
                Name = t.Name,
                Description = t.Description,
                Color = t.Color,
                IsActive = t.IsActive
            })
            .ToListAsync();
    }

    public async Task<Dictionary<string, IEnumerable<TagDto>>> GetTagsByEntityTypeAsync()
    {
        var result = new Dictionary<string, IEnumerable<TagDto>>();
        
        result["Ticket"] = await GetTicketTagsUsageAsync();
        result["Complain"] = await GetComplainTagsUsageAsync();
        result["Project"] = await GetProjectTagsUsageAsync();
        
        return result;
    }

    // Private helper methods for each entity type
    private async Task<IEnumerable<TagDto>> GetTicketTagsAsync(int ticketId)
    {
        return await _context.TicketTags
            .Where(tt => tt.TicketId == ticketId)
            .Include(tt => tt.Tag)
            .Select(tt => new TagDto
            {
                TagId = tt.Tag.TagId,
                Name = tt.Tag.Name,
                Description = tt.Tag.Description,
                Color = tt.Tag.Color,
                IsActive = tt.Tag.IsActive
            })
            .ToListAsync();
    }

    private async Task<IEnumerable<TagDto>> GetComplainTagsAsync(int complainId)
    {
        return await _context.ComplainTags
            .Where(ct => ct.ComplainId == complainId)
            .Include(ct => ct.Tag)
            .Select(ct => new TagDto
            {
                TagId = ct.Tag.TagId,
                Name = ct.Tag.Name,
                Description = ct.Tag.Description,
                Color = ct.Tag.Color,
                IsActive = ct.Tag.IsActive
            })
            .ToListAsync();
    }

    private async Task<IEnumerable<TagDto>> GetProjectTagsAsync(int projectId)
    {
        return await _context.ProjectTags
            .Where(pt => pt.ProjectId == projectId)
            .Include(pt => pt.Tag)
            .Select(pt => new TagDto
            {
                TagId = pt.Tag.TagId,
                Name = pt.Tag.Name,
                Description = pt.Tag.Description,
                Color = pt.Tag.Color,
                IsActive = pt.Tag.IsActive
            })
            .ToListAsync();
    }

    private async Task<bool> AssignTagsToTicketAsync(int ticketId, List<int> tagIds, string? assignedBy)
    {
        var existingTags = await _context.TicketTags
            .Where(tt => tt.TicketId == ticketId)
            .Select(tt => tt.TagId)
            .ToListAsync();

        var newTagIds = tagIds.Except(existingTags).ToList();
        
        foreach (var tagId in newTagIds)
        {
            _context.TicketTags.Add(new TicketTag
            {
                TicketId = ticketId,
                TagId = tagId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = assignedBy
            });
        }

        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> AssignTagsToComplainAsync(int complainId, List<int> tagIds, string? assignedBy)
    {
        var existingTags = await _context.ComplainTags
            .Where(ct => ct.ComplainId == complainId)
            .Select(ct => ct.TagId)
            .ToListAsync();

        var newTagIds = tagIds.Except(existingTags).ToList();
        
        foreach (var tagId in newTagIds)
        {
            _context.ComplainTags.Add(new ComplainTag
            {
                ComplainId = complainId,
                TagId = tagId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = assignedBy
            });
        }

        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> AssignTagsToProjectAsync(int projectId, List<int> tagIds, string? assignedBy)
    {
        var existingTags = await _context.ProjectTags
            .Where(pt => pt.ProjectId == projectId)
            .Select(pt => pt.TagId)
            .ToListAsync();

        var newTagIds = tagIds.Except(existingTags).ToList();
        
        foreach (var tagId in newTagIds)
        {
            _context.ProjectTags.Add(new ProjectTag
            {
                ProjectId = projectId,
                TagId = tagId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = assignedBy
            });
        }

        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> RemoveTagsFromTicketAsync(int ticketId, List<int> tagIds)
    {
        var tagsToRemove = await _context.TicketTags
            .Where(tt => tt.TicketId == ticketId && tagIds.Contains(tt.TagId))
            .ToListAsync();

        _context.TicketTags.RemoveRange(tagsToRemove);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> RemoveTagsFromComplainAsync(int complainId, List<int> tagIds)
    {
        var tagsToRemove = await _context.ComplainTags
            .Where(ct => ct.ComplainId == complainId && tagIds.Contains(ct.TagId))
            .ToListAsync();

        _context.ComplainTags.RemoveRange(tagsToRemove);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> RemoveTagsFromProjectAsync(int projectId, List<int> tagIds)
    {
        var tagsToRemove = await _context.ProjectTags
            .Where(pt => pt.ProjectId == projectId && tagIds.Contains(pt.TagId))
            .ToListAsync();

        _context.ProjectTags.RemoveRange(tagsToRemove);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> RemoveAllTagsFromTicketAsync(int ticketId)
    {
        var allTags = await _context.TicketTags
            .Where(tt => tt.TicketId == ticketId)
            .ToListAsync();

        _context.TicketTags.RemoveRange(allTags);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> RemoveAllTagsFromComplainAsync(int complainId)
    {
        var allTags = await _context.ComplainTags
            .Where(ct => ct.ComplainId == complainId)
            .ToListAsync();

        _context.ComplainTags.RemoveRange(allTags);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> RemoveAllTagsFromProjectAsync(int projectId)
    {
        var allTags = await _context.ProjectTags
            .Where(pt => pt.ProjectId == projectId)
            .ToListAsync();

        _context.ProjectTags.RemoveRange(allTags);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<IEnumerable<TagDto>> GetTicketTagsUsageAsync()
    {
        return await _context.Tags
            .Where(t => t.TicketTags.Any())
            .Select(t => new TagDto
            {
                TagId = t.TagId,
                Name = t.Name,
                Description = t.Description,
                Color = t.Color,
                IsActive = t.IsActive
            })
            .ToListAsync();
    }

    private async Task<IEnumerable<TagDto>> GetComplainTagsUsageAsync()
    {
        return await _context.Tags
            .Where(t => t.ComplainTags.Any())
            .Select(t => new TagDto
            {
                TagId = t.TagId,
                Name = t.Name,
                Description = t.Description,
                Color = t.Color,
                IsActive = t.IsActive
            })
            .ToListAsync();
    }

    private async Task<IEnumerable<TagDto>> GetProjectTagsUsageAsync()
    {
        return await _context.Tags
            .Where(t => t.ProjectTags.Any())
            .Select(t => new TagDto
            {
                TagId = t.TagId,
                Name = t.Name,
                Description = t.Description,
                Color = t.Color,
                IsActive = t.IsActive
            })
            .ToListAsync();
    }
}
