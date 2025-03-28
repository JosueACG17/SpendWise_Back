using Microsoft.AspNetCore.Mvc;
using SpendWise.DTOs;
using SpendWise.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendWise.Controllers
{
    [ApiController]
    [Route("api/presupuestos")]
    public class PresupuestoController : ControllerBase
    {
        private readonly PresupuestoService _service;
        private readonly ErrorLogService _errorLogService;

        public PresupuestoController(PresupuestoService service, ErrorLogService errorLogService)
        {
            _service = service;
            _errorLogService = errorLogService;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<PresupuestoDTO>>> GetAllByUsuarioId(int usuarioId)
        {
            try
            {
                var presupuestos = await _service.GetAllByUsuarioIdAsync(usuarioId);
                return Ok(presupuestos);
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al buscar un presupuesto por usuario");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PresupuestoDTO>> GetById(int id)
        {
            try
            {
                var presupuesto = await _service.GetByIdAsync(id);
                if (presupuesto == null) return NotFound();
                return Ok(presupuesto);
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al buscar un presupuesto");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] PresupuestoDTO presupuestoDto)
        {
            try
            {
                await _service.AddAsync(presupuestoDto);
                return CreatedAtAction(nameof(GetById), new { id = presupuestoDto.Id }, presupuestoDto);
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al crear un presupuesto");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] PresupuestoDTO presupuestoDto)
        {
            try
            {
                if (id != presupuestoDto.Id) return BadRequest();
                await _service.UpdateAsync(presupuestoDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al editar un presupuesto");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al eliminar un presupuesto");
            }
        }
    }
}