using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Colaborador.ListarColaborador
{
    public class ListarColaboradorPorIdHandler : Notifiable, IRequestHandler<ListarColaboradorRequest, Commands.Response>
    {
        private readonly IRepositoryColaborador _repositoryColaborador;
        private readonly IRepositoryDepartamento _repositoryDepartamento;
        public ListarColaboradorPorIdHandler(IRepositoryColaborador repositoryColaborador,
            IRepositoryDepartamento repositoryDepartamento)
        {
            _repositoryColaborador = repositoryColaborador;
            _repositoryDepartamento = repositoryDepartamento;
        }

        public async Task<Commands.Response> Handle(ListarColaboradorRequest request, CancellationToken cancellationToken)
        {
            //Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return new Commands.Response(this);
            }

            var grupoCollection = await _repositoryColaborador.ListarAsync(cancellationToken, x => x.Usuario, x => x.Departamento);

            //Cria objeto de resposta
            var response = new Commands.Response(this, grupoCollection);

            ////Retorna o resultado
            return response;
        }
    }
}
