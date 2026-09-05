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

        public TarefasService(TarefasRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TarefaResponse>> GetAllAsync()
        {
            var tarefas = await _repository.GetAllAsync();
            return tarefas.Select(t => new TarefaResponse { Id = t.Id, Name = t.Nome }).ToList();
        }

        public async Task<TarefaResponse> CreateAsync(TarefaRequest request)
        {
            var tarefa = new Tarefa(0, request.Nome);
            var created = await _repository.AddAsync(tarefa);
            return new TarefaResponse { Id = created.Id, Name = created.Nome };
        }
    }
}
