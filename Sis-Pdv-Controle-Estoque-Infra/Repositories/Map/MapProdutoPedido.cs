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
            builder.Property(x => x.codBarras).HasMaxLength(150).IsRequired();
            builder.Property(x => x.quantidadeItemPedido).IsRequired();
            builder.Property(x => x.totalProdutoPedido).IsRequired();
            builder.Property(x => x.PedidoId).IsRequired();
            builder.Property(x => x.ProdutoId).IsRequired();

        }
    }
}
