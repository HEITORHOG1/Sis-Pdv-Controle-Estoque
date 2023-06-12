using AdicionarProduto;
using Commands.Produto.AdicionarProduto;
using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Interfaces;

namespace Sis_Pdv_Controle_Estoque.Commands.AdicionarProduto
{
    public class AdicionarProdutoHandler : Notifiable, IRequestHandler<AdicionarProdutoRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProduto _repositoryProduto;

        public AdicionarProdutoHandler(IMediator mediator, IRepositoryProduto repositoryProduto)
        {
            _mediator = mediator;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<Response> Handle(AdicionarProdutoRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AdicionarProdutoRequestValidator();

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
            if (_repositoryProduto.Existe(x => x.codBarras == request.codBarras))
            {
                AddNotification("codBarras", "codBarras ja Cadastrado");
                return new Response(this);
            }

            Model.Produto Produto = new (
                                request.codBarras, 
                                request.nomeProduto, 
                                request.descricaoProduto,
                                request.precoCusto, 
                                request.precoVenda, 
                                request.margemLucro,
                                request.dataFabricao, 
                                request.dataVencimento,
                                request.quatidadeEstoqueProduto,
                                request.FornecedorId ,
                                request.CategoriaId,
                                request.statusAtivo);

            if (IsInvalid())
            {
                return new Response(this);
            }

            Produto = _repositoryProduto.Adicionar(Produto);

            //Criar meu objeto de resposta
            var response = new Response(this, Produto);

            return await Task.FromResult(response);
        }
    }
}
