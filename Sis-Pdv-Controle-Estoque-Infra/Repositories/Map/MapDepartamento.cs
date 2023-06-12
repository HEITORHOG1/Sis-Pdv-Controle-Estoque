using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sis_Pdv_Controle_Estoque.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories.Map
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
