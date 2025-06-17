using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Map
{
    public class MapCategoria : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categoria");

            ////Propriedades
            builder.HasKey(x => x.Id);
            builder.Property(x => x.NomeCategoria).HasMaxLength(150).IsRequired();
        }
    }
}
