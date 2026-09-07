using CP4_to_do_api.Tarefas.Contoller;
using CP4_to_do_api.Tarefas.Diagnostic;
using CP4_to_do_api.Tarefas.DTOs;
using CP4_to_do_api.Tarefas.Model;
using CP4_to_do_api.Tarefas.Repository;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;

namespace CP4_to_do_api.Tarefas.Services
{
    public class TarefaService
    {
        private readonly TarefaRepository _repository;
        private readonly ILogger<TarefaService> _logger;
        private readonly Counter<int> _contadorTarefas;

        public TarefaService(TarefaRepository repository, ILogger<TarefaService> logger, IMeterFactory meterFactory)
        {
            _repository = repository;
            _logger = logger;
            var meter = meterFactory.Create(TarefaConstants.MeterName);
            _contadorTarefas = meter.CreateCounter<int>("games_created_total", description: "Total de jogos criados");
        }

        public async Task<List<TarefaResponse>> GetAllAsync()
        {
            var tarefas = await _repository.GetAllAsync();
            _logger.LogInformation("Recuperando todas as tarefas. Total: {TotalTarefas}", tarefas.Count);
            return tarefas.Select(t => new TarefaResponse { Id = t.Id, Name = t.Nome }).ToList();
        }

        public async Task<TarefaResponse> CreateAsync(TarefaRequest request)
        {
            var tarefa = new Tarefa(0, request.Nome);

            if (string.IsNullOrWhiteSpace(tarefa.Nome))
            {
                _logger.LogWarning(
                    "Tentativa de criar tarefa sem título"
                );

                throw new ArgumentException(
                    "O título da tarefa é obrigatório."
                );
            }

            var created = await _repository.AddAsync(tarefa);

            _logger.LogInformation(
                "Nova tarefa criada: {NomeTarefa}",
                tarefa.Nome
            );

            _contadorTarefas.Add(1, new KeyValuePair<string, object?>("nome", tarefa.Nome));

            return new TarefaResponse { Id = created.Id, Name = created.Nome };
        }
    }
}
