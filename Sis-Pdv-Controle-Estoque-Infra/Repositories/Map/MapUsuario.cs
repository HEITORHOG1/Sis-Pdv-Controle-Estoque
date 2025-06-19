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
            builder.Property(x => x.Senha).HasMaxLength(150).IsRequired();
            builder.Property(x => x.StatusAtivo).IsRequired();
        }
    }
}
