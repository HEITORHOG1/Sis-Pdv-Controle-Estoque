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
