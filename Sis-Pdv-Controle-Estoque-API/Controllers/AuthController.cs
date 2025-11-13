using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sis_Pdv_Controle_Estoque_API.Models;
using Sis_Pdv_Controle_Estoque_API.Models.Auth;
using Sis_Pdv_Controle_Estoque_API.Services.Auth;
using Asp.Versioning;
using System.ComponentModel.DataAnnotations;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    /// <summary>
    /// Authentication and authorization endpoints for the PDV Control System
    /// </summary>
    /// <remarks>
    /// This controller handles user authentication, token management, and user session information.
    /// All endpoints except /me are publicly accessible and don't require authentication.
    /// 
    /// **Authentication Flow:**
    /// 1. Use `/login` to authenticate with username/email and password
    /// 2. Receive access token (JWT) and refresh token
    /// 3. Include access token in Authorization header for protected endpoints
    /// 4. Use `/refresh` to get new access token when current one expires
    /// 5. Use `/logout` to invalidate refresh token and end session
    /// 
    /// **Token Security:**
    /// - Access tokens expire in 60 minutes (configurable)
    /// - Refresh tokens expire in 7 days (configurable)
    /// - All tokens are signed with HMAC-SHA256
    /// - Refresh tokens are stored securely and can be revoked
    /// </remarks>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    [Produces("application/json")]
    [Tags("Authentication")]
    public class AuthController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Authenticate user and obtain access tokens
        /// </summary>
        /// <param name="request">Login credentials containing username/email and password</param>
        /// <returns>Authentication result with access token, refresh token, and user information</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// This endpoint authenticates a user using their username or email address and password.
        /// Upon successful authentication, it returns:
        /// - Access token (JWT) for API authorization
        /// - Refresh token for obtaining new access tokens
        /// - User information including roles and permissions
        /// 
        /// **Example Request:**
        /// ```json
        /// {
        ///   "login": "admin@pdvsystem.com",
        ///   "password": "SecurePassword123!"
        /// }
        /// ```
        /// 
        /// **Example Response:**
        /// ```json
        /// {
        ///   "success": true,
        ///   "message": "Login realizado com sucesso",
        ///   "data": {
        ///     "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        ///     "refreshToken": "def502004a8c...",
        ///     "expiresIn": 3600,
        ///     "user": {
        ///       "id": "123e4567-e89b-12d3-a456-426614174000",
        ///       "login": "admin",
        ///       "email": "admin@pdvsystem.com",
        ///       "nome": "Administrator",
        ///       "roles": ["Admin"],
        ///       "permissions": ["user.manage", "inventory.manage"]
        ///     }
        ///   }
        /// }
        /// ```
        /// 
        /// **Security Notes:**
        /// - Passwords are validated against stored bcrypt hashes
        /// - Failed login attempts are logged for security monitoring
        /// - Account lockout may occur after multiple failed attempts
        /// - Use HTTPS in production to protect credentials in transit
        /// </remarks>
        /// <response code="200">Login successful - returns authentication tokens and user info</response>
        /// <response code="400">Invalid request data or validation errors</response>
        /// <response code="401">Invalid credentials or account locked</response>
        /// <response code="500">Internal server error during authentication</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<AuthResult>>> Login([FromBody, Required] LoginRequest request)
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
        /// Refresh access token using refresh token
        /// </summary>
        /// <param name="request">Refresh token request containing the refresh token</param>
        /// <returns>New access token and updated refresh token</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// When an access token expires, use this endpoint to obtain a new one without requiring
        /// the user to log in again. The refresh token must be valid and not expired.
        /// 
        /// **Example Request:**
        /// ```json
        /// {
        ///   "refreshToken": "def502004a8c..."
        /// }
        /// ```
        /// 
        /// **Security Notes:**
        /// - Refresh tokens are single-use and automatically rotated
        /// - Old refresh token is invalidated when new one is issued
        /// - Refresh tokens expire after 7 days of inactivity
        /// - Invalid or expired refresh tokens require re-authentication
        /// </remarks>
        /// <response code="200">Token refreshed successfully</response>
        /// <response code="400">Invalid request data</response>
        /// <response code="401">Invalid or expired refresh token</response>
        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<AuthResult>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<AuthResult>>> RefreshToken([FromBody, Required] RefreshTokenRequest request)
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
        /// Logout user and revoke refresh token
        /// </summary>
        /// <param name="request">Refresh token to revoke</param>
        /// <returns>Logout confirmation</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// This endpoint invalidates the user's refresh token, effectively logging them out
        /// from the system. The access token will continue to work until it expires naturally,
        /// but no new tokens can be obtained.
        /// 
        /// **Example Request:**
        /// ```json
        /// {
        ///   "refreshToken": "def502004a8c..."
        /// }
        /// ```
        /// 
        /// **Security Notes:**
        /// - Always call this endpoint when user explicitly logs out
        /// - Prevents refresh token from being used if compromised
        /// - Client should clear stored tokens after successful logout
        /// - Access tokens remain valid until natural expiration
        /// </remarks>
        /// <response code="200">Logout successful</response>
        /// <response code="400">Invalid request data or logout failed</response>
        [HttpPost("logout")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<bool>>> Logout([FromBody, Required] RefreshTokenRequest request)
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
        /// Get current authenticated user information
        /// </summary>
        /// <returns>Current user's profile information, roles, and permissions</returns>
        /// <remarks>
        /// **Usage:**
        /// 
        /// This endpoint returns detailed information about the currently authenticated user
        /// based on the provided JWT token. Useful for displaying user profile information
        /// and determining available features based on roles and permissions.
        /// 
        /// **Authentication Required:**
        /// Include the access token in the Authorization header:
        /// ```
        /// Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
        /// ```
        /// 
        /// **Example Response:**
        /// ```json
        /// {
        ///   "success": true,
        ///   "message": "Informações do usuário obtidas com sucesso",
        ///   "data": {
        ///     "id": "123e4567-e89b-12d3-a456-426614174000",
        ///     "login": "admin",
        ///     "email": "admin@pdvsystem.com",
        ///     "nome": "Administrator",
        ///     "roles": ["Admin", "Manager"],
        ///     "permissions": [
        ///       "user.manage",
        ///       "inventory.manage",
        ///       "reports.view",
        ///       "backup.create"
        ///     ]
        ///   }
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">User information retrieved successfully</response>
        /// <response code="401">Unauthorized - invalid or missing token</response>
        /// <response code="403">Forbidden - token valid but insufficient permissions</response>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
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