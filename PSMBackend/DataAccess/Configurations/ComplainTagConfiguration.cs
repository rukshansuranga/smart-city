using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class ComplainTagConfiguration : IEntityTypeConfiguration<ComplainTag>
{
    public void Configure(EntityTypeBuilder<ComplainTag> builder)
    {
        // Composite primary key
        builder.HasKey(ct => new { ct.ComplainId, ct.TagId });
        
        // Configure relationships
        builder.HasOne(ct => ct.Complain)
            .WithMany(c => c.ComplainTags)
            .HasForeignKey(ct => ct.ComplainId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(ct => ct.Tag)
            .WithMany(t => t.ComplainTags)
            .HasForeignKey(ct => ct.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
