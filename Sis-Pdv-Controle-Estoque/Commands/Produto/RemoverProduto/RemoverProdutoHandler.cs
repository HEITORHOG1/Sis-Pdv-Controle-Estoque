﻿using Commands.Produto.RemoverProduto;
using MediatR;
using prmToolkit.NotificationPattern;
using prmToolkit.NotificationPattern.Extensions;
using Sis_Pdv_Controle_Estoque.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Commands.Produto.RemoverProduto

{
    public class RemoverProdutoHandler : Notifiable, IRequestHandler<RemoverProdutoResquest, Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProduto _repositoryProduto;

        public RemoverProdutoHandler(IMediator mediator, IRepositoryProduto repositoryProduto)
        {
            _mediator = mediator;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<Sis_Pdv_Controle_Estoque.Commands.Response> Handle(RemoverProdutoResquest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            Sis_Pdv_Controle_Estoque.Model.Produto Produto = _repositoryProduto.ObterPorId(request.Id);

            if (Produto == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            _repositoryProduto.Remover(Produto);

            var result = new { Id = Produto.Id };

            //Cria objeto de resposta
            var response = new Sis_Pdv_Controle_Estoque.Commands.Response(this, result);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}
