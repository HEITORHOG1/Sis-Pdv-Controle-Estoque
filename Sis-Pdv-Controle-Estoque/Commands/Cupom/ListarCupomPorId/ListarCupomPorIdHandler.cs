using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Cupom.ListarCupomPorId
{
    public class ListarCupomPorIdHandler : Notifiable, IRequestHandler<ListarCupomPorIdRequest, Response>
    {
        private readonly IRepositoryCupom _repositoryCupom;

        public ListarCupomPorIdHandler(IRepositoryCupom repositoryCupom)
        {
            _repositoryCupom = repositoryCupom;
        }

        public Task<Response> Handle(ListarCupomPorIdRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return Task.FromResult(new Response(this));
            }

            var cupom = _repositoryCupom.ObterPorId(request.Id);
            if (cupom == null)
            {
                AddNotification("Cupom", "Cupom não encontrado.");
                return Task.FromResult(new Response(this));
            }

            return Task.FromResult(new Response(this, cupom));
        }
    }
}
