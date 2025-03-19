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

        public PresupuestoController(PresupuestoService service)
        {
            _service = service;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<PresupuestoDTO>>> GetAllByUsuarioId(int usuarioId)
        {
            var presupuestos = await _service.GetAllByUsuarioIdAsync(usuarioId);
            return Ok(presupuestos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PresupuestoDTO>> GetById(int id)
        {
            var presupuesto = await _service.GetByIdAsync(id);
            if (presupuesto == null) return NotFound();
            return Ok(presupuesto);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] PresupuestoDTO presupuestoDto)
        {
            await _service.AddAsync(presupuestoDto);
            return CreatedAtAction(nameof(GetById), new { id = presupuestoDto.Id }, presupuestoDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] PresupuestoDTO presupuestoDto)
        {
            if (id != presupuestoDto.Id) return BadRequest();
            await _service.UpdateAsync(presupuestoDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}