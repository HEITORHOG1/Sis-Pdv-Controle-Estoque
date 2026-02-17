using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Sis_Pdv_Controle_Estoque_API.Middleware;
using System.Text;

namespace Sis_Pdv_Controle_Estoque_API.Configuration
{
    /// <summary>
    /// Comprehensive security configuration
    /// </summary>
    public static class SecurityConfiguration
    {
        public static void ConfigureSecurity(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            // Configure JWT Authentication
            ConfigureJwtAuthentication(services, configuration, environment);

            // Configure Authorization Policies
            ConfigureAuthorizationPolicies(services);

            // Security configuration complete

            // Configure CORS
            services.ConfigureCors(environment, configuration);

            // Configure HTTPS Redirection
            // Only enable when NOT in Development, Docker, or running behind a reverse proxy (container)
            var isContainerized = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"))
                || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOCKER_CONTAINER"));
            var skipHttps = environment.IsDevelopment() 
                || environment.IsEnvironment("Docker") 
                || environment.IsEnvironment("Staging")
                || isContainerized;

            if (!skipHttps)
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = 443;
                });

                services.AddHsts(options =>
                {
                    options.Preload = true;
                    options.IncludeSubDomains = true;
                    options.MaxAge = TimeSpan.FromDays(365);
                });
            }

            // Configure Data Protection
            services.AddDataProtection();
        }

        private static void ConfigureJwtAuthentication(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var jwtSecret = configuration["Authentication:JwtSecret"] 
                ?? throw new InvalidOperationException("JWT Secret not configured");
            var key = Encoding.UTF8.GetBytes(jwtSecret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Only require HTTPS metadata when NOT running in a container or development
                var requireHttps = !environment.IsDevelopment() 
                    && !environment.IsEnvironment("Docker")
                    && !environment.IsEnvironment("Staging")
                    && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"));
                options.RequireHttpsMetadata = requireHttps;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Authentication:Issuer"],
                    ValidAudience = configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                    RequireSignedTokens = true
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogWarning("JWT Authentication failed: {Error}", context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        var userId = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                        logger.LogDebug("JWT Token validated for user {UserId}", userId);
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogWarning("JWT Challenge triggered: {Error}", context.Error);
                        return Task.CompletedTask;
                    }
                };
            });
        }

        private static void ConfigureAuthorizationPolicies(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Default policy requires authentication
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                // Note: No global fallback policy forcing authentication.
                // Endpoints without [Authorize] will be accessible (e.g., Swagger, health checks).

                // Admin-only policies
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("SuperAdminOnly", policy =>
                    policy.RequireRole("SuperAdmin"));

                // Manager policies
                options.AddPolicy("ManagerOrAbove", policy =>
                    policy.RequireRole("Manager", "Admin", "SuperAdmin"));

                // User management policies
                options.AddPolicy("UserManagement", policy =>
                    policy.RequireClaim("permission", "user.manage"));

                options.AddPolicy("RoleManagement", policy =>
                    policy.RequireClaim("permission", "role.manage"));

                // Inventory policies
                options.AddPolicy("InventoryRead", policy =>
                    policy.RequireClaim("permission", "inventory.read"));

                options.AddPolicy("InventoryWrite", policy =>
                    policy.RequireClaim("permission", "inventory.write"));

                options.AddPolicy("InventoryManage", policy =>
                    policy.RequireClaim("permission", "inventory.manage"));

                // Sales policies
                options.AddPolicy("SalesRead", policy =>
                    policy.RequireClaim("permission", "sales.read"));

                options.AddPolicy("SalesWrite", policy =>
                    policy.RequireClaim("permission", "sales.write"));

                options.AddPolicy("SalesManage", policy =>
                    policy.RequireClaim("permission", "sales.manage"));

                // Financial policies
                options.AddPolicy("FinancialRead", policy =>
                    policy.RequireClaim("permission", "financial.read"));

                options.AddPolicy("FinancialWrite", policy =>
                    policy.RequireClaim("permission", "financial.write"));

                // Reports policies
                options.AddPolicy("ReportsRead", policy =>
                    policy.RequireClaim("permission", "reports.read"));

                options.AddPolicy("ReportsGenerate", policy =>
                    policy.RequireClaim("permission", "reports.generate"));

                // System administration policies
                options.AddPolicy("SystemAdmin", policy =>
                    policy.RequireClaim("permission", "system.admin"));

                options.AddPolicy("BackupManage", policy =>
                    policy.RequireClaim("permission", "backup.manage"));

                options.AddPolicy("ConfigurationManage", policy =>
                    policy.RequireClaim("permission", "configuration.manage"));
            });
        }



        public static void UseSecurityMiddleware(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            // Security headers (should be early in pipeline)
            app.UseMiddleware<SecurityHeadersMiddleware>();

            // HTTPS redirection
            // Only enable when NOT in Development, Docker, Staging, or running in a container
            var isContainerized = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"))
                || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOCKER_CONTAINER"));
            if (!environment.IsDevelopment() && !environment.IsEnvironment("Docker") && !environment.IsEnvironment("Staging") && !isContainerized)
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            // Security middleware configured

            // CORS (before authentication and authorization)
            app.UseCorsPolicy(environment);

            // Authentication and Authorization
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}