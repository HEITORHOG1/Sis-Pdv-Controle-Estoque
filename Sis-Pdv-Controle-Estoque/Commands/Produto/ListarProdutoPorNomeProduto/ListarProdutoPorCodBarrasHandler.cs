using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Interfaces;
using System.Text.RegularExpressions;

namespace Commands.Produto.ListarProdutoPorCodBarras
{
    public class ListarProdutoPorCodBarrasHandler : Notifiable, IRequestHandler<ListarProdutoPorCodBarrasRequest, Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProduto _repositoryProduto;

        public ListarProdutoPorCodBarrasHandler(IMediator mediator, IRepositoryProduto repositoryProduto)
        {
            _mediator = mediator;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<Sis_Pdv_Controle_Estoque.Commands.Response> Handle(ListarProdutoPorCodBarrasRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            var Collection = _repositoryProduto.Listar().Where(x => x.codBarras == request.codBarras); 
            if (!Collection.Any()) 
            {
                AddNotification("ATENÇÃO", "PRODUTO NÃO ENCONTRADA");
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


