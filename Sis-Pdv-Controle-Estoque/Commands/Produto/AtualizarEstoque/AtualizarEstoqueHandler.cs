using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Produto.AtualizarEstoque
{
    public class AtualizarEstoqueHandler : Notifiable, IRequestHandler<AtualizarEstoqueRequest, Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryProduto _repositoryProduto;

        public AtualizarEstoqueHandler(IMediator mediator, IRepositoryProduto repositoryProduto)
        {
            _mediator = mediator;
            _repositoryProduto = repositoryProduto;
        }

        public async Task<Response> Handle(AtualizarEstoqueRequest request, CancellationToken cancellationToken)
        {
            // Instancia o validador
            var validator = new AtualizarEstoqueRequestValidator();

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

            Sis_Pdv_Controle_Estoque.Model.Produto Produto = new Sis_Pdv_Controle_Estoque.Model.Produto();

            var retornoExist = _repositoryProduto.Listar().Where(x => x.Id == request.Id).FirstOrDefault();

            if (retornoExist == null)
            {
                AddNotification("Produto", "");
                return new Response(this);
            }
            if (retornoExist.quatidadeEstoqueProduto <= 0)
            {
                AddNotification("Produto", "Produto Sem Estoque");
                return new Response(this);
            }
            if (request.quatidadeEstoqueProduto > retornoExist.quatidadeEstoqueProduto)
            {
                AddNotification("Produto", "Quantidade em estoque " + retornoExist.quatidadeEstoqueProduto);
                return new Response(this);
            }

            retornoExist.quatidadeEstoqueProduto = retornoExist.quatidadeEstoqueProduto - request.quatidadeEstoqueProduto;
            Produto = _repositoryProduto.Editar(retornoExist);

            //Criar meu objeto de resposta
            var response = new Response(this, Produto);

            return await Task.FromResult(response);
        }
    }
}
