using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class ProjectTagConfiguration : IEntityTypeConfiguration<ProjectTag>
{
    public void Configure(EntityTypeBuilder<ProjectTag> builder)
    {
        // Composite primary key
        builder.HasKey(pt => new { pt.ProjectId, pt.TagId });
        
        // Configure relationships
        builder.HasOne(pt => pt.Project)
            .WithMany(p => p.ProjectTags)
            .HasForeignKey(pt => pt.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(pt => pt.Tag)
            .WithMany(t => t.ProjectTags)
            .HasForeignKey(pt => pt.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
