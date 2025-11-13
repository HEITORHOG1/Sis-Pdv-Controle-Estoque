using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sis_Pdv_Controle_Estoque_API.Controllers.Base;
using Sis_Pdv_Controle_Estoque_API.Models.Health;
using Sis_Pdv_Controle_Estoque_API.Services.Health;
using Asp.Versioning;
using Repositories.Transactions;
using HealthCheckResult = Sis_Pdv_Controle_Estoque_API.Models.Health.HealthCheckResult;
using System.Net;

namespace Sis_Pdv_Controle_Estoque_API.Controllers;

/// <summary>
/// Controller for health monitoring and system status endpoints
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class HealthCheckController : Microsoft.AspNetCore.Mvc.ControllerBase
{
    private readonly HealthCheckService _healthCheckService;
    private readonly IMetricsCollectionService _metricsService;

    public HealthCheckController(
        HealthCheckService healthCheckService,
        IMetricsCollectionService metricsService)
    {
        _healthCheckService = healthCheckService;
        _metricsService = metricsService;
    }

    /// <summary>
    /// Get comprehensive health status of all system components
    /// </summary>
    /// <returns>Detailed health check results</returns>
    [HttpGet("status")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(HealthCheckResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(HealthCheckResponse), (int)HttpStatusCode.ServiceUnavailable)]
    public async Task<IActionResult> GetHealthStatus(CancellationToken cancellationToken = default)
    {
        try
        {
            var healthReport = await _healthCheckService.CheckHealthAsync(cancellationToken);
            
            var response = new HealthCheckResponse
            {
                Status = healthReport.Status.ToString(),
                TotalDuration = healthReport.TotalDuration,
                Results = healthReport.Entries.ToDictionary(
                    kvp => kvp.Key,
                    kvp => new HealthCheckResult
                    {
                        Status = kvp.Value.Status.ToString(),
                        Description = kvp.Value.Description,
                        Duration = kvp.Value.Duration,
                        Data = kvp.Value.Data?.ToDictionary(d => d.Key, d => d.Value),
                        Exception = kvp.Value.Exception?.Message
                    })
            };

            var statusCode = healthReport.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy 
                ? HttpStatusCode.OK 
                : HttpStatusCode.ServiceUnavailable;

            return StatusCode((int)statusCode, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to get health status", message = ex.Message });
        }
    }

    /// <summary>
    /// Get system performance metrics
    /// </summary>
    /// <returns>System metrics including CPU, memory, and disk usage</returns>
    [HttpGet("metrics/system")]
    [Authorize]
    [ProducesResponseType(typeof(SystemMetrics), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetSystemMetrics(CancellationToken cancellationToken = default)
    {
        try
        {
            var metrics = await _metricsService.GetSystemMetricsAsync(cancellationToken);
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to get system metrics", message = ex.Message });
        }
    }

    /// <summary>
    /// Get business-specific metrics
    /// </summary>
    /// <returns>Business metrics including sales, inventory, and user data</returns>
    [HttpGet("metrics/business")]
    [Authorize]
    [ProducesResponseType(typeof(BusinessMetrics), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBusinessMetrics(CancellationToken cancellationToken = default)
    {
        try
        {
            var metrics = await _metricsService.GetBusinessMetricsAsync(cancellationToken);
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to get business metrics", message = ex.Message });
        }
    }

    /// <summary>
    /// Get application performance metrics
    /// </summary>
    /// <returns>Application metrics including request counts and response times</returns>
    [HttpGet("metrics/application")]
    [Authorize]
    [ProducesResponseType(typeof(ApplicationMetrics), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetApplicationMetrics(CancellationToken cancellationToken = default)
    {
        try
        {
            var metrics = await _metricsService.GetApplicationMetricsAsync(cancellationToken);
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to get application metrics", message = ex.Message });
        }
    }

    /// <summary>
    /// Get all metrics in a single response for dashboard
    /// </summary>
    /// <returns>Combined metrics for monitoring dashboard</returns>
    [HttpGet("dashboard")]
    [Authorize]
    [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetDashboardMetrics(CancellationToken cancellationToken = default)
    {
        try
        {
            var healthReport = await _healthCheckService.CheckHealthAsync(cancellationToken);
            var systemMetrics = await _metricsService.GetSystemMetricsAsync(cancellationToken);
            var businessMetrics = await _metricsService.GetBusinessMetricsAsync(cancellationToken);
            var applicationMetrics = await _metricsService.GetApplicationMetricsAsync(cancellationToken);

            var dashboardData = new
            {
                Health = new
                {
                    Status = healthReport.Status.ToString(),
                    TotalDuration = healthReport.TotalDuration,
                    ComponentCount = healthReport.Entries.Count,
                    HealthyComponents = healthReport.Entries.Count(e => e.Value.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy),
                    DegradedComponents = healthReport.Entries.Count(e => e.Value.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded),
                    UnhealthyComponents = healthReport.Entries.Count(e => e.Value.Status == Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy)
                },
                System = systemMetrics,
                Business = businessMetrics,
                Application = applicationMetrics,
                Timestamp = DateTime.UtcNow
            };

            return Ok(dashboardData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to get dashboard metrics", message = ex.Message });
        }
    }

    /// <summary>
    /// Get health check results for specific components
    /// </summary>
    /// <param name="component">Component name to check (database, rabbitmq, business-operations, system-metrics)</param>
    /// <returns>Health status of the specified component</returns>
    [HttpGet("component/{component}")]
    [Authorize]
    [ProducesResponseType(typeof(HealthCheckResult), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetComponentHealth(string component, CancellationToken cancellationToken = default)
    {
        try
        {
            var healthReport = await _healthCheckService.CheckHealthAsync(cancellationToken);
            
            if (!healthReport.Entries.TryGetValue(component, out var entry))
            {
                return NotFound(new { error = "Component not found", message = $"Health check component '{component}' not found" });
            }

            var result = new HealthCheckResult
            {
                Status = entry.Status.ToString(),
                Description = entry.Description,
                Duration = entry.Duration,
                Data = entry.Data?.ToDictionary(d => d.Key, d => d.Value),
                Exception = entry.Exception?.Message
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to get component health", message = ex.Message });
        }
    }

    /// <summary>
    /// Get list of available health check components
    /// </summary>
    /// <returns>List of available health check components</returns>
    [HttpGet("components")]
    [Authorize]
    [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAvailableComponents(CancellationToken cancellationToken = default)
    {
        try
        {
            var healthReport = await _healthCheckService.CheckHealthAsync(cancellationToken);
            var components = healthReport.Entries.Keys.ToArray();
            
            return Ok(components);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to get available components", message = ex.Message });
        }
    }
}