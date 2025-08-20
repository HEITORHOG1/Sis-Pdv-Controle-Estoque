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
            builder.Property(x => x.IsPerecivel).HasDefaultValue(false);
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
