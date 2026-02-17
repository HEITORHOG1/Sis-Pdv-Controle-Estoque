using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Map
{
    public class MapPedido : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedido");
            ////Propriedades
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Status).HasMaxLength(50).IsRequired();
            builder.Property(x => x.DataDoPedido).IsRequired();
            builder.Property(x => x.FormaPagamento).HasMaxLength(150).IsRequired();
            builder.Property(x => x.TotalPedido).IsRequired();
            builder.Property(x => x.ColaboradorId).IsRequired();
            builder.Property(x => x.ClienteId).IsRequired(false);

            // FK Cliente nullable — permite vendas sem cliente (consumidor final)
            builder.HasOne(x => x.Cliente)
                .WithMany()
                .HasForeignKey(x => x.ClienteId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
