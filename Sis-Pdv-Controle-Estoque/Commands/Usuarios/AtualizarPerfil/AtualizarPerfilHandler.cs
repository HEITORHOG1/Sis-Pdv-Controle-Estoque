using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;

namespace Commands.Usuarios.AtualizarPerfil
{
    public class AtualizarPerfilHandler : Notifiable, IRequestHandler<AtualizarPerfilRequest, Response>
    {
        private readonly IRepositoryUsuario _repositoryUsuario;

        public AtualizarPerfilHandler(IRepositoryUsuario repositoryUsuario)
        {
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<Response> Handle(AtualizarPerfilRequest request, CancellationToken cancellationToken)
        {
            // Validar requisição
            var validator = new AtualizarPerfilRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    AddNotification(error.PropertyName, error.ErrorMessage);
                }
                return new Response(this);
            }

            // Buscar usuário
            var usuario = await _repositoryUsuario.ObterPorIdAsync(request.UsuarioId);
            if (usuario == null)
            {
                AddNotification("Usuario", "Usuário não encontrado");
                return new Response(this);
            }

            // Verificar se email já está em uso por outro usuário
            var existingUserByEmail = await _repositoryUsuario.GetByEmailAsync(request.Email);
            if (existingUserByEmail != null && existingUserByEmail.Id != request.UsuarioId)
            {
                AddNotification("Email", "Email já está em uso por outro usuário");
                return new Response(this);
            }

            // Atualizar dados do usuário
            usuario.Nome = request.Nome;
            usuario.Email = request.Email;

            await _repositoryUsuario.EditarAsync(usuario);

            var result = new
            {
                Id = usuario.Id,
                Login = usuario.Login,
                Email = usuario.Email,
                Nome = usuario.Nome,
                StatusAtivo = usuario.StatusAtivo
            };

            return new Response(this, result);
        }
    }
}