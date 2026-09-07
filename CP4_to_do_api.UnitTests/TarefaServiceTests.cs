using CP4_to_do_api.Tarefas.DTOs;
using CP4_to_do_api.Tarefas.Model;
using CP4_to_do_api.Tarefas.Repository;
using CP4_to_do_api.Tarefas.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CP4_to_do_api.UnitTests
{
    /// <summary>
    /// Testes de unidade do TarefaService, seguindo o padrao AAA
    /// (Arrange, Act, Assert) e mockando a dependencia TarefaRepository
    /// com Moq para manter o teste isolado.
    ///
    /// Nomenclatura: MetodoTestado_EstadoSobTeste_ComportamentoEsperado.
    /// </summary>
    public class TarefaServiceTests
    {
        private readonly Mock<TarefaRepository> _repositoryMock;
        private readonly TarefaService _service;

        public TarefaServiceTests()
        {
            _repositoryMock = new Mock<TarefaRepository>();
            _service = new TarefaService(
                _repositoryMock.Object,
                NullLogger<TarefaService>.Instance,
                new TestMeterFactory());
        }

        [Fact]
        public async Task CreateAsync_TituloValido_RetornaTarefaCriadaEChamaRepositorio()
        {
            // Arrange
            var request = new TarefaRequest { Nome = "Estudar para o CP4" };

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Tarefa>()))
                .ReturnsAsync((Tarefa tarefa) =>
                {
                    tarefa.SetId(1);
                    return tarefa;
                });

            // Act
            var response = await _service.CreateAsync(request);

            // Assert
            Assert.Equal(1, response.Id);
            Assert.Equal("Estudar para o CP4", response.Name);
            _repositoryMock.Verify(r => r.AddAsync(It.Is<Tarefa>(t => t.Nome == "Estudar para o CP4")), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateAsync_TituloNuloVazioOuEmBranco_LancaArgumentExceptionENaoChamaRepositorio(string? nomeInvalido)
        {
            // Arrange
            var request = new TarefaRequest { Nome = nomeInvalido! };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(request));

            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Tarefa>()), Times.Never);
        }

        [Fact]
        public async Task GetAllAsync_TarefasExistentesNoRepositorio_RetornaListaMapeadaParaResponse()
        {
            // Arrange
            var tarefasNoRepositorio = new List<Tarefa>
            {
                new Tarefa(1, "Tarefa 1"),
                new Tarefa(2, "Tarefa 2"),
            };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tarefasNoRepositorio);

            // Act
            var response = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, response.Count);
            Assert.Contains(response, r => r.Id == 1 && r.Name == "Tarefa 1");
            Assert.Contains(response, r => r.Id == 2 && r.Name == "Tarefa 2");
        }
    }
}
