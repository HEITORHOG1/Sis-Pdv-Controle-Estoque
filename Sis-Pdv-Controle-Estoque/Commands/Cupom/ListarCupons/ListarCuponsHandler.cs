using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Cupom.ListarCupons
{
    public class ListarCuponsHandler : Notifiable, IRequestHandler<ListarCuponsRequest, Response>
    {
        private readonly IRepositoryCupom _repositoryCupom;

        public ListarCuponsHandler(IRepositoryCupom repositoryCupom)
        {
            _repositoryCupom = repositoryCupom;
        }

        public Task<Response> Handle(ListarCuponsRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Response(this));
            }

            var cupons = _repositoryCupom.Listar().ToList();

            return Task.FromResult(new Response(this, cupons));
        }
    }
}
