using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Cupom.RemoverCupom
{
    public class RemoverCupomHandler : Notifiable, IRequestHandler<RemoverCupomRequest, Response>
    {
        private readonly IRepositoryCupom _repositoryCupom;

        public RemoverCupomHandler(IRepositoryCupom repositoryCupom)
        {
            _repositoryCupom = repositoryCupom;
        }

        public Task<Response> Handle(RemoverCupomRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                AddNotification("Request", "Request não pode ser nulo.");
                return Task.FromResult(new Response(this));
            }

            var cupom = _repositoryCupom.ObterPorId(request.Id);
            if (cupom == null)
            {
                AddNotification("Cupom", "Cupom não encontrado.");
                return Task.FromResult(new Response(this));
            }

            if (cupom.IsDeleted)
            {
                AddNotification("Cupom", "Cupom já foi removido.");
                return Task.FromResult(new Response(this));
            }

            _repositoryCupom.Remover(cupom);

            return Task.FromResult(new Response(this, cupom));
        }
    }
}
