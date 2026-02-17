using Interfaces;
using MediatR;
using prmToolkit.NotificationPattern;

namespace Commands.Colaborador.RemoverColaborador
{
    public class RemoverColaboradorHandler : Notifiable, IRequestHandler<RemoverColaboradorRequest, Commands.Response>
    {
        private readonly IRepositoryColaborador _repositoryColaborador;
        private readonly IRepositoryUsuario _repositoryUsuario;
        public RemoverColaboradorHandler(IRepositoryColaborador repositoryColaborador, IRepositoryUsuario repositoryUsuario)
        {
            _repositoryColaborador = repositoryColaborador;
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<Commands.Response> Handle(RemoverColaboradorRequest request, CancellationToken cancellationToken)
        {
            // Valida se o objeto request esta nulo
            if (request == null)
            {
                AddNotification("Request", "A requisição não pode ser nula.");
                return new Commands.Response(this);
            }

            // Busca colaborador e usuario via repositório (sem EF no Handler)
            Model.Colaborador colaborador = await _repositoryColaborador.ObterPorAsync(
                x => x.Id == request.Id,
                cancellationToken,
                x => x.Usuario);

            if (colaborador == null)
            {
                AddNotification("Request", "Colaborador não encontrado.");
                return new Commands.Response(this);
            }

            // Remove logicamente (Soft Delete)
            // Se Usuario for null (ex: tabela inconsistente), valida antes?
            if (colaborador.Usuario != null)
            {
                _repositoryUsuario.Remover(colaborador.Usuario);
            }
            
            _repositoryColaborador.Remover(colaborador);
            
            var result = new { colaborador.Id };

            // Cria objeto de resposta
            var response = new Commands.Response(this, result);

            // Retorna o resultado
            return response;
        }
    }
}
