using System;
using Model.Exceptions;

namespace Model
{
    public class Cupom : EntityBase
    {
        public Cupom()
        {
        }

        public Cupom(Guid pedidoId, DateTime dataEmissao, string numeroSerie, string chaveAcesso, decimal valorTotal, string documentoCliente)
        {
            ValidarPedidoId(pedidoId);
            ValidarNumeroSerie(numeroSerie);
            ValidarChaveAcesso(chaveAcesso);

            PedidoId = pedidoId;
            DataEmissao = dataEmissao;
            NumeroSerie = numeroSerie;
            ChaveAcesso = chaveAcesso;
            ValorTotal = valorTotal;
            DocumentoCliente = documentoCliente;
        }

        public Guid PedidoId { get; set; }
        public DateTime DataEmissao { get; set; }
        public string NumeroSerie { get; set; } = string.Empty;
        public string ChaveAcesso { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public string DocumentoCliente { get; set; } = string.Empty;

        // Navigation property
        public virtual Pedido? Pedido { get; set; }

        public void AlterarCupom(Guid pedidoId, DateTime dataEmissao, string numeroSerie, string chaveAcesso, decimal valorTotal, string documentoCliente)
        {
            ValidarPedidoId(pedidoId);
            ValidarNumeroSerie(numeroSerie);
            ValidarChaveAcesso(chaveAcesso);

            PedidoId = pedidoId;
            DataEmissao = dataEmissao;
            NumeroSerie = numeroSerie;
            ChaveAcesso = chaveAcesso;
            ValorTotal = valorTotal;
            DocumentoCliente = documentoCliente;
        }

        private static void ValidarPedidoId(Guid pedidoId)
        {
            if (pedidoId == Guid.Empty)
                throw new DomainException("O Id do pedido é obrigatório.");
        }

        private static void ValidarNumeroSerie(string numeroSerie)
        {
            if (string.IsNullOrWhiteSpace(numeroSerie))
                throw new DomainException("O número de série é obrigatório.");
        }

        private static void ValidarChaveAcesso(string chaveAcesso)
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso))
                throw new DomainException("A chave de acesso é obrigatória.");
        }
    }
}
