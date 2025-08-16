using Commands.Usuarios.RegistrarUsuario;
using Commands.Usuarios.AlterarSenha;
using Commands.Usuarios.AtualizarPerfil;
using Commands.Usuarios.ListarUsuarios;
using Commands.Usuarios.ListarSessoes;
using Commands.Usuarios.RevogarSessao;
using Commands.Usuarios.AtribuirRoles;
using Commands.Usuarios.AlterarStatusUsuario;
using Commands.Usuarios.ResetarSenha;
using Commands.Usuarios.Login;
using Commands.Roles.CriarRole;
using Commands.Roles.ListarRoles;
using Commands.Roles.AtualizarRole;
using Commands.Roles.RemoverRole;
using Commands.Permissions.ListarPermissions;
using Sis_Pdv_Controle_Estoque_API.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sis_Pdv_Controle_Estoque_API.Models.Auth;
using Sis_Pdv_Controle_Estoque_API.Services.Auth;
using Repositories.Transactions;
using System.Security.Claims;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserManagementController : Sis_Pdv_Controle_Estoque_API.Controllers.Base.ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthenticationService _authenticationService;

        public UserManagementController(IMediator mediator, IAuthenticationService authenticationService, IUnitOfWork unitOfWork) 
            : base(unitOfWork)
        {
            _mediator = mediator;
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Registra um novo usuário no sistema
        /// </summary>
        [HttpPost("register")]
        [Authorize(Policy = "RequireUserManagementPermission")]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrarUsuarioRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Realiza login do usuário
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Sis_Pdv_Controle_Estoque_API.Models.Auth.LoginRequest request)
        {
            var response = await _authenticationService.AuthenticateAsync(request);
            
            if (response.Success)
            {
                return Ok(response);
            }
            
            return BadRequest(response);
        }

        /// <summary>
        /// Altera a senha do usuário
        /// </summary>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] AlterarSenhaRequest request)
        {
            // Garantir que o usuário só pode alterar sua própria senha ou ter permissão de administrador
            var currentUserId = GetCurrentUserId();
            if (request.UsuarioId != currentUserId && !User.HasClaim("permission", "user.manage"))
            {
                return Forbid("Você só pode alterar sua própria senha");
            }

            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Atualiza o perfil do usuário
        /// </summary>
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] AtualizarPerfilRequest request)
        {
            // Garantir que o usuário só pode alterar seu próprio perfil ou ter permissão de administrador
            var currentUserId = GetCurrentUserId();
            if (request.UsuarioId != currentUserId && !User.HasClaim("permission", "user.manage"))
            {
                return Forbid("Você só pode alterar seu próprio perfil");
            }

            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Lista usuários do sistema
        /// </summary>
        [HttpGet("users")]
        [Authorize(Policy = "RequireUserManagementPermission")]
        public async Task<IActionResult> ListUsers([FromQuery] ListarUsuariosRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Lista sessões ativas do usuário
        /// </summary>
        [HttpGet("sessions/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> ListUserSessions(Guid userId)
        {
            // Garantir que o usuário só pode ver suas próprias sessões ou ter permissão de administrador
            var currentUserId = GetCurrentUserId();
            if (userId != currentUserId && !User.HasClaim("permission", "user.manage"))
            {
                return Forbid("Você só pode ver suas próprias sessões");
            }

            var request = new ListarSessoesRequest { UsuarioId = userId };
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Revoga uma sessão específica
        /// </summary>
        [HttpPost("sessions/{sessionId:guid}/revoke")]
        [Authorize]
        public async Task<IActionResult> RevokeSession(Guid sessionId, [FromBody] RevogarSessaoRequest request)
        {
            request.SessionId = sessionId;

            // Garantir que o usuário só pode revogar suas próprias sessões ou ter permissão de administrador
            var currentUserId = GetCurrentUserId();
            if (request.UsuarioId != currentUserId && !User.HasClaim("permission", "user.manage"))
            {
                return Forbid("Você só pode revogar suas próprias sessões");
            }

            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Atribui roles a um usuário
        /// </summary>
        [HttpPost("users/{userId:guid}/roles")]
        [Authorize(Policy = "RequireUserManagementPermission")]
        public async Task<IActionResult> AssignRoles(Guid userId, [FromBody] AtribuirRolesRequest request)
        {
            request.UsuarioId = userId;
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Cria uma nova role
        /// </summary>
        [HttpPost("roles")]
        [Authorize(Policy = "RequireRoleManagementPermission")]
        public async Task<IActionResult> CreateRole([FromBody] CriarRoleRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Lista roles do sistema
        /// </summary>
        [HttpGet("roles")]
        [Authorize(Policy = "RequireRoleManagementPermission")]
        public async Task<IActionResult> ListRoles([FromQuery] ListarRolesRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Atualiza uma role existente
        /// </summary>
        [HttpPut("roles/{roleId:guid}")]
        [Authorize(Policy = "RequireRoleManagementPermission")]
        public async Task<IActionResult> UpdateRole(Guid roleId, [FromBody] AtualizarRoleRequest request)
        {
            request.Id = roleId;
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Remove uma role do sistema
        /// </summary>
        [HttpDelete("roles/{roleId:guid}")]
        [Authorize(Policy = "RequireRoleManagementPermission")]
        public async Task<IActionResult> DeleteRole(Guid roleId)
        {
            var request = new RemoverRoleRequest { Id = roleId };
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Lista permissões do sistema
        /// </summary>
        [HttpGet("permissions")]
        [Authorize(Policy = "RequireRoleManagementPermission")]
        public async Task<IActionResult> ListPermissions([FromQuery] ListarPermissionsRequest request)
        {
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Altera o status de um usuário (ativo/inativo)
        /// </summary>
        [HttpPatch("users/{userId:guid}/status")]
        [Authorize(Policy = "RequireUserManagementPermission")]
        public async Task<IActionResult> ChangeUserStatus(Guid userId, [FromBody] AlterarStatusUsuarioRequest request)
        {
            request.UsuarioId = userId;
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Reseta a senha de um usuário
        /// </summary>
        [HttpPost("users/{userId:guid}/reset-password")]
        [Authorize(Policy = "RequireUserManagementPermission")]
        public async Task<IActionResult> ResetUserPassword(Guid userId, [FromBody] ResetarSenhaRequest request)
        {
            request.UsuarioId = userId;
            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        /// <summary>
        /// Login alternativo via MediatR (para consistência com CQRS)
        /// </summary>
        [HttpPost("login-cqrs")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginCqrs([FromBody] Commands.Usuarios.Login.LoginRequest request)
        {
            // Adicionar informações de contexto da requisição
            request.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            request.UserAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var response = await _mediator.Send(request);
            return await ResponseAsync(response);
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }
    }
}