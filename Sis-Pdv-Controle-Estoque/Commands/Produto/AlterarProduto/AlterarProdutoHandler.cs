using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Produto.AlterarProduto
{
    public class AlterarProdutoHandler : Notifiable, IRequestHandler<AlterarProdutoRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProduto _repositoryProduto;

        public AlterarProdutoHandler(IMediator mediator, IRepositoryProduto repositoryProduto)
        {
            _mediator = mediator;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<Response> Handle(AlterarProdutoRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AlterarProdutoRequestValidator();

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

            Model.Produto Produto = new Model.Produto();

            Produto.AlterarProduto(
                request.Id,
                request.codBarras,
                request.nomeProduto,
                request.descricaoProduto,
                request.precoCusto,
                request.precoVenda,
                request.margemLucro,
                request.dataFabricao,
                request.dataVencimento,
                request.quatidadeEstoqueProduto,
                request.FornecedorId,
                request.CategoriaId,
                request.statusAtivo);

            var retornoExist = _repositoryProduto.Listar().Where(x => x.Id == request.Id);

            if (!retornoExist.Any())
            {
                AddNotification("Produto", "retornoExist");
                return new Response(this);
            }

            Produto = _repositoryProduto.Editar(Produto);

            //Criar meu objeto de resposta
            var response = new Response(this, Produto);

            return await Task.FromResult(response);
        }
    }
}
