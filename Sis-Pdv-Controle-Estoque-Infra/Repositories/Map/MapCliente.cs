using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Map
{
    public class MapCliente : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Cliente");

            ////Propriedades
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CpfCnpj).HasMaxLength(50).IsRequired();
            builder.Property(x => x.TipoCliente).IsRequired();
        }
    }
}
