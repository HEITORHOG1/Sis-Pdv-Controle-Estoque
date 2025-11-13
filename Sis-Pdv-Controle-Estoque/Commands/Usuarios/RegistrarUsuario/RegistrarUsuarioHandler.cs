using MediatR;
using prmToolkit.NotificationPattern;
using Interfaces;
using Interfaces.Services;

namespace Commands.Usuarios.RegistrarUsuario
{
    public class RegistrarUsuarioHandler : Notifiable, IRequestHandler<RegistrarUsuarioRequest, Response>
    {
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IRepositoryRole _repositoryRole;
        private readonly IRepositoryUserRole _repositoryUserRole;
        private readonly IPasswordService _passwordService;

        public RegistrarUsuarioHandler(
            IRepositoryUsuario repositoryUsuario,
            IRepositoryRole repositoryRole,
            IRepositoryUserRole repositoryUserRole,
            IPasswordService passwordService)
        {
            _repositoryUsuario = repositoryUsuario;
            _repositoryRole = repositoryRole;
            _repositoryUserRole = repositoryUserRole;
            _passwordService = passwordService;
        }

        public async Task<Response> Handle(RegistrarUsuarioRequest request, CancellationToken cancellationToken)
        {
            // Validar requisição
            var validator = new RegistrarUsuarioRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    AddNotification(error.PropertyName, error.ErrorMessage);
                }
                return new Response(this);
            }

            // Verificar se login já existe
            var existingUserByLogin = await _repositoryUsuario.GetByLoginAsync(request.Login);
            if (existingUserByLogin != null)
            {
                AddNotification("Login", "Login já está em uso");
                return new Response(this);
            }

            // Verificar se email já existe
            var existingUserByEmail = await _repositoryUsuario.GetByEmailAsync(request.Email);
            if (existingUserByEmail != null)
            {
                AddNotification("Email", "Email já está em uso");
                return new Response(this);
            }

            // Verificar se as roles existem
            if (request.RoleIds.Any())
            {
                var existingRoles = await _repositoryRole.ListarAsync();
                var existingRoleIds = existingRoles.Select(r => r.Id).ToList();
                var invalidRoleIds = request.RoleIds.Except(existingRoleIds).ToList();

                if (invalidRoleIds.Any())
                {
                    AddNotification("RoleIds", $"Roles inválidas: {string.Join(", ", invalidRoleIds)}");
                    return new Response(this);
                }
            }

            // Criar hash da senha
            var passwordHash = _passwordService.HashPassword(request.Senha);

            // Criar usuário
            var usuario = new Model.Usuario
            {
                Login = request.Login,
                Email = request.Email,
                Nome = request.Nome,
                Senha = passwordHash,
                StatusAtivo = request.StatusAtivo
            };

            // Salvar usuário
            usuario = await _repositoryUsuario.AdicionarAsync(usuario);

            // Adicionar roles ao usuário
            if (request.RoleIds.Any())
            {
                foreach (var roleId in request.RoleIds)
                {
                    var userRole = new Model.UserRole
                    {
                        UserId = usuario.Id,
                        RoleId = roleId
                    };
                    await _repositoryUserRole.AdicionarAsync(userRole);
                }
            }

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