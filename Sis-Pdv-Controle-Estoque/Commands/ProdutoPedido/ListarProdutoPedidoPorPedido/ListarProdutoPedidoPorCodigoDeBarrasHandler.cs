using MediatR;
using Microsoft.EntityFrameworkCore;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Interfaces;
using System.Text.RegularExpressions;

namespace Commands.ProdutoPedido.ListarProdutoPedidoPorNomeCpfCnpj
{
    public class ListarProdutoPedidoPorCodigoDeBarrasHandler : Notifiable, IRequestHandler<ListarProdutoPedidoPorCodigoDeBarrasRequest, Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProdutoPedido _repositoryProdutoPedido;

        public ListarProdutoPedidoPorCodigoDeBarrasHandler(IMediator mediator, IRepositoryProdutoPedido repositoryProdutoPedido)
        {
            _mediator = mediator;
            _repositoryProdutoPedido = repositoryProdutoPedido;
        }

        public async Task<Sis_Pdv_Controle_Estoque.Commands.Response> Handle(ListarProdutoPedidoPorCodigoDeBarrasRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            var Collection = _repositoryProdutoPedido.Listar()
                                .Where(x => x.codBarras == request.CodBarras); 
            if (!Collection.Any()) 
            {
                AddNotification("ATENÇÃO", "ProdutoPedido NÃO ENCONTRADA");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            //Criar meu objeto de resposta
            var response = new Sis_Pdv_Controle_Estoque.Commands.Response(this, Collection);
            //Cria objeto de resposta

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}


