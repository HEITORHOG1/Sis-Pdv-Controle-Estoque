using MediatR;
using prmToolkit.NotificationPattern;
using Sis_Pdv_Controle_Estoque.Interfaces;
using System.Text.RegularExpressions;

namespace Commands.Categoria.ListarCategoria.ListarClientePorCpfCnpj
{
    public class ListarClientePorCpfCnpjHandler : Notifiable, IRequestHandler<ListarClientePorCpfCnpjRequest, Sis_Pdv_Controle_Estoque.Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryCliente _repositoryCliente;

        public ListarClientePorCpfCnpjHandler(IMediator mediator, IRepositoryCliente repositoryCliente)
        {
            _mediator = mediator;
            _repositoryCliente = repositoryCliente;
        }

        public async Task<Sis_Pdv_Controle_Estoque.Commands.Response> Handle(ListarClientePorCpfCnpjRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Erro", "Handle");
                return new Sis_Pdv_Controle_Estoque.Commands.Response(this);
            }

            var Collection = _repositoryCliente.Listar().Where(x => x.CpfCnpj == request.CpfCnpj); 
            if (!Collection.Any()) 
            {
                AddNotification("ATENÇÃO", "CATEGORIA NÃO ENCONTRADA");
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


