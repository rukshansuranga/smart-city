using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class TicketTagConfiguration : IEntityTypeConfiguration<TicketTag>
{
    public void Configure(EntityTypeBuilder<TicketTag> builder)
    {
        // Composite primary key
        builder.HasKey(tt => new { tt.TicketId, tt.TagId });
        
        // Configure relationships
        builder.HasOne(tt => tt.Ticket)
            .WithMany(t => t.TicketTags)
            .HasForeignKey(tt => tt.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(tt => tt.Tag)
            .WithMany(t => t.TicketTags)
            .HasForeignKey(tt => tt.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
