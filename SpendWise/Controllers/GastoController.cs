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
        private readonly ErrorLogService _errorLogService;

        public GastoController(GastoService service, ErrorLogService errorLogService)
        {
            _service = service;
            _errorLogService = errorLogService;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<GastoDTO>>> GetAllByUsuarioId(int usuarioId)
        {
            try
            {
                var gastos = await _service.GetAllGastosByUsuarioIdAsync(usuarioId);
                return Ok(gastos);
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al buscar un gasto por usuario");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GastoDTO>> GetById(int id)
        {
            try
            {
                var gasto = await _service.GetGastoByIdAsync(id);
                if (gasto == null) return NotFound();
                return Ok(gasto);
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al buscar un gasto");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] GastoDTO gastoDto)
        {
            try
            {
                await _service.AddGastoAsync(gastoDto);
                return CreatedAtAction(nameof(GetById), new { id = gastoDto.Id }, gastoDto);
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al crear un gasto");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] GastoDTO gastoDto)
        {
            try
            {
                if (id != gastoDto.Id) return BadRequest();
                await _service.UpdateGastoAsync(gastoDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al editar un gasto");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteGastoAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al eliminar un gasto");
            }
        }
    }
}
