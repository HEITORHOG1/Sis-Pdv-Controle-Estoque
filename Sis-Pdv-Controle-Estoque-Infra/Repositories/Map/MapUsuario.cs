using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Map
{
    public class MapUsuario : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Login).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Senha).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Nome).HasMaxLength(255).IsRequired();
            builder.Property(x => x.StatusAtivo).IsRequired();
            builder.Property(x => x.LastLoginAt);
            builder.Property(x => x.RefreshToken).HasMaxLength(500);
            builder.Property(x => x.RefreshTokenExpiryTime);
            
            // Audit fields
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt);
            builder.Property(x => x.CreatedBy);
            builder.Property(x => x.UpdatedBy);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.DeletedAt);
            builder.Property(x => x.DeletedBy);

            // Indexes
            builder.HasIndex(x => x.Login).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.RefreshToken);

            // Relationships
            builder.HasMany(x => x.UserRoles)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
