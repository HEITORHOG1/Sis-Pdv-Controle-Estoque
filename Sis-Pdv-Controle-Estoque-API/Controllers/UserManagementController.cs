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
using Asp.Versioning;
using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    /// <summary>
    /// User and role management endpoints for the PDV Control System
    /// </summary>
    /// <remarks>
    /// This controller provides comprehensive user management functionality including:
    /// 
    /// **User Operations:**
    /// - User registration and profile management
    /// - Password management and reset functionality
    /// - User status management (active/inactive)
    /// - Session management and monitoring
    /// 
    /// **Role and Permission Management:**
    /// - Role creation and management
    /// - Permission assignment and revocation
    /// - Role-based access control (RBAC)
    /// 
    /// **Security Features:**
    /// - Secure password hashing with bcrypt
    /// - Session tracking and management
    /// - Audit logging for all user operations
    /// - Permission-based access control
    /// 
    /// **Required Permissions:**
    /// - Most endpoints require `user.manage` permission
    /// - Role management requires `role.manage` permission
    /// - Users can manage their own profile without special permissions
    /// </remarks>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    [Produces("application/json")]
    [Tags("User Management")]
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
        /// Register a new user in the system
        /// </summary>
        /// <param name="request">User registration data including login, email, password, and profile information</param>
        /// <returns>Created user information with assigned ID and default role</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// Creates a new user account with the provided information. The user will be created with:
        /// - Secure password hashing using bcrypt
        /// - Default role assignment (usually "User" role)
        /// - Email validation and uniqueness check
        /// - Audit trail entry for user creation
        /// 
        /// **Example Request:**
        /// ```json
        /// {
        ///   "login": "newuser",
        ///   "email": "newuser@company.com",
        ///   "password": "SecurePassword123!",
        ///   "nome": "New User",
        ///   "telefone": "(11) 99999-9999",
        ///   "isActive": true
        /// }
        /// ```
        /// 
        /// **Password Requirements:**
        /// - Minimum 8 characters
        /// - At least one uppercase letter
        /// - At least one lowercase letter
        /// - At least one number
        /// - At least one special character
        /// 
        /// **Validation Rules:**
        /// - Login must be unique and 3-50 characters
        /// - Email must be valid format and unique
        /// - Phone number must follow Brazilian format
        /// - Name is required and 2-100 characters
        /// </remarks>
        /// <response code="201">User created successfully</response>
        /// <response code="400">Invalid request data or validation errors</response>
        /// <response code="401">Unauthorized - invalid or missing token</response>
        /// <response code="403">Forbidden - insufficient permissions</response>
        /// <response code="409">Conflict - login or email already exists</response>
        [HttpPost("register")]
        [Authorize(Policy = "RequireUserManagementPermission")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RegisterUser([FromBody, Required] RegistrarUsuarioRequest request)
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