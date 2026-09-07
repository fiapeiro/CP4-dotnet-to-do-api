using System.ComponentModel.DataAnnotations;

namespace CP4_to_do_api.Tarefas.DTOs
{
    public class TarefaRequest
    {
        [Required(ErrorMessage = "O nome da tarefa é obrigatório.")]
        public string Nome { get; set; }
    }
}