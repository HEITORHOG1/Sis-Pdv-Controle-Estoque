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
            builder.Property(x => x.codBarras).HasMaxLength(150).IsRequired();
            builder.Property(x => x.nomeProduto).HasMaxLength(150).IsRequired();
            builder.Property(x => x.descricaoProduto).HasMaxLength(150).IsRequired();
            builder.Property(x => x.precoCusto).IsRequired();
            builder.Property(x => x.precoVenda).IsRequired();
            builder.Property(x => x.margemLucro).IsRequired();
            builder.Property(x => x.dataFabricao).IsRequired();
            builder.Property(x => x.dataVencimento).IsRequired();
            builder.Property(x => x.quatidadeEstoqueProduto).IsRequired();
            builder.Property(x => x.statusAtivo).IsRequired();
            builder.Property(x => x.FornecedorId).IsRequired();
            builder.Property(x => x.CategoriaId).IsRequired();
        }
    }
}
