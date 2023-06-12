using MediatR;
using Sis_Pdv_Controle_Estoque.Commands;
using System;

namespace Commands.Fornecedor.RemoverFornecedor
{
    public class RemoverFornecedorResquest : IRequest<Response>
    {
        public RemoverFornecedorResquest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
