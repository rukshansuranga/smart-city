using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSMModel.Models;

public class Attachment : BaseEntity
{
    [Key]
    public int AttachmentId { get; set; }
    
    [Required]
    [MaxLength(255)]
    public required string FileName { get; set; }
    
    [Required]
    [MaxLength(255)]
    public required string OriginalFileName { get; set; }
    
    [Required]
    [MaxLength(500)]
    public required string FilePath { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string ContentType { get; set; }
    
    public long FileSize { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    // Generic foreign key properties
    [Required]
    [MaxLength(50)]
    public required string EntityType { get; set; } // "Complain", "Ticket", "Project", etc.
    
    [Required]
    public int EntityId { get; set; } // The ID of the related entity
    
    [MaxLength(50)]
    public string? AttachmentType { get; set; } // "Document", "Image", "Video", etc.
    
    // Optional: For organization and categorization
    [MaxLength(100)]
    public string? Category { get; set; } // "Specification", "Progress", "Evidence", etc.
    
    public int? OrderIndex { get; set; } // For ordering attachments
}
