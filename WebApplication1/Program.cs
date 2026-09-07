using Serilog;
using Serilog.Events;
using CP4_to_do_api.Tarefas.Infrastructure; // Added using statement for infrastructure extension

// Configuração do Serilog (Log Estruturado)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    // Renderiza logs em formato JSON ou estruturado no console
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Iniciando a API de Tarefas");
    var builder = WebApplication.CreateBuilder(args);

    // Substitui o logger padrão do ASP.NET pelo Serilog
    builder.Host.UseSerilog();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApi();

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddHealthChecks();

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    // Injetando as dependências da infraestrutura (OpenTelemetry, Services)
    builder.Services.AddInfrastructure();

    // optional: set default file path for tarefas.json inside app folder
    builder.Configuration["TarefasFile"] ??= Path.Combine(AppContext.BaseDirectory, "tarefas.json");

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.MapHealthChecks("/health");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação falhou ao iniciar.");
}
finally
{
    Log.CloseAndFlush();
}


// Necessario para que o WebApplicationFactory<Program>, usado nos testes de
// integracao, consiga enxergar esta classe -- top-level statements geram uma
// classe "Program" implicita e interna; esta declaracao parcial a torna
// acessivel (e publica) para os projetos de teste.
public partial class Program { }
