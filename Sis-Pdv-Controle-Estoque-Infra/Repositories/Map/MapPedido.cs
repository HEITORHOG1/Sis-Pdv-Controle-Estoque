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
    public class MapPedido : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedido");
            ////Propriedades
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Status).HasMaxLength(50).IsRequired();
            builder.Property(x => x.dataDoPedido).IsRequired();
            builder.Property(x => x.formaPagamento).HasMaxLength(150).IsRequired();
            builder.Property(x => x.totalPedido).IsRequired();
            builder.Property(x => x.ColaboradorId).IsRequired();
            builder.Property(x => x.ClienteId).IsRequired();
        }
    }
}
