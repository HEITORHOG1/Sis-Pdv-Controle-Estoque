using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Map
{
    public class MapDepartamento : IEntityTypeConfiguration<Departamento>
    {
        public void Configure(EntityTypeBuilder<Departamento> builder)
        {
            builder.ToTable("Departamento");
            ////Propriedades
            builder.HasKey(x => x.Id);
            builder.Property(x => x.NomeDepartamento).HasMaxLength(150).IsRequired();
        }
    }
}
