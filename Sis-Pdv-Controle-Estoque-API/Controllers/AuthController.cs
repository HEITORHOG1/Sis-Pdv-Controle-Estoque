using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sis_Pdv_Controle_Estoque_API.Models;
using Sis_Pdv_Controle_Estoque_API.Models.Auth;
using Sis_Pdv_Controle_Estoque_API.Services.Auth;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Realiza login no sistema
        /// </summary>
        /// <param name="request">Dados de login</param>
        /// <returns>Token de acesso e informações do usuário</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AuthResult>>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<AuthResult>.Error("Dados inválidos", errors));
            }

            var result = await _authenticationService.AuthenticateAsync(request);

            if (!result.Success)
            {
                return Unauthorized(ApiResponse<AuthResult>.Error(result.Message));
            }

            return Ok(ApiResponse<AuthResult>.Ok(result, "Login realizado com sucesso"));
        }

        /// <summary>
        /// Renova o token de acesso usando o refresh token
        /// </summary>
        /// <param name="request">Refresh token</param>
        /// <returns>Novo token de acesso</returns>
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AuthResult>>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<AuthResult>.Error("Dados inválidos", errors));
            }

            var result = await _authenticationService.RefreshTokenAsync(request.RefreshToken);

            if (!result.Success)
            {
                return Unauthorized(ApiResponse<AuthResult>.Error(result.Message));
            }

            return Ok(ApiResponse<AuthResult>.Ok(result, "Token renovado com sucesso"));
        }

        /// <summary>
        /// Revoga o refresh token (logout)
        /// </summary>
        /// <param name="request">Refresh token para revogar</param>
        /// <returns>Confirmação de logout</returns>
        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<bool>>> Logout([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<bool>.Error("Dados inválidos", errors));
            }

            var success = await _authenticationService.RevokeTokenAsync(request.RefreshToken);

            if (!success)
            {
                return BadRequest(ApiResponse<bool>.Error("Erro ao realizar logout"));
            }

            return Ok(ApiResponse<bool>.Ok(true, "Logout realizado com sucesso"));
        }

        /// <summary>
        /// Obtém informações do usuário autenticado
        /// </summary>
        /// <returns>Informações do usuário</returns>
        [HttpGet("me")]
        [Authorize]
        public ActionResult<ApiResponse<object>> GetCurrentUser()
        {
            var userInfo = new
            {
                Id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                Login = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value,
                Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
                Nome = User.FindFirst("nome")?.Value,
                Roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value),
                Permissions = User.FindAll("permission").Select(c => c.Value)
            };

            return Ok(ApiResponse<object>.Ok(userInfo, "Informações do usuário obtidas com sucesso"));
        }
    }
}