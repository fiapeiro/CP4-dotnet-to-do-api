using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace CP4_to_do_api.IntegrationTests
{
    /// <summary>
    /// WebApplicationFactory customizada: redireciona o TarefasFile para um
    /// arquivo JSON temporario exclusivo desta execucao de testes, em vez do
    /// tarefas.json usado em desenvolvimento. Isso evita sujar dados reais e
    /// deixar os testes dependentes de execucoes anteriores.
    /// </summary>
    public class TarefaApiFactory : WebApplicationFactory<Program>
    {
        public string TarefasFilePath { get; } =
            Path.Combine(Path.GetTempPath(), $"tarefas-tests-{Guid.NewGuid():N}.json");

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["TarefasFile"] = TarefasFilePath
                });
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && File.Exists(TarefasFilePath))
            {
                File.Delete(TarefasFilePath);
            }
        }
    }

    /// <summary>
    /// Definicao da collection do xUnit: permite que varias classes de teste
    /// (TarefaPostEndpointTests, TarefaGetEndpointTests) compartilhem a mesma
    /// instancia de TarefaApiFactory, em vez de subir a aplicacao do zero para
    /// cada classe.
    /// </summary>
    [CollectionDefinition("TarefaApi")]
    public class TarefaApiCollection : ICollectionFixture<TarefaApiFactory>
    {
    }
}
