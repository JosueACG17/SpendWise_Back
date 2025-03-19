using Microsoft.AspNetCore.Mvc;
using SpendWise.DTOs;
using SpendWise.Services;

namespace SpendWise.Controllers
{
    [ApiController]
    [Route("api/gastos")]
    public class GastoController : ControllerBase
    {
        private readonly GastoService _service;

        public GastoController(GastoService service)
        {
            _service = service;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<GastoDTO>>> GetAllByUsuarioId(int usuarioId)
        {
            var gastos = await _service.GetAllGastosByUsuarioIdAsync(usuarioId);
            return Ok(gastos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GastoDTO>> GetById(int id)
        {
            var gasto = await _service.GetGastoByIdAsync(id);
            if (gasto == null) return NotFound();
            return Ok(gasto);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] GastoDTO gastoDto)
        {
            await _service.AddGastoAsync(gastoDto);
            return CreatedAtAction(nameof(GetById), new { id = gastoDto.Id }, gastoDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] GastoDTO gastoDto)
        {
            if (id != gastoDto.Id) return BadRequest();
            await _service.UpdateGastoAsync(gastoDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteGastoAsync(id);
            return NoContent();
        }
    }
}
