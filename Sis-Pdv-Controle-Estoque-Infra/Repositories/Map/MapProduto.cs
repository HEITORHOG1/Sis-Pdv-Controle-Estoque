using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Map
{
    public class MapProduto : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produto");
            ////Propriedades
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CodBarras).HasMaxLength(150).IsRequired();
            builder.Property(x => x.NomeProduto).HasMaxLength(150).IsRequired();
            builder.Property(x => x.DescricaoProduto).HasMaxLength(150).IsRequired();
            builder.Property(x => x.PrecoCusto).IsRequired();
            builder.Property(x => x.PrecoVenda).IsRequired();
            builder.Property(x => x.MargemLucro).IsRequired();
            builder.Property(x => x.DataFabricao).IsRequired();
            builder.Property(x => x.DataVencimento).IsRequired();
            builder.Property(x => x.QuatidadeEstoqueProduto).IsRequired();
            builder.Property(x => x.StatusAtivo).IsRequired();
            builder.Property(x => x.FornecedorId).IsRequired();
            builder.Property(x => x.CategoriaId).IsRequired();
        }
    }
}
