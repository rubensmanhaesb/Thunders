using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TarefasApp.Application.Commands;
using TarefasApp.Application.Dtos;
using TarefasApp.Application.Interfaces;

namespace TarefasApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {

        private readonly ITarefaAppService? _tarefaAppService;

        public TarefasController(ITarefaAppService? tarefaAppService)
        {
            _tarefaAppService = tarefaAppService;
        }


        [HttpPost]
        [ProducesResponseType(typeof(TarefaDto), 201)]
        public async Task<IActionResult> Post(TarefaCreateCommand command)
        {
            var dto = await _tarefaAppService.Create(command);
            return StatusCode(201, dto);
        }

        [HttpPut]
        [ProducesResponseType(typeof(TarefaDto), 200)]
        public async Task<IActionResult> Put(TarefaUpdateCommand command)
        {
            var dto = await _tarefaAppService.Update(command);
            return StatusCode(200, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(TarefaDto), 200)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new TarefaDeleteCommand { Id = id };
            var dto = await _tarefaAppService.Delete(command);

            return StatusCode(200, dto);
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<TarefaDto>), 200)]
        public IActionResult GetAll()
        {
            var dtos = _tarefaAppService.GetAll();
            return StatusCode(200, dtos);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TarefaDto), 200)]
        public IActionResult GetById(Guid id)
        {
            var dto = _tarefaAppService.GetById(id);
            return StatusCode(200, dto);
        }
    }
}
