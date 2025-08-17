using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Repositories.Base;
using Sis_Pdv_Controle_Estoque_API.Services.Health;

namespace Sis_Pdv_Controle_Estoque_API.Configuration;

public static class HealthCheckConfiguration
{
    public static IServiceCollection AddHealthCheckServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var rabbitMqConnection = configuration.GetConnectionString("RabbitMQ") ?? "amqp://guest:guest@localhost:5672/";

        services.AddHealthChecks()
            // Database health check
            .AddDbContextCheck<PdvContext>("database", 
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "database", "sql", "ready" })
            
            // MySQL specific health check with proper connection string
            // Use default health query (SELECT 1) and named arguments to avoid overload confusion
            .AddMySql(connectionString: connectionString!, 
                failureStatus: HealthStatus.Unhealthy,
                tags: new[] { "database", "mysql", "ready" },
                timeout: TimeSpan.FromSeconds(10))
            
            // RabbitMQ health check (only if connection string is valid)
            .AddCheck("rabbitmq", () =>
            {
                try
                {
                    if (string.IsNullOrEmpty(rabbitMqConnection) || !Uri.TryCreate(rabbitMqConnection, UriKind.Absolute, out _))
                    {
                        return HealthCheckResult.Degraded("RabbitMQ connection string not configured or invalid");
                    }
                    
                    // In development, just check if the connection string is valid
                    return HealthCheckResult.Healthy("RabbitMQ connection string is valid");
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy("RabbitMQ health check failed", ex);
                }
            }, tags: new[] { "messaging", "rabbitmq", "ready" })
            
            // Custom business health check
            .AddCheck<BusinessHealthCheck>("business-operations",
                failureStatus: HealthStatus.Degraded,
                tags: new[] { "business", "custom", "live" })
            
            // System metrics health check
            .AddCheck<SystemMetricsHealthCheck>("system-metrics",
                failureStatus: HealthStatus.Degraded,
                tags: new[] { "system", "metrics", "live" })
            
            // Memory health check (using a simple check)
            .AddCheck("memory", () =>
            {
                var memoryUsed = GC.GetTotalMemory(false);
                var memoryUsedMB = memoryUsed / (1024 * 1024);
                return memoryUsedMB > 1000 
                    ? HealthCheckResult.Degraded($"High memory usage: {memoryUsedMB}MB")
                    : HealthCheckResult.Healthy($"Memory usage: {memoryUsedMB}MB");
            }, tags: new[] { "system", "memory", "live" });

        // Add Health Checks UI
        services.AddHealthChecksUI(options =>
        {
            options.SetEvaluationTimeInSeconds(30); // Evaluate every 30 seconds
            options.MaximumHistoryEntriesPerEndpoint(50);
            options.AddHealthCheckEndpoint("PDV System", "/health");
            options.AddHealthCheckEndpoint("PDV System Ready", "/health/ready");
            options.AddHealthCheckEndpoint("PDV System Live", "/health/live");
        })
        .AddInMemoryStorage();

        return services;
    }

    public static IApplicationBuilder UseHealthCheckEndpoints(this IApplicationBuilder app)
    {
        // Detailed health check endpoint
        app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });

        // Readiness probe (for Kubernetes)
        app.UseHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });

        // Liveness probe (for Kubernetes)
        app.UseHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("live"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });

        // Simple health check endpoint
        app.UseHealthChecks("/health/simple", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            Predicate = _ => false, // Exclude all checks, just return healthy if app is running
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"status\":\"Healthy\",\"timestamp\":\"" + DateTime.UtcNow.ToString("O") + "\"}");
            }
        });

        // Health Checks UI
        app.UseHealthChecksUI(options =>
        {
            options.UIPath = "/health-ui";
            options.ApiPath = "/health-ui-api";
        });

        return app;
    }
}