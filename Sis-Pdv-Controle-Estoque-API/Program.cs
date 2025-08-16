using Serilog;
using Serilog.Events;
using Sis_Pdv_Controle_Estoque_API;
using Sis_Pdv_Controle_Estoque_API.Configuration;
using Sis_Pdv_Controle_Estoque_API.Middleware;
using Sis_Pdv_Controle_Estoque_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure enhanced Serilog with enrichers and structured logging
var loggerConfiguration = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithCorrelationId()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .Enrich.WithProcessId()
    .Enrich.WithThreadId()
    .WriteTo.Console(
        restrictedToMinimumLevel: LogEventLevel.Debug,
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File(
        "logs/pdv-api-.log",
        restrictedToMinimumLevel: LogEventLevel.Information,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {CorrelationId} {Message:lj} {Properties:j}{NewLine}{Exception}")
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("System", LogEventLevel.Warning);

// Set minimum level based on environment
if (builder.Environment.IsDevelopment())
{
    loggerConfiguration.MinimumLevel.Debug();
}
else
{
    loggerConfiguration.MinimumLevel.Information();
}

Log.Logger = loggerConfiguration.CreateLogger();

// Use Serilog for all logging
builder.Host.UseSerilog();

// Add services to the container
builder.Services.ConfigureRepositories(builder.Configuration);
builder.Services.ConfigureMediatR();
builder.Services.ConfigureValidation();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureHealthChecks(builder.Configuration);

// Configure API versioning
builder.Services.ConfigureApiVersioning();

// Configure controllers with enhanced JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = builder.Environment.IsDevelopment();
    });

// Configure API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger(builder.Environment);

// Configure CORS for development
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline with proper middleware order
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors();
}
else
{
    app.UseHsts();
}

// Add global exception handling middleware (must be early in pipeline)
app.UseMiddleware<GlobalExceptionMiddleware>();

// Add request logging middleware
app.UseMiddleware<RequestLoggingMiddleware>();

// Add metrics collection middleware
app.UseMiddleware<Sis_Pdv_Controle_Estoque_API.Middleware.MetricsMiddleware>();

app.UseHttpsRedirection();

// Configure Swagger documentation
app.UseSwaggerDocumentation(app.Environment);

app.UseStaticFiles();

// Authentication and Authorization middleware (order is important)
app.UseAuthentication();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseAuthorization();

// Configure health check endpoints
app.UseHealthCheckEndpoints();

app.MapControllers();

// Log application startup
Log.Information("PDV Control System API starting up...");
Log.Information("Environment: {Environment}", app.Environment.EnvironmentName);

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
