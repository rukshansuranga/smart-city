# Multiple Attachment Upload Component

A reusable React component for handling multiple file uploads with drag-and-drop functionality, file validation, and categorization.

## Features

- **Drag & Drop Support**: Easy file upload with drag and drop interface
- **File Validation**: Configurable file type and size validation
- **Multiple File Support**: Upload multiple files with customizable limits
- **File Categorization**: Assign types and categories to uploaded files
- **Existing Files Display**: Show previously uploaded files with download functionality
- **Progress Feedback**: Toast notifications for upload status
- **Responsive Design**: Works well on desktop and mobile devices

## Usage

### Basic Usage

```tsx
import MultipleAttachmentUpload from "@/app/components/forms/MultipleAttachmentUpload";
import { useForm } from "react-hook-form";

const MyForm = () => {
  const { control } = useForm();

  return (
    <MultipleAttachmentUpload
      name="attachments"
      control={control}
      entityType="Ticket"
      entityId={123}
      label="Upload Files"
      showlabel={true}
    />
  );
};
```

### Advanced Usage with Custom Configuration

```tsx
<MultipleAttachmentUpload
  name="attachments"
  control={control}
  entityType="Project"
  entityId={456}
  label="Project Documents"
  showlabel={true}
  maxFiles={10}
  maxFileSize={25} // 25MB
  acceptedFileTypes={[".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png"]}
  allowedCategories={["Specification", "Drawing", "Report", "Other"]}
  allowedAttachmentTypes={["Document", "Image", "Technical"]}
  existingAttachments={existingFiles}
  onAttachmentsChange={(attachments) => {
    console.log("New attachments:", attachments);
  }}
/>
```

## Props

| Prop                     | Type           | Default            | Description                                         |
| ------------------------ | -------------- | ------------------ | --------------------------------------------------- |
| `name`                   | `string`       | Required           | Form field name                                     |
| `control`                | `Control`      | Required           | React Hook Form control object                      |
| `entityType`             | `string`       | Required           | Entity type (e.g., "Ticket", "Project", "Complain") |
| `entityId`               | `number`       | Optional           | Entity ID for existing entities                     |
| `label`                  | `string`       | "Attachments"      | Label text for the component                        |
| `showlabel`              | `boolean`      | `true`             | Whether to show the label                           |
| `className`              | `string`       | Optional           | Additional CSS classes                              |
| `required`               | `boolean`      | `false`            | Whether the field is required                       |
| `maxFiles`               | `number`       | `10`               | Maximum number of files allowed                     |
| `maxFileSize`            | `number`       | `10`               | Maximum file size in MB                             |
| `acceptedFileTypes`      | `string[]`     | Common file types  | Allowed file extensions                             |
| `allowedCategories`      | `string[]`     | Default categories | Available file categories                           |
| `allowedAttachmentTypes` | `string[]`     | Default types      | Available attachment types                          |
| `existingAttachments`    | `Attachment[]` | `[]`               | Previously uploaded files                           |
| `onAttachmentsChange`    | `function`     | Optional           | Callback when attachments change                    |

## Backend Integration

### API Endpoints

The component expects the following backend endpoints:

#### Upload Attachments

```
POST /api/attachments/{entityType}/{entityId}
Content-Type: multipart/form-data

FormData:
- files: File[]
- descriptions[index]: string
- attachmentTypes[index]: string
- categories[index]: string
```

#### Get Attachments

```
GET /api/attachments/{entityType}/{entityId}
```

#### Download Attachment

```
GET /api/attachments/{attachmentId}/download
```

### Entity Implementation

Your entities should implement the `IAttachable` interface:

```csharp
public class YourEntity : BaseEntity, IAttachable
{
    public int Id { get; set; }
    public virtual ICollection<Attachment>? Attachments { get; set; }

    public int GetEntityId() => Id;
    public string GetEntityType() => nameof(YourEntity);
}
```

### Database Schema

The component works with the following attachment model:

```csharp
public class Attachment : BaseEntity
{
    [Key]
    public int AttachmentId { get; set; }

    [Required, MaxLength(255)]
    public required string FileName { get; set; }

    [Required, MaxLength(255)]
    public required string OriginalFileName { get; set; }

    [Required, MaxLength(500)]
    public required string FilePath { get; set; }

    [Required, MaxLength(100)]
    public required string ContentType { get; set; }

    public long FileSize { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required, MaxLength(50)]
    public required string EntityType { get; set; }

    [Required]
    public int EntityId { get; set; }

    [MaxLength(50)]
    public string? AttachmentType { get; set; }

    [MaxLength(100)]
    public string? Category { get; set; }

    public int? OrderIndex { get; set; }
}
```

## Form Integration

### React Hook Form Schema

```tsx
import * as z from "zod";

const schema = z.object({
  // ... other fields
  attachments: z.array(z.any()).optional(),
});

type FormData = z.infer<typeof schema>;
```

### Form Submission

```tsx
const onSubmit = async (data: FormData) => {
  // Handle other form data first
  const response = await createEntity(data);

  if (response.isSuccess && data.attachments?.length > 0) {
    // Upload attachments after entity creation
    await uploadAttachmentsClient(
      "YourEntityType",
      response.data.id,
      data.attachments
    );
  }
};
```

## Styling

The component uses Flowbite React components and Tailwind CSS. Key styling features:

- **Drag States**: Visual feedback during drag operations
- **Validation States**: Error styling for validation failures
- **File Icons**: Automatic icon selection based on file type
- **Responsive Layout**: Adapts to different screen sizes

## Error Handling

The component provides comprehensive error handling:

- **File Size Validation**: Prevents oversized files
- **File Type Validation**: Restricts to allowed file types
- **Upload Limits**: Enforces maximum file count
- **Network Errors**: Graceful handling of API failures
- **User Feedback**: Toast notifications for all operations

## Example Integration in TicketForm

```tsx
// In your form component
<MultipleAttachmentUpload
  name="attachments"
  control={control}
  entityType="Ticket"
  entityId={ticket?.ticketId}
  label="Ticket Attachments"
  showlabel={true}
  maxFiles={5}
  maxFileSize={10}
  acceptedFileTypes={[".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png", ".txt"]}
  allowedCategories={["Document", "Image", "Evidence", "Other"]}
  allowedAttachmentTypes={["Evidence", "Report", "Specification", "Progress"]}
  existingAttachments={existingAttachments}
  onAttachmentsChange={(attachments) => {
    console.log("Attachments updated:", attachments);
  }}
/>
```

## Dependencies

- React Hook Form
- Flowbite React
- React Hot Toast
- React Icons (Heroicons)
- Tailwind CSS

## Notes

- Files are validated on the client side before upload
- The component handles both new uploads and existing file display
- All file operations provide user feedback via toast notifications
- The component is fully accessible and keyboard navigable
