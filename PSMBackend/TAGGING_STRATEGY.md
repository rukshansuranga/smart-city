# Multi-Entity Tagging System Strategy

## Overview

This document outlines the strategy for implementing a flexible tagging system that can be applied to multiple entities (Ticket, Complain, Project, and future entities) in the PSM Backend system.

## Architecture Decision

### Selected Strategy: Entity-Independent Tags with Junction Tables

We've chosen this approach because it provides:

- **Flexibility**: Easy to add new entity types
- **Performance**: Optimized database queries with proper indexing
- **Maintainability**: Clean separation of concerns
- **Reusability**: Single tag management system for all entities

## Database Schema

### Core Tables

1. **Tags Table**

   - `TagId` (Primary Key)
   - `Name` (Unique, Required)
   - `Description` (Optional)
   - `Color` (Optional - for UI display)
   - `IsActive` (Boolean - for soft delete)

2. **Junction Tables** (Many-to-Many relationships)
   - `TicketTags` (TicketId, TagId, CreatedAt, CreatedBy)
   - `ComplainTags` (ComplainId, TagId, CreatedAt, CreatedBy)
   - `ProjectTags` (ProjectId, TagId, CreatedAt, CreatedBy)

### Benefits of This Design

1. **Normalization**: No data duplication
2. **Consistency**: Single source of truth for tag definitions
3. **Flexibility**: Easy to add new entity types by creating new junction tables
4. **Performance**: Efficient queries with proper indexing
5. **Audit Trail**: Track who assigned tags and when

## Implementation Components

### 1. Models

- `Tag.cs` - Core tag entity
- `TicketTag.cs`, `ComplainTag.cs`, `ProjectTag.cs` - Junction entities
- Updated existing models with navigation properties

### 2. Entity Framework Configuration

- `TagConfiguration.cs` - Core tag configuration with seeded data
- Junction table configurations with composite keys
- Updated `ApplicationDbContext.cs`

### 3. DTOs

- `TagDto.cs` - For API responses
- `CreateTagRequest.cs` - For creating new tags
- `UpdateTagRequest.cs` - For updating tags
- `EntityTagRequest.cs` - For assigning tags to entities

### 4. Services

- `ITagService.cs` - Service interface
- `TagService.cs` - Complete implementation with all CRUD operations

### 5. Controllers

- `TagsController.cs` - RESTful API endpoints for tag management

## API Endpoints

### Tag Management

- `GET /api/tags` - Get all tags
- `GET /api/tags/{id}` - Get specific tag
- `POST /api/tags` - Create new tag
- `PUT /api/tags/{id}` - Update tag
- `DELETE /api/tags/{id}` - Delete tag (soft delete)
- `GET /api/tags/search?searchTerm={term}` - Search tags

### Entity Tag Operations

- `GET /api/tags/entity/{entityType}/{entityId}` - Get tags for entity
- `POST /api/tags/entity/{entityType}/{entityId}` - Assign tags to entity
- `DELETE /api/tags/entity/{entityType}/{entityId}` - Remove specific tags
- `DELETE /api/tags/entity/{entityType}/{entityId}/all` - Remove all tags

### Analytics

- `GET /api/tags/by-entity-type` - Get tag usage by entity type

## Migration from Current String-Based Tags

### Phase 1: Parallel Implementation

1. Keep existing `Tags` string field (marked as `[Obsolete]`)
2. Implement new tag system alongside
3. Create data migration to convert existing string tags to new system

### Phase 2: Data Migration Script

```csharp
// Example migration logic - COMPLETED
// This shows how the old string tags were migrated to the new system
public async Task MigrateStringTagsToNewSystem()
{
    // NOTE: This migration logic is no longer needed as the old Tags column
    // has been removed. This is kept for reference only.

    // var tickets = await _context.Tickets
    //     .Where(t => !string.IsNullOrEmpty(t.Tags))
    //     .ToListAsync();

    // foreach (var ticket in tickets)
    // {
    //     var tagNames = ticket.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
    //         .Select(t => t.Trim());

    //     foreach (var tagName in tagNames)
    //     {
    //         // Create or find existing tag
    //         var tag = await GetOrCreateTag(tagName);

    //         // Create junction record
    //         await AssignTagToTicket(ticket.TicketId, tag.TagId);
    //     }
    // }
}
```

### Phase 3: Cleanup

1. Remove obsolete `Tags` string field after migration is complete
2. Update all references to use new tag system

## Best Practices

### 1. Tag Naming Conventions

- Use consistent naming (PascalCase recommended)
- Keep names short but descriptive
- Avoid special characters except hyphens and underscores

### 2. Performance Considerations

- Index tag names for fast searching
- Use composite indexes on junction tables
- Consider tag caching for frequently accessed tags
- Implement pagination for large tag lists

### 3. Validation Rules

- Tag names must be unique (case-insensitive)
- Maximum length for tag names (50 characters)
- Sanitize tag names to prevent XSS
- Validate entity existence before tag assignment

### 4. Security Considerations

- Validate user permissions before tag operations
- Log tag assignments for audit purposes
- Sanitize tag descriptions and names

## Usage Examples

### Creating Tags

```csharp
var createRequest = new CreateTagRequest
{
    Name = "Critical",
    Description = "Critical priority items",
    Color = "#FF0000",
    IsActive = true
};

var tag = await _tagService.CreateTagAsync(createRequest);
```

### Assigning Tags to Entities

```csharp
// Assign multiple tags to a ticket
var tagIds = new List<int> { 1, 2, 3 };
await _tagService.AssignTagsToEntityAsync("Ticket", ticketId, tagIds, userId);
```

### Querying Entities by Tags

```csharp
// Get all tickets with specific tags
var ticketsWithUrgentTag = await _context.Tickets
    .Where(t => t.TicketTags.Any(tt => tt.Tag.Name == "Urgent"))
    .Include(t => t.TicketTags)
    .ThenInclude(tt => tt.Tag)
    .ToListAsync();
```

## Future Extensibility

### Adding New Entity Types

To add tags to a new entity (e.g., `Contract`):

1. Create junction table model:

```csharp
public class ContractTag
{
    public int ContractId { get; set; }
    public virtual Contract Contract { get; set; }

    public int TagId { get; set; }
    public virtual Tag Tag { get; set; }

    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
}
```

2. Add navigation property to `Tag` model:

```csharp
public virtual ICollection<ContractTag> ContractTags { get; set; }
```

3. Add navigation property to entity model:

```csharp
public virtual ICollection<ContractTag> ContractTags { get; set; }
```

4. Update `TagService` with new entity type methods
5. Add EF configuration
6. Create and run migration

## Monitoring and Analytics

### Useful Queries for Analytics

- Most used tags across all entities
- Entities without any tags
- Tag usage trends over time
- User tag assignment patterns

## Migration Schedule

1. **Week 1**: Implement core tag system (models, services, controllers)
2. **Week 2**: Create data migration scripts and test
3. **Week 3**: Deploy to staging and verify functionality
4. **Week 4**: Deploy to production and run migration
5. **Week 5**: Remove obsolete string fields after verification

## Support for Existing Features

The new tag system maintains backward compatibility during the transition period and provides enhanced functionality including:

- Better search and filtering capabilities
- Standardized tag management
- Improved reporting and analytics
- User-friendly tag assignment interfaces
- Audit trail for tag operations

This strategy provides a robust, scalable, and maintainable solution for multi-entity tagging in your PSM Backend system.
