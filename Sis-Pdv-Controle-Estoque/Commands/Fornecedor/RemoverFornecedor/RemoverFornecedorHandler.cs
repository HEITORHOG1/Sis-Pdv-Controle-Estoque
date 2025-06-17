using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Fornecedor.RemoverFornecedor

{
    public class RemoverFornecedorHandler : Notifiable, IRequestHandler<RemoverFornecedorResquest, Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryFornecedor _repositoryFornecedor;

        public RemoverFornecedorHandler(IMediator mediator, IRepositoryFornecedor repositoryFornecedor)
        {
            _mediator = mediator;
            _repositoryFornecedor = repositoryFornecedor;
        }

        public async Task<Sis_Pdv_Controle_Estoque.Commands.Response> Handle(RemoverFornecedorResquest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            Sis_Pdv_Controle_Estoque.Model.Fornecedor Fornecedor = _repositoryFornecedor.ObterPorId(request.Id);

            if (Fornecedor == null)
            {
                AddNotification("Request", "");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            _repositoryFornecedor.Remover(Fornecedor);

            var result = new { Id = Fornecedor.Id };

            //Cria objeto de resposta
            var response = new Sis_Pdv_Controle_Estoque.Commands.Response(this, result);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}
