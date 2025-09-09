# Bulk Upload Attachment Endpoint

This document describes the enhanced bulk upload functionality for the PSM Backend attachment system.

## New Endpoint

### POST `/api/attachments/{entityType}/{entityId}/bulk`

Enhanced bulk upload endpoint that provides advanced features beyond the existing `/multiple` endpoint.

## Features

### Enhanced Error Handling

- Individual file error tracking
- Configurable failure behavior (continue on error vs. stop on first error)
- Detailed error reporting with file names and error messages
- Transaction support for atomic operations

### Per-File Configuration

- Individual descriptions for each file
- Individual categories for each file
- Individual attachment types for each file
- Fallback to default values when individual values not provided

### Progress Tracking

- Success and failure counts
- Detailed result summary
- List of successful uploads with full attachment details
- List of failed uploads with error details

## Request Format

```http
POST /api/attachments/{entityType}/{entityId}/bulk
Content-Type: multipart/form-data

Form Fields:
- files: IFormFile[] (required) - The files to upload
- Descriptions: string[] (optional) - Individual descriptions for each file
- Categories: string[] (optional) - Individual categories for each file
- AttachmentTypes: string[] (optional) - Individual attachment types for each file
- DefaultCategory: string (optional) - Default category for files without individual categories
- DefaultAttachmentType: string (optional) - Default attachment type for files without individual types
- ContinueOnError: boolean (optional, default: true) - Whether to continue uploading when a file fails
```

## Response Format

```json
{
  "totalFiles": 3,
  "successCount": 2,
  "failureCount": 1,
  "successfulUploads": [
    {
      "attachmentId": 123,
      "fileName": "file1.jpg",
      "originalFileName": "document.jpg",
      "contentType": "image/jpeg",
      "fileSize": 1024,
      "description": "File description",
      "category": "Evidence",
      "attachmentType": "Image",
      "createdAt": "2025-09-08T10:30:00"
    }
  ],
  "errors": [
    {
      "fileName": "invalid.exe",
      "fileIndex": 2,
      "error": "File type not allowed",
      "details": "Executable files are not permitted"
    }
  ],
  "hasErrors": true,
  "summary": "Successfully uploaded 2 of 3 files"
}
```

## Error Handling Modes

### Continue on Error (ContinueOnError: true)

- Uploads all valid files
- Collects errors for invalid files
- Returns partial success with error details
- No transaction rollback

### Stop on Error (ContinueOnError: false)

- Uses database transaction
- Stops on first error
- Rolls back all uploads on failure
- All-or-nothing behavior

## Comparison with Existing Endpoints

| Feature                 | Single Upload | Multiple Upload | **Bulk Upload** |
| ----------------------- | ------------- | --------------- | --------------- |
| File Count              | 1             | Multiple        | Multiple        |
| Individual Descriptions | ✅            | ❌              | ✅              |
| Individual Categories   | ✅            | ❌              | ✅              |
| Error Handling          | Simple        | Basic           | Advanced        |
| Transaction Support     | N/A           | ❌              | ✅              |
| Progress Tracking       | N/A           | Basic           | Detailed        |
| Failure Recovery        | N/A           | ❌              | ✅              |

## Usage Examples

### JavaScript/Fetch Example

```javascript
const formData = new FormData();

// Add files
files.forEach((file) => formData.append("files", file));

// Add individual descriptions
descriptions.forEach((desc) => formData.append("Descriptions", desc));

// Add individual categories
categories.forEach((cat) => formData.append("Categories", cat));

// Add defaults
formData.append("DefaultCategory", "General");
formData.append("DefaultAttachmentType", "Document");
formData.append("ContinueOnError", "true");

const response = await fetch("/api/attachments/Complain/123/bulk", {
  method: "POST",
  body: formData,
});

const result = await response.json();
console.log(`Uploaded ${result.successCount} of ${result.totalFiles} files`);
```

### C# Example

```csharp
var request = new BulkUploadRequest
{
    Files = files,
    Descriptions = new[] { "Doc 1", "Doc 2", "Doc 3" },
    Categories = new[] { "Evidence", "Document", "Photo" },
    DefaultCategory = "General",
    DefaultAttachmentType = "Document",
    ContinueOnError = true
};

var result = await attachmentService.BulkUploadAttachmentsAsync(
    request, "Complain", 123);
```

## File Validation

- Maximum file size: 10MB per file
- Supported file types: Images, documents, text files
- File name length: Maximum 255 characters
- Total request size: Limited by server configuration

## Best Practices

1. **Use bulk upload for multiple files** with different metadata requirements
2. **Use ContinueOnError: false** for critical uploads that must be atomic
3. **Provide individual descriptions** for better file organization
4. **Set appropriate categories** for easier file filtering and searching
5. **Handle errors gracefully** in client applications
6. **Monitor file sizes** to avoid timeouts on large uploads

## Error Codes

- **400 Bad Request**: Invalid request format or validation errors
- **413 Payload Too Large**: File size exceeds limits
- **415 Unsupported Media Type**: File type not allowed
- **500 Internal Server Error**: Server-side processing errors

## Migration from Multiple Upload

The existing `/multiple` endpoint remains available for backward compatibility. New applications should use the `/bulk` endpoint for enhanced features.

```http
# Old way
POST /api/attachments/Complain/123/multiple

# New way (recommended)
POST /api/attachments/Complain/123/bulk
```
