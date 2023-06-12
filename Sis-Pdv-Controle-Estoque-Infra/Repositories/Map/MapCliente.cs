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
    public  class MapCliente : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Cliente");

            ////Propriedades
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CpfCnpj).HasMaxLength(50).IsRequired();
            builder.Property(x => x.tipoCliente).IsRequired();
        }
    }
}
