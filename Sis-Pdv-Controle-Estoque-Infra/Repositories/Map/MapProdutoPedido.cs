using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Map
{
    public class MapProdutoPedido : IEntityTypeConfiguration<ProdutoPedido>
    {
        public void Configure(EntityTypeBuilder<ProdutoPedido> builder)
        {
            builder.ToTable("ProdutoPedido");
            ////Propriedades
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CodBarras).HasMaxLength(150).IsRequired();
            builder.Property(x => x.QuantidadeItemPedido).IsRequired();
            builder.Property(x => x.TotalProdutoPedido).IsRequired();
            builder.Property(x => x.PedidoId).IsRequired();
            builder.Property(x => x.ProdutoId).IsRequired();

        }
    }
}
