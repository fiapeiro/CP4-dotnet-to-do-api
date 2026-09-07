using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CP4_to_do_api.Tarefas.Model;
using CP4_to_do_api.Tarefas.DTOs;
using CP4_to_do_api.Tarefas.Services;

namespace CP4_to_do_api.Tarefas.Contoller
{
    [ApiController]
    [Route("api/tarefas")]
    [Produces("application/json")]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaService _service;

        public TarefaController(TarefaService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TarefaResponse>>> GetTarefas()
        {
            var response = await _service.GetAllAsync();
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<TarefaResponse>> CriarTarefa([FromBody] TarefaRequest tarefaRequest)
        {
            var response = await _service.CreateAsync(tarefaRequest);
            return CreatedAtAction(nameof(GetTarefas), new { id = response.Id }, response);
        }
    }
}