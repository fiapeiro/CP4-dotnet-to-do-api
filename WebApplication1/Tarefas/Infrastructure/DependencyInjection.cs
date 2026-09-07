using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using CP4_to_do_api.Tarefas.Services;
using CP4_to_do_api.Tarefas.Diagnostic;
using CP4_to_do_api.Tarefas.Repository;

namespace CP4_to_do_api.Tarefas.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<TarefaRepository, JsonTarefasRepository>();
            services.AddScoped<TarefaService>();

            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(TarefaConstants.ServiceName);

            // Configuração do OpenTelemetry
            services.AddOpenTelemetry()
                .WithTracing(tracerProviderBuilder =>
                {
                    tracerProviderBuilder
                        .SetResourceBuilder(resourceBuilder)
                        .AddAspNetCoreInstrumentation() // Captura requisições HTTP de entrada
                        .AddHttpClientInstrumentation() // Captura requisições HTTP de saída
                        .AddSource(TarefaConstants.ServiceName); // Assina nossos traces customizados
                })
                .WithMetrics(meterProviderBuilder =>
                {
                    meterProviderBuilder
                        .SetResourceBuilder(resourceBuilder)
                        .AddAspNetCoreInstrumentation() // Métricas padrão do ASP.NET Core
                        .AddHttpClientInstrumentation()
                        .AddMeter(TarefaConstants.MeterName); // Assina nossas métricas customizadas
                });

            return services;
        }
    }

}
