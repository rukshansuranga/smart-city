# Attachment Management Strategy

## Overview

This document outlines the comprehensive attachment management strategy implemented for the PSM Backend application. The solution provides a scalable, generic approach to handle multiple attachments across different entities (Complains, Tickets, Projects, and future entities).

## Architecture

### 1. Generic Attachment Entity

The `Attachment` model serves as a central entity for managing all file attachments across the application:

```csharp
public class Attachment : BaseEntity
{
    public int AttachmentId { get; set; }
    public required string FileName { get; set; }           // System generated unique filename
    public required string OriginalFileName { get; set; }   // Original user filename
    public required string FilePath { get; set; }           // Physical file path
    public required string ContentType { get; set; }        // MIME type
    public long FileSize { get; set; }                      // File size in bytes
    public string? Description { get; set; }               // User description
    public required string EntityType { get; set; }         // "Complain", "Ticket", "Project", etc.
    public int EntityId { get; set; }                      // ID of the related entity
    public string? AttachmentType { get; set; }            // "Document", "Image", "Video", etc.
    public string? Category { get; set; }                  // "Specification", "Progress", "Evidence", etc.
    public int? OrderIndex { get; set; }                   // For ordering attachments
}
```

### 2. Benefits of This Strategy

#### **Scalability**

- **Future-Proof**: Easily add attachments to new entities without schema changes
- **No Table Proliferation**: Single table handles all attachment needs
- **Consistent API**: Same endpoints work for all entity types

#### **Performance**

- **Indexed Queries**: Optimized indexes on `EntityType` and `EntityId`
- **Efficient Retrieval**: Quick lookups by entity type and ID
- **Minimal Joins**: Direct queries without complex relationships

#### **Maintainability**

- **Single Source of Truth**: All attachment logic in one place
- **Consistent Validation**: Same rules apply to all entities
- **Centralized File Management**: Unified file storage and cleanup

#### **Flexibility**

- **Categorization**: Group attachments by category (Specification, Progress, Evidence)
- **Metadata**: Rich metadata support (description, order, type)
- **Soft Deletes**: Maintain audit trail with soft delete functionality

### 3. File Storage Strategy

#### **Physical Storage**

- **Organized Structure**: Files stored in `uploads/attachments/{entitytype}/`
- **Unique Naming**: GUID-based filenames prevent conflicts
- **Original Names Preserved**: User filenames stored in database
- **MIME Type Validation**: Automatic content type detection

#### **Security Considerations**

- **File Size Limits**: Configurable maximum file size (default: 10MB)
- **Content Type Validation**: Restrict allowed file types
- **Path Security**: Prevent directory traversal attacks
- **Access Control**: Implement proper authorization (to be added)

## Implementation Details

### 4. Database Schema

The migration creates the following table structure:

```sql
CREATE TABLE "Attachments" (
    "AttachmentId" SERIAL PRIMARY KEY,
    "FileName" VARCHAR(255) NOT NULL,
    "OriginalFileName" VARCHAR(255) NOT NULL,
    "FilePath" VARCHAR(500) NOT NULL,
    "ContentType" VARCHAR(100) NOT NULL,
    "FileSize" BIGINT NOT NULL,
    "Description" VARCHAR(500),
    "EntityType" VARCHAR(50) NOT NULL,
    "EntityId" INTEGER NOT NULL,
    "AttachmentType" VARCHAR(50),
    "Category" VARCHAR(100),
    "OrderIndex" INTEGER,
    -- BaseEntity fields
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP,
    "CreatedBy" TEXT,
    "UpdatedBy" TEXT,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

-- Performance indexes
CREATE INDEX "IX_Attachment_EntityType_EntityId" ON "Attachments" ("EntityType", "EntityId");
CREATE INDEX "IX_Attachment_EntityType" ON "Attachments" ("EntityType");
```

### 5. API Endpoints

The attachment system provides RESTful endpoints:

| Method   | Endpoint                                            | Purpose                        |
| -------- | --------------------------------------------------- | ------------------------------ |
| `GET`    | `/api/attachments/{entityType}/{entityId}`          | Get all attachments for entity |
| `GET`    | `/api/attachments/{id}`                             | Get specific attachment        |
| `GET`    | `/api/attachments/{id}/download`                    | Download attachment file       |
| `POST`   | `/api/attachments/{entityType}/{entityId}`          | Upload single attachment       |
| `POST`   | `/api/attachments/{entityType}/{entityId}/multiple` | Upload multiple attachments    |
| `PUT`    | `/api/attachments/{id}`                             | Update attachment metadata     |
| `DELETE` | `/api/attachments/{id}`                             | Delete specific attachment     |
| `DELETE` | `/api/attachments/{entityType}/{entityId}`          | Delete all entity attachments  |

### 6. Service Layer

The `AttachmentService` provides:

- **File Upload Handling**: Secure file processing and storage
- **Metadata Management**: Description, category, and ordering
- **File Download**: Secure file retrieval with proper headers
- **Cleanup Operations**: Soft delete with optional physical file removal
- **Validation**: File size, type, and security validation

### 7. Entity Integration

Entities implement the `IAttachable` interface:

```csharp
public interface IAttachable
{
    int GetEntityId();
    string GetEntityType();
}
```

Each entity includes:

- Navigation property: `public virtual ICollection<Attachment>? Attachments { get; set; }`
- Interface implementation for type safety

## Usage Examples

### Adding Attachments to New Entities

To add attachment support to a new entity:

1. **Implement IAttachable**:

```csharp
public class NewEntity : BaseEntity, IAttachable
{
    public int Id { get; set; }
    // ... other properties

    public virtual ICollection<Attachment>? Attachments { get; set; }

    public int GetEntityId() => Id;
    public string GetEntityType() => nameof(NewEntity);
}
```

2. **Use Existing API**: No additional code needed - existing endpoints work immediately!

### Frontend Integration

```javascript
// Upload attachment
const formData = new FormData();
formData.append("file", fileInput.files[0]);
formData.append("description", "User description");
formData.append("category", "Evidence");

fetch("/api/attachments/Complain/123", {
  method: "POST",
  body: formData,
});

// Get attachments
fetch("/api/attachments/Complain/123")
  .then((response) => response.json())
  .then((attachments) => console.log(attachments));
```

## Configuration

### appsettings.json

```json
{
  "FileStorage": {
    "UploadPath": "uploads/attachments"
  }
}
```

### Service Registration

```csharp
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
```

## Future Enhancements

1. **Cloud Storage**: Integrate with Azure Blob Storage or AWS S3
2. **Virus Scanning**: Add file security scanning
3. **Image Processing**: Thumbnail generation and image optimization
4. **Versioning**: File version management
5. **Batch Operations**: Bulk upload/download functionality
6. **Access Control**: Role-based attachment access
7. **Compression**: Automatic file compression for large files
8. **CDN Integration**: Content delivery network support

## Migration Notes

### From Old Ticket Attachments

The migration automatically:

- Removes the old `List<string>? Attachments` property from Ticket
- Creates the new Attachments table
- Maintains data integrity with proper foreign keys

### Backward Compatibility

- Old attachment strings in tickets will need manual migration if data exists
- New system is fully backward compatible for new attachments

## Best Practices

1. **Always validate file types** before upload
2. **Use transactions** for multiple file operations
3. **Implement proper error handling** for file system operations
4. **Monitor storage usage** and implement cleanup policies
5. **Use appropriate file size limits** based on your infrastructure
6. **Implement proper authorization** before allowing file access
7. **Consider implementing file virus scanning** for security

This attachment strategy provides a robust, scalable foundation for file management across your entire application while maintaining consistency and performance.
