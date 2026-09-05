using CP4_to_do_api.Tarefas.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CP4_to_do_api.Tarefas.Repository
{
    public interface TarefasRepository
    {
        Task<List<Tarefa>> GetAllAsync();
        Task<Tarefa> AddAsync(Tarefa tarefa);
    }
}
