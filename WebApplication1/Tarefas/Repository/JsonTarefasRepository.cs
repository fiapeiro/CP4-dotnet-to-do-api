using CP4_to_do_api.Tarefas.Model;
using System.Text.Json;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CP4_to_do_api.Tarefas.Repository
{
    public class JsonTarefasRepository : TarefasRepository
    {
        private readonly string _filePath;
        private readonly object _lock = new();

        public JsonTarefasRepository(IConfiguration configuration)
        {
            _filePath = configuration["TarefasFile"] ?? Path.Combine(AppContext.BaseDirectory, "tarefas.json");

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        public async Task<List<Tarefa>> GetAllAsync()
        {
            using var stream = File.OpenRead(_filePath);
            var tarefas = await JsonSerializer.DeserializeAsync<List<Tarefa>>(stream) ?? new List<Tarefa>();
            return tarefas;
        }

        public Task<Tarefa> AddAsync(Tarefa tarefa)
        {
            lock (_lock)
            {
                var list = JsonSerializer.Deserialize<List<Tarefa>>(File.ReadAllText(_filePath)) ?? new List<Tarefa>();
                var nextId = list.Any() ? list.Max(t => t.Id) + 1 : 1;
                tarefa.SetId(nextId);
                list.Add(tarefa);
                File.WriteAllText(_filePath, JsonSerializer.Serialize(list));
                return Task.FromResult(tarefa);
            }
        }
    }
}
