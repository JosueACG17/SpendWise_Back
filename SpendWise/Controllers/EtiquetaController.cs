using Microsoft.AspNetCore.Mvc;
using SpendWise.DTOs;
using SpendWise.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendWise.Controllers
{
    [ApiController]
    [Route("api/etiquetas")]
    public class EtiquetaController : ControllerBase
    {
        private readonly EtiquetaService _service;

        public EtiquetaController(EtiquetaService service)
        {
            _service = service;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<EtiquetaDTO>>> GetAllByUsuarioId(int usuarioId)
        {
            var etiquetas = await _service.GetAllByUsuarioIdAsync(usuarioId);
            return Ok(etiquetas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EtiquetaDTO>> GetById(int id)
        {
            var etiqueta = await _service.GetByIdAsync(id);
            if (etiqueta == null) return NotFound();
            return Ok(etiqueta);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] EtiquetaDTO etiquetaDto)
        {
            await _service.AddAsync(etiquetaDto);
            return CreatedAtAction(nameof(GetById), new { id = etiquetaDto.Id }, etiquetaDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] EtiquetaDTO etiquetaDto)
        {
            if (id != etiquetaDto.Id) return BadRequest();
            await _service.UpdateAsync(etiquetaDto);
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