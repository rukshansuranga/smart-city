using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(a => a.AttachmentId);
        
        builder.Property(a => a.FileName)
            .IsRequired()
            .HasMaxLength(255);
            
        builder.Property(a => a.OriginalFileName)
            .IsRequired()
            .HasMaxLength(255);
            
        builder.Property(a => a.FilePath)
            .IsRequired()
            .HasMaxLength(500);
            
        builder.Property(a => a.ContentType)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(a => a.EntityType)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(a => a.Description)
            .HasMaxLength(500);
            
        builder.Property(a => a.AttachmentType)
            .HasMaxLength(50);
            
        builder.Property(a => a.Category)
            .HasMaxLength(100);
            
        // Create index for better query performance
        builder.HasIndex(a => new { a.EntityType, a.EntityId })
            .HasDatabaseName("IX_Attachment_EntityType_EntityId");
            
        builder.HasIndex(a => a.EntityType)
            .HasDatabaseName("IX_Attachment_EntityType");
    }
}
