using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;
using System;

namespace Commands.Produto.RemoverProduto
{
    public class RemoverProdutoResquest : IRequest<Response>
    {
        public RemoverProdutoResquest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
