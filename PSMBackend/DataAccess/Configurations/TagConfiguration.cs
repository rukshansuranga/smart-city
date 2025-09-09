using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.TagId);
        
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(t => t.Description)
            .HasMaxLength(255);
            
        builder.Property(t => t.Color)
            .HasMaxLength(20);
        
        // Ensure unique tag names
        builder.HasIndex(t => t.Name)
            .IsUnique();
        
        // Seed some default tags
        builder.HasData(
            new Tag { TagId = 1, Name = "Urgent", Description = "High priority items", Color = "#FF4444", IsActive = true },
            new Tag { TagId = 2, Name = "Bug", Description = "Bug related issues", Color = "#FF8800", IsActive = true },
            new Tag { TagId = 3, Name = "Enhancement", Description = "Feature improvements", Color = "#00AA00", IsActive = true },
            new Tag { TagId = 4, Name = "Maintenance", Description = "Routine maintenance tasks", Color = "#0088FF", IsActive = true },
            new Tag { TagId = 5, Name = "Documentation", Description = "Documentation related", Color = "#8800FF", IsActive = true }
        );
    }
}
