using Serilog;
using Serilog.Events;
using Sis_Pdv_Controle_Estoque_API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var loggerConfiguration = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .WriteTo.File("log.txt", restrictedToMinimumLevel: LogEventLevel.Information)
    .MinimumLevel.Verbose();

Log.Logger = loggerConfiguration.CreateLogger();

builder.Services.ConfigureRepositories(builder.Configuration);
builder.Services.ConfigureMediatR();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandler("/error");
app.UseHsts();
app.UseDeveloperExceptionPage();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
