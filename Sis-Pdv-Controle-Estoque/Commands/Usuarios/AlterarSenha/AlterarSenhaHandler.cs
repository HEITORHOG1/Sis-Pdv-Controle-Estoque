using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;
using Interfaces.Services;

namespace Commands.Usuarios.AlterarSenha
{
    public class AlterarSenhaHandler : Notifiable, IRequestHandler<AlterarSenhaRequest, Response>
    {
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IPasswordService _passwordService;

        public AlterarSenhaHandler(
            IRepositoryUsuario repositoryUsuario,
            IPasswordService passwordService)
        {
            _repositoryUsuario = repositoryUsuario;
            _passwordService = passwordService;
        }

        public async Task<Response> Handle(AlterarSenhaRequest request, CancellationToken cancellationToken)
        {
            // Validar requisição
            var validator = new AlterarSenhaRequestValidator();
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

            // Verificar se usuário está ativo
            if (!usuario.StatusAtivo)
            {
                AddNotification("Usuario", "Usuário inativo");
                return new Response(this);
            }

            // Verificar senha atual
            if (!_passwordService.VerifyPassword(request.SenhaAtual, usuario.Senha))
            {
                AddNotification("SenhaAtual", "Senha atual incorreta");
                return new Response(this);
            }

            // Verificar se a nova senha é diferente da atual
            if (_passwordService.VerifyPassword(request.NovaSenha, usuario.Senha))
            {
                AddNotification("NovaSenha", "A nova senha deve ser diferente da senha atual");
                return new Response(this);
            }

            // Atualizar senha
            usuario.Senha = _passwordService.HashPassword(request.NovaSenha);
            
            // Invalidar refresh tokens existentes por segurança
            usuario.RefreshToken = null;
            usuario.RefreshTokenExpiryTime = null;

            await _repositoryUsuario.EditarAsync(usuario);

            return new Response(this, new { Message = "Senha alterada com sucesso" });
        }
    }
}