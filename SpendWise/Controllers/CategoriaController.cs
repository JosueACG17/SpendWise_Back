using Microsoft.AspNetCore.Mvc;
using SpendWise.DTOs;
using SpendWise.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendWise.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _service;
        private readonly ErrorLogService _errorLogService;

        public CategoriaController(CategoriaService service, ErrorLogService errorLogService)
        {
            _service = service;
            _errorLogService = errorLogService;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAllByUsuarioId(int usuarioId)
        {
            try
            {
                var categorias = await _service.GetAllByUsuarioIdAsync(usuarioId);
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al buscar la categoria por usuario");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetById(int id)
        {
            try
            {
                var categoria = await _service.GetByIdAsync(id);
                if (categoria == null) return NotFound();
                return Ok(categoria);
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al buscar la categoria");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                await _service.AddAsync(categoriaDto);
                return CreatedAtAction(nameof(GetById), new { id = categoriaDto.Id }, categoriaDto);
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al crear la categoria");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.Id) return BadRequest();
                await _service.UpdateAsync(categoriaDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al editar la categoria");
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
                return StatusCode(500, "Ocurrió un error al eliminar la categoria");
            }
        }

        [HttpGet("isInUse/{categoryId}")]
        public async Task<ActionResult<bool>> IsCategoryInUse(int categoryId)
        {
            try
            {
                var inUse = await _service.IsCategoryInUseAsync(categoryId);
                return Ok(new { inUse });
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error mientras la categoria esta en uso");
            }
        }
    }
}