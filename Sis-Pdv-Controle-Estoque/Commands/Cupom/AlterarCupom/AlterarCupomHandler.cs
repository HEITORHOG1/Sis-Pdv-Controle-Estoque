using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Cupom.AlterarCupom
{
    public class AlterarCupomHandler : Notifiable, IRequestHandler<AlterarCupomRequest, Response>
    {
        private readonly IRepositoryCupom _repositoryCupom;

        public AlterarCupomHandler(IRepositoryCupom repositoryCupom)
        {
            _repositoryCupom = repositoryCupom;
        }

        public async Task<Response> Handle(AlterarCupomRequest request, CancellationToken cancellationToken)
        {
            var cupom = await _repositoryCupom.ObterPorIdAsync(request.Id, cancellationToken);
            if (cupom == null)
            {
                AddNotification("Cupom", "Cupom não encontrado.");
                return new Response(this);
            }

            cupom.AlterarCupom(
                request.PedidoId,
                request.DataEmissao,
                request.NumeroSerie,
                request.ChaveAcesso,
                request.ValorTotal,
                request.DocumentoCliente);

            await _repositoryCupom.EditarAsync(cupom, cancellationToken);

            return new Response(this, cupom);
        }
    }
}
