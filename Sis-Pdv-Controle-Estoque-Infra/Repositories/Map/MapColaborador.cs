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
            builder.Property(x => x.NomeColaborador).HasMaxLength(150).IsRequired();
            builder.Property(x => x.EmailCorporativo).HasMaxLength(50).IsRequired();
            builder.Property(x => x.EmailPessoalColaborador).HasMaxLength(50).IsRequired();
            builder.Property(x => x.TelefoneColaborador).HasMaxLength(30).IsRequired();
            builder.Property(x => x.CargoColaborador).HasMaxLength(150).IsRequired();
            builder.Property(x => x.CpfColaborador).HasMaxLength(11).IsRequired();

            builder.HasOne(x => x.Usuario).WithMany().HasForeignKey("UsuarioId");
        }
    }
}
