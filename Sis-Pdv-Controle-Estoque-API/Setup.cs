using Commands.Categoria.AdicionarCategoria;
using Commands.Categoria.RemoverCategoria;
using Commands.Cliente.AdicionarCliente;
using Commands.Colaborador.AdicionarColaborador;
using Commands.Departamento.AdicionarDepartamento;
using Commands.Fornecedor.AdicionarFornecedor;
using Commands.Fornecedor.AlterarFornecedor;
using Commands.Fornecedor.ListarFornecedor;
using Commands.Fornecedor.ListarFornecedorPorId;
using Commands.Fornecedor.ListarFornecedorPorNomeDepartamento;
using Commands.Produto.AdicionarProduto;
using Commands.Produto.AlterarProduto;
using Commands.Produto.ListarProdutoPorNomeProduto;
using Commands.Produto.RemoverProduto;
using Commands.Usuarios.AlterarUsuario;
using Interfaces;
using MediatR;
using Repositories;
using Repositories.Base;
using Repositories.Transactions;
using Services.Stock;
using Interfaces.Services;
using Sis_Pdv_Controle_Estoque_API.RabbitMQSender;
using Sis_Pdv_Controle_Estoque_API.Services;
using Sis_Pdv_Controle_Estoque_API.Services.Auth;
using Sis_Pdv_Controle_Estoque_API.Services.Cache;
using Sis_Pdv_Controle_Estoque_API.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interceptors;

namespace Sis_Pdv_Controle_Estoque_API
{
    public static class Setup
    {
        public static void ConfigureMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.RegisterServicesFromAssembly(typeof(AdicionarCategoriaRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AdicionarDepartamentoRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AdicionarColaboradorRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AdicionarFornecedorRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AdicionarProdutoRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AdicionarClienteRequest).GetTypeInfo().Assembly);

                cfg.RegisterServicesFromAssembly(typeof(AlterarUsuarioRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AlterarFornecedorRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(AlterarProdutoRequest).GetTypeInfo().Assembly);

                cfg.RegisterServicesFromAssembly(typeof(ListarFornecedorRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ListarFornecedorPorIdRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ListarFornecedorPorNomeFornecedorRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ListarProdutoPorCodBarrasRequest).GetTypeInfo().Assembly);
                
                // Register new paginated handlers
                cfg.RegisterServicesFromAssembly(typeof(Commands.Produto.ListarProdutosPaginado.ListarProdutosPaginadoRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(Commands.Cliente.ListarClientesPaginado.ListarClientesPaginadoRequest).GetTypeInfo().Assembly);
                
                // Register user management handlers
                cfg.RegisterServicesFromAssembly(typeof(Commands.Usuarios.RegistrarUsuario.RegistrarUsuarioRequest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(Commands.Roles.CriarRole.CriarRoleRequest).GetTypeInfo().Assembly);

                cfg.RegisterServicesFromAssembly(typeof(RemoverCategoriaResquest).GetTypeInfo().Assembly);
                cfg.RegisterServicesFromAssembly(typeof(RemoverProdutoResquest).GetTypeInfo().Assembly);
            });

            // Add pipeline behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Sis_Pdv_Controle_Estoque_API.Behaviors.ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Sis_Pdv_Controle_Estoque_API.Behaviors.CachingBehavior<,>));
        }

        public static void ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            // Register audit interceptor
            services.AddScoped<AuditInterceptor>();
            
            services.AddScoped<PdvContext, PdvContext>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IRepositoryCategoria, RepositoryCategoria>();

            services.AddTransient<IRepositoryDepartamento, RepositoryDepartamento>();

            services.AddTransient<IRepositoryColaborador, RepositoryColaborador>();

            services.AddTransient<IRepositoryUsuario, RepositoryUsuario>();

            services.AddTransient<IRepositoryFornecedor, RepositoryFornecedor>();

            services.AddTransient<IRepositoryProduto, RepositoryProduto>();

            services.AddTransient<IRepositoryCliente, RepositoryCliente>();

            services.AddTransient<IRepositoryPedido, RepositoryPedido>();

            services.AddTransient<IRepositoryProdutoPedido, RepositoryProdutoPedido>();

            // Authentication repositories
            services.AddTransient<IRepositoryRole, RepositoryRole>();
            services.AddTransient<IRepositoryPermission, RepositoryPermission>();
            services.AddTransient<IRepositoryUserRole, RepositoryUserRole>();
            services.AddTransient<IRepositoryRolePermission, RepositoryRolePermission>();
            services.AddTransient<IRepositoryAuditLog, RepositoryAuditLog>();
            services.AddTransient<IRepositoryUserSession, RepositoryUserSession>();
            services.AddTransient<Interfaces.Repositories.IRepositoryStockMovement, RepositoryStockMovement>();

            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));
            services.AddTransient<IRabbitMQMessageSender, RabbitMQMessageSender>();
            
            // Register application services
            services.AddScoped<IApplicationLogger, ApplicationLogger>();
            
            // Authentication services
            services.AddScoped<Sis_Pdv_Controle_Estoque_API.Services.Auth.IJwtTokenService, Sis_Pdv_Controle_Estoque_API.Services.Auth.JwtTokenService>();
            
            // Inventory services
            services.AddScoped<IStockValidationService, StockValidationService>();
            services.AddScoped<Sis_Pdv_Controle_Estoque_API.Services.Auth.IPasswordService, PasswordService>();
            services.AddScoped<Interfaces.Services.IPasswordService, PasswordService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<AuthSeederService>();
            
            // Cache services
            services.Configure<CacheOptions>(configuration.GetSection(CacheOptions.SectionName));
            services.AddMemoryCache();
            services.AddScoped<ICacheService, MemoryCacheService>();
            
            // Report services
            services.AddScoped<Interfaces.Services.IReportDataService, Sis_Pdv_Controle_Estoque_API.Services.Reports.ReportDataService>();
            services.AddScoped<Interfaces.Services.IReportService, Sis_Pdv_Controle_Estoque_API.Services.Reports.ReportService>();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSecret = configuration["Authentication:JwtSecret"] ?? throw new InvalidOperationException("JWT Secret not configured");
            var key = Encoding.UTF8.GetBytes(jwtSecret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Set to true in production
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
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogError(context.Exception, "Authentication failed");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        var userId = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                        logger.LogDebug("Token validated for user {UserId}", userId);
                        return Task.CompletedTask;
                    }
                };
            });
        }

        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Add default policy
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                // Add specific permission policies
                options.AddPolicy("RequireUserManagementPermission", policy =>
                    policy.RequireClaim("permission", "user.manage"));
                    
                options.AddPolicy("RequireRoleManagementPermission", policy =>
                    policy.RequireClaim("permission", "role.manage"));
                    
                // Inventory management policies
                options.AddPolicy("InventoryManagement", policy =>
                    policy.RequireClaim("permission", "inventory.manage"));
                    
                options.AddPolicy("InventoryView", policy =>
                    policy.RequireClaim("permission", "inventory.view"));

                // Add permission-based policies dynamically
                // This will be handled by the PermissionAuthorizationHandler
            });

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }

        public static void ConfigureValidation(this IServiceCollection services)
        {
            // Register all validators from the domain assembly
            services.AddValidatorsFromAssembly(typeof(AdicionarClienteRequest).Assembly);
            
            // Configure FluentValidation to work with ASP.NET Core
            services.Configure<ApiBehaviorOptions>(options =>
            {
                // Disable automatic model state validation since we're handling it in the pipeline
                options.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}
