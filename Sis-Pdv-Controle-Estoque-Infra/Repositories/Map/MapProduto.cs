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
            builder.Property(x => x.QuantidadeEstoqueProduto).IsRequired();
            builder.Property(x => x.MinimumStock).HasPrecision(18, 2).HasDefaultValue(0);
            builder.Property(x => x.MaximumStock).HasPrecision(18, 2).HasDefaultValue(0);
            builder.Property(x => x.ReorderPoint).HasPrecision(18, 2).HasDefaultValue(0);
            builder.Property(x => x.Location).HasMaxLength(100);
            builder.Property(x => x.StatusAtivo).IsRequired();
            builder.Property(x => x.FornecedorId).IsRequired();
            builder.Property(x => x.CategoriaId).IsRequired();
            
            // Relationships
            builder.HasMany(x => x.StockMovements)
                   .WithOne(x => x.Produto)
                   .HasForeignKey(x => x.ProdutoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
