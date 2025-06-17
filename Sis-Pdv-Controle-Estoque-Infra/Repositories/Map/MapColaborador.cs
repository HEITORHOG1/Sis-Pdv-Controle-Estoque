using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Map
{
    public class MapColaborador : IEntityTypeConfiguration<Colaborador>
    {
        public void Configure(EntityTypeBuilder<Colaborador> builder)
        {
            builder.ToTable("Colaborador");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.nomeColaborador).HasMaxLength(150).IsRequired();
            builder.Property(x => x.emailCorporativo).HasMaxLength(50).IsRequired();
            builder.Property(x => x.emailPessoalColaborador).HasMaxLength(50).IsRequired();
            builder.Property(x => x.telefoneColaborador).HasMaxLength(30).IsRequired();
            builder.Property(x => x.cargoColaborador).HasMaxLength(150).IsRequired();
            builder.Property(x => x.cpfColaborador).HasMaxLength(11).IsRequired();

            builder.HasOne(x => x.Usuario).WithMany().HasForeignKey("UsuarioId");
        }
    }
}
