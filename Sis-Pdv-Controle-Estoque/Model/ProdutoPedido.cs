using Sis_Pdv_Controle_Estoque.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Model
{
    [Table("ProdutoPedido")]
    public class ProdutoPedido : EntityBase
    {
        public ProdutoPedido()
        {
           
        }
        public ProdutoPedido(Guid pedidoId,
                             Guid produtoId,
                            string codBarras,
                            int quantidadeItemPedido,
                            decimal totalProdutoPedido)
        {
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            this.codBarras = codBarras;
            this.quantidadeItemPedido = quantidadeItemPedido;
            this.totalProdutoPedido = totalProdutoPedido;
        }



        public virtual Pedido? Pedido { get; set; }
        public virtual Produto? Produto { get; set; }
        public  Guid PedidoId { get; set; }
        public  Guid ProdutoId { get; set; }
        public string? codBarras { get; set; }
        public int? quantidadeItemPedido { get; set; }
        public decimal? totalProdutoPedido { get; set; }


        internal void AlterarProdutoPedido(Guid pedidoId, Guid produtoId, string codBarras, int quantidadeItemPedido, decimal totalProdutoPedido)
        {
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            Pedido = new Pedido { Id = pedidoId };
            Produto = new Produto { Id = produtoId };
            this.codBarras = codBarras;
            this.quantidadeItemPedido = quantidadeItemPedido;
            this.totalProdutoPedido = totalProdutoPedido;
        }
    }
}
