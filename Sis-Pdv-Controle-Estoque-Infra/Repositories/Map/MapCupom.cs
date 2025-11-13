using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Map
{
    public class MapCupom : IEntityTypeConfiguration<Cupom>
    {
        public void Configure(EntityTypeBuilder<Cupom> builder)
        {
            builder.ToTable("Cupom");
            
            // Propriedades
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PedidoId).IsRequired();
            builder.Property(x => x.DataEmissao).IsRequired();
            builder.Property(x => x.NumeroSerie).HasMaxLength(50).IsRequired();
            builder.Property(x => x.ChaveAcesso).HasMaxLength(100).IsRequired();
            builder.Property(x => x.ValorTotal).HasColumnType("decimal(10,2)").IsRequired();
            builder.Property(x => x.DocumentoCliente).HasMaxLength(20);

            // Relacionamentos
            builder.HasOne(x => x.Pedido)
                  .WithOne()
                  .HasForeignKey<Cupom>(x => x.PedidoId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
