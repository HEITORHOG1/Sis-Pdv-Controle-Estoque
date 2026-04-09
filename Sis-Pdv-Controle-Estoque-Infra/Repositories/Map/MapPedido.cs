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

            // Indexes para consultas frequentes
            builder.HasIndex(x => x.Status).HasDatabaseName("IX_Pedido_Status");
            builder.HasIndex(x => x.DataDoPedido).HasDatabaseName("IX_Pedido_DataDoPedido");
            builder.HasIndex(x => x.ClienteId).HasDatabaseName("IX_Pedido_ClienteId");
            builder.HasIndex(x => x.ColaboradorId).HasDatabaseName("IX_Pedido_ColaboradorId");
        }
    }
}
