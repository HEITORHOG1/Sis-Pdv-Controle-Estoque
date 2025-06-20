﻿using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Fornecedor.ListarFornecedorPorId
{
    public class ListarFornecedorPorIdHandler : Notifiable, IRequestHandler<ListarFornecedorPorIdRequest, ListarFornecedorPorIdResponse>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryFornecedor _repositoryFornecedor;

        public ListarFornecedorPorIdHandler(IMediator mediator, IRepositoryFornecedor repositoryFornecedor)
        {
            _mediator = mediator;
            _repositoryFornecedor = repositoryFornecedor;
        }

        public async Task<ListarFornecedorPorIdResponse> Handle(ListarFornecedorPorIdRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return null;
            }

            Model.Fornecedor Collection = _repositoryFornecedor.ObterPor(x => x.Id == request.Id);

            if (Collection == null)
            {
                AddNotification("Request", "");
                return null;
            }

            //Cria objeto de resposta
            var response = (ListarFornecedorPorIdResponse)Collection;

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

