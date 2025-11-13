using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sis_Pdv_Controle_Estoque_API.Models;
using Sis_Pdv_Controle_Estoque_API.Services.Auth;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly AuthSeederService _seederService;
        private readonly IWebHostEnvironment _environment;

        public SeedController(AuthSeederService seederService, IWebHostEnvironment environment)
        {
            _seederService = seederService;
            _environment = environment;
        }

        /// <summary>
        /// Seeds the database with initial authentication data (Development only)
        /// </summary>
        /// <returns>Success message</returns>
        [HttpPost("auth")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> SeedAuthData()
        {
            if (!_environment.IsDevelopment())
            {
                return Forbid("This endpoint is only available in development environment");
            }

            try
            {
                await _seederService.SeedAsync();
                return Ok(ApiResponse<string>.Ok("Default admin user created with login: admin, password: admin123", 
                    "Authentication data seeded successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Error($"Error seeding data: {ex.Message}"));
            }
        }
    }
}