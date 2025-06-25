using System;

namespace Model
{
    public class Cupom : EntityBase
    {
        public Cupom()
        {
            
        }

        public Cupom(Guid pedidoId, DateTime dataEmissao, string numeroSerie, string chaveAcesso, decimal valorTotal, string documentoCliente)
        {
            PedidoId = pedidoId;
            DataEmissao = dataEmissao;
            NumeroSerie = numeroSerie;
            ChaveAcesso = chaveAcesso;
            ValorTotal = valorTotal;
            DocumentoCliente = documentoCliente;
        }

        public Guid PedidoId { get; set; }
        public DateTime DataEmissao { get; set; }
        public string NumeroSerie { get; set; }
        public string ChaveAcesso { get; set; }
        public decimal ValorTotal { get; set; }
        public string DocumentoCliente { get; set; }
        
        // Navigation property
        public virtual Pedido Pedido { get; set; }
    }
}
