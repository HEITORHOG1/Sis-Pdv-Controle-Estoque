﻿using AdicionarCliente;
using Commands.Categoria.AdicionarCategoria;
using Commands.Categoria.AdicionarCliente;
using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Interfaces;
using Sis_Pdv_Controle_Estoque.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sis_Pdv_Controle_Estoque.Commands.Cliente
{
    public class AdicionarClienteHandler : Notifiable, IRequestHandler<AdicionarClienteRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryCliente _repositoryCliente;

        public AdicionarClienteHandler(IMediator mediator, IRepositoryCliente repositoryCliente)
        {
            _mediator = mediator;
            _repositoryCliente = repositoryCliente;
        }

        public async Task<Response> Handle(AdicionarClienteRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AdicionarClienteRequestValidator();

            // Valida a requisição
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            // Se não passou na validação, adiciona as falhas como notificações
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    AddNotification(error.PropertyName, error.ErrorMessage);
                }

                return new Response(this);
            }

            //Verificar se o usuário já existe
            if (_repositoryCliente.Existe(x => x.CpfCnpj == request.CpfCnpj))
            {
                AddNotification("NomeCategoria", "Categoria ja Cadastrada");
                return new Response(this);
            }

            Model.Cliente cliente = new(request.CpfCnpj, request.TipoCliente);

            if (IsInvalid())
            {
                return new Response(this);
            }
            cliente = _repositoryCliente.Adicionar(cliente);

            //Criar meu objeto de resposta
            var response = new Response(this, cliente);

            return await Task.FromResult(response);
        }
    }
}
