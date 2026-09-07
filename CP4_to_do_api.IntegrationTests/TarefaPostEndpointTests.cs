using CP4_to_do_api.Tarefas.DTOs;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace CP4_to_do_api.IntegrationTests
{
    /// <summary>
    /// Testes de integracao do POST /api/tarefas via WebApplicationFactory.
    /// Faz parte da collection "TarefaApi", compartilhando a mesma instancia
    /// de TarefaApiFactory com TarefaGetEndpointTests (ICollectionFixture).
    /// </summary>
    [Collection("TarefaApi")]
    public class TarefaPostEndpointTests
    {
        private readonly HttpClient _client;

        public TarefaPostEndpointTests(TarefaApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task PostTarefas_DadosValidos_RetornaCreatedComATarefaCriada()
        {
            // Arrange
            var request = new TarefaRequest { Nome = "Estudar para o CP4" };

            // Act
            var httpResponse = await _client.PostAsJsonAsync("/api/tarefas", request);

            // Assert
            Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
            Assert.NotNull(httpResponse.Headers.Location);

            var tarefaCriada = await httpResponse.Content.ReadFromJsonAsync<TarefaResponse>();
            Assert.NotNull(tarefaCriada);
            Assert.True(tarefaCriada!.Id > 0);
            Assert.Equal("Estudar para o CP4", tarefaCriada.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task PostTarefas_SemTitulo_RetornaBadRequest(string nomeInvalido)
        {
            // Arrange
            var request = new TarefaRequest { Nome = nomeInvalido };

            // Act
            var httpResponse = await _client.PostAsJsonAsync("/api/tarefas", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        }
    }
}
