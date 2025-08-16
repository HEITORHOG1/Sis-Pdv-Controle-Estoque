using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Map
{
    public class MapUserSession : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {
            builder.ToTable("UserSessions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .HasColumnName("Id");

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasColumnName("UserId");

            builder.Property(x => x.SessionToken)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("SessionToken");

            builder.Property(x => x.IpAddress)
                .HasMaxLength(45)
                .HasColumnName("IpAddress");

            builder.Property(x => x.UserAgent)
                .HasMaxLength(500)
                .HasColumnName("UserAgent");

            builder.Property(x => x.LoginAt)
                .IsRequired()
                .HasColumnName("LoginAt");

            builder.Property(x => x.LogoutAt)
                .HasColumnName("LogoutAt");

            builder.Property(x => x.ExpiresAt)
                .IsRequired()
                .HasColumnName("ExpiresAt");

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasColumnName("IsActive");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("CreatedBy");

            builder.Property(x => x.UpdatedBy)
                .HasColumnName("UpdatedBy");

            builder.Property(x => x.IsDeleted)
                .IsRequired()
                .HasColumnName("IsDeleted");

            builder.Property(x => x.DeletedAt)
                .HasColumnName("DeletedAt");

            builder.Property(x => x.DeletedBy)
                .HasColumnName("DeletedBy");

            // Relationships
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.SessionToken).IsUnique();
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.ExpiresAt);
        }
    }
}