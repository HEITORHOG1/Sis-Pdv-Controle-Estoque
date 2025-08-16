using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Map
{
    public class MapAuditLog : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLog");
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.EntityName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.EntityId).IsRequired();
            builder.Property(x => x.Action).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Changes).HasColumnType("TEXT");
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Timestamp).IsRequired();
            builder.Property(x => x.OldValues).HasColumnType("TEXT");
            builder.Property(x => x.NewValues).HasColumnType("TEXT");
            
            // Audit fields
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt);
            builder.Property(x => x.CreatedBy);
            builder.Property(x => x.UpdatedBy);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.DeletedAt);
            builder.Property(x => x.DeletedBy);

            // Indexes
            builder.HasIndex(x => new { x.EntityName, x.EntityId });
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Timestamp);

            // Relationships
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}