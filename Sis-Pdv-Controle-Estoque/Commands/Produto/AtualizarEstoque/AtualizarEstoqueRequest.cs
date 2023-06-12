using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;

namespace Commands.Produto.AtualizarEstoque
{
    public class AtualizarEstoqueRequest : IRequest<Response>
    {
       
        public Guid Id { get; set; }
        public int quatidadeEstoqueProduto { get; set; }
    }
}
