using Microsoft.AspNetCore.Mvc;
using Sis_Pdv_Controle_Estoque_API.Models;
using System.Reflection;

namespace Sis_Pdv_Controle_Estoque_API.Controllers
{
    /// <summary>
    /// Health check controller for monitoring system status
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Basic health check endpoint
        /// </summary>
        /// <returns>System health status</returns>
        [HttpGet]
        public IActionResult GetHealth()
        {
            var healthData = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                MachineName = Environment.MachineName,
                ProcessId = Environment.ProcessId,
                WorkingSet = GC.GetTotalMemory(false),
                CorrelationId = HttpContext.TraceIdentifier
            };

            _logger.LogInformation("Health check requested. Status: Healthy, CorrelationId: {CorrelationId}", 
                HttpContext.TraceIdentifier);

            return Ok(ApiResponse<object>.Ok(healthData, "System is healthy", HttpContext.TraceIdentifier));
        }

        /// <summary>
        /// Detailed health check with system information
        /// </summary>
        /// <returns>Detailed system health information</returns>
        [HttpGet("detailed")]
        public IActionResult GetDetailedHealth()
        {
            var detailedHealthData = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Application = new
                {
                    Name = "PDV Control System API",
                    Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                    Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                    StartTime = DateTime.UtcNow.AddMilliseconds(-Environment.TickCount64)
                },
                System = new
                {
                    MachineName = Environment.MachineName,
                    ProcessId = Environment.ProcessId,
                    WorkingSet = GC.GetTotalMemory(false),
                    ProcessorCount = Environment.ProcessorCount,
                    OSVersion = Environment.OSVersion.ToString(),
                    Is64BitProcess = Environment.Is64BitProcess
                },
                Runtime = new
                {
                    Version = Environment.Version.ToString(),
                    Framework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription
                }
            };

            _logger.LogInformation("Detailed health check requested. CorrelationId: {CorrelationId}", 
                HttpContext.TraceIdentifier);

            return Ok(ApiResponse<object>.Ok(detailedHealthData, "Detailed system health information", HttpContext.TraceIdentifier));
        }
    }
}