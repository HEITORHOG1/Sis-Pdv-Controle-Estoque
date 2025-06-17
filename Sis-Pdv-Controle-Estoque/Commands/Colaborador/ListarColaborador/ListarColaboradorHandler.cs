using Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using prmToolkit.NotificationPattern;

namespace Commands.Colaborador.ListarColaborador
{
    public class ListarColaboradorPorIdHandler : Notifiable, IRequestHandler<ListarColaboradorRequest, Commands.Response>
    {
        private readonly IMediator _mediator;
        private readonly IRepositoryColaborador _repositoryColaborador;
        private readonly IRepositoryDepartamento _repositoryDepartamento;
        public ListarColaboradorPorIdHandler(IMediator mediator, IRepositoryColaborador repositoryColaborador,
            IRepositoryDepartamento repositoryDepartamento)
        {
            _mediator = mediator;
            _repositoryColaborador = repositoryColaborador;
            _repositoryDepartamento = repositoryDepartamento;
        }

        public async Task<Commands.Response> Handle(ListarColaboradorRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "");
                return new Commands.Response(this);
            }


            var grupoCollection = _repositoryColaborador.Listar().Include(x => x.Usuario).Include(x => x.Departamento).ToList();


            //Cria objeto de resposta
            var response = new Commands.Response(this, grupoCollection);

            ////Retorna o resultado
            return await Task.FromResult(response);
        }
    }
}

