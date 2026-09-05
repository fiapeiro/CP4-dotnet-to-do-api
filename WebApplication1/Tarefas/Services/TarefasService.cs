using CP4_to_do_api.Tarefas.Contoller;
using CP4_to_do_api.Tarefas.DTOs;
using CP4_to_do_api.Tarefas.Model;
using CP4_to_do_api.Tarefas.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CP4_to_do_api.Tarefas.Services
{
    public class TarefasService
    {
        private readonly TarefasRepository _repository;
        private readonly ILogger<TarefasService> _logger;

        public TarefasService(TarefasRepository repository, ILogger<TarefasService> logger)
        {
            _repository = repository;
            _logger = logger;
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

            return new TarefaResponse { Id = created.Id, Name = created.Nome };
        }
    }
}
