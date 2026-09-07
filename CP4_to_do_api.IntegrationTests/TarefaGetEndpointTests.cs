using CP4_to_do_api.Tarefas.DTOs;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace CP4_to_do_api.IntegrationTests
{
    /// <summary>
    /// Testes de integracao do GET /api/tarefas. Compartilha a mesma
    /// TarefaApiFactory que TarefaPostEndpointTests, via ICollectionFixture
    /// (collection "TarefaApi") -- a aplicacao nao e recriada para esta classe.
    /// </summary>
    [Collection("TarefaApi")]
    public class TarefaGetEndpointTests
    {
        private readonly HttpClient _client;

        public TarefaGetEndpointTests(TarefaApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTarefas_TarefaExistente_RetornaOkComListaContendoATarefa()
        {
            // Arrange: garante que existe ao menos uma tarefa conhecida antes de listar
            var request = new TarefaRequest { Nome = "Tarefa para aparecer na listagem" };
            await _client.PostAsJsonAsync("/api/tarefas", request);

            // Act
            var httpResponse = await _client.GetAsync("/api/tarefas");

            // Assert
            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

            var tarefas = await httpResponse.Content.ReadFromJsonAsync<List<TarefaResponse>>();
            Assert.NotNull(tarefas);
            Assert.Contains(tarefas!, t => t.Name == "Tarefa para aparecer na listagem");
        }
    }
}
