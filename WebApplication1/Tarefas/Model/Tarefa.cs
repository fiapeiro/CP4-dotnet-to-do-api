namespace CP4_to_do_api.Tarefas.Model
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public Tarefa(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public void SetId(int id) => Id = id;
      

        public void SetNome(string nome) => Nome = nome;
    }
}
