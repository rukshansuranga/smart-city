using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PSMModel.Models;

namespace PSMDataAccess.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.CommentId);
        
        builder.Property(c => c.Text)
            .IsRequired()
            .HasMaxLength(2000);
            
        builder.Property(c => c.EntityType)
            .IsRequired()
            .HasConversion<string>();
            
        builder.Property(c => c.EntityId)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(c => c.IsPrivate)
            .HasDefaultValue(false);
            
        builder.Property(c => c.Type)
            .HasConversion<string>();
            
        // Index for better query performance
        builder.HasIndex(c => new { c.EntityType, c.EntityId });
        
        // Foreign key relationships
        builder.HasOne(c => c.Client)
            .WithMany()
            .HasForeignKey(c => c.ClientId)
            .OnDelete(DeleteBehavior.SetNull);
            
        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
