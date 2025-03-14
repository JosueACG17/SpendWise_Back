using Microsoft.AspNetCore.Mvc;
using SpendWise.DTOs;
using SpendWise.Models;
using SpendWise.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendWise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly IPerfilService _perfilService;
        private readonly ErrorLogService _errorLogService;

        public PerfilController(IPerfilService perfilService, ErrorLogService errorLogService)
        {
            _perfilService = perfilService;
            _errorLogService = errorLogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Perfil>>> GetAllPerfiles()
        {
            var perfiles = await _perfilService.GetAllPerfilesAsync();
            return Ok(perfiles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Perfil>> GetPerfilById(int id)
        {
            try
            {
                var perfil = await _perfilService.GetPerfilByIdAsync(id);
                if (perfil == null)
                {
                    return NotFound();
                }
                return Ok(perfil);
            }
            catch (Exception ex)
            {
                // Registrar el error usando el servicio
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al buscar el perfil por usuario");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Perfil>> CreatePerfil([FromForm] PerfilDTO perfilDTO)
        {
            try
            {
                string folderName = "perfiles";
                var perfil = await _perfilService.CreatePerfilAsync(perfilDTO, folderName);
                return CreatedAtAction(nameof(GetPerfilById), new { id = perfil.Id }, perfil);
            }
            catch (KeyNotFoundException ex)
            {
                // Registrar el error usando el servicio
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return NotFound("Perfil no encontrado");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerfil(int id, [FromBody] PerfilDTO perfilDTO)
        {
            try
            {
                string folderName = "perfiles";
                await _perfilService.UpdatePerfilAsync(id, perfilDTO, folderName);
                return NoContent();
            }
            catch (Exception ex) // Captura cualquier excepción
            {
                // Registrar el error usando el servicio
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error interno. Por favor, contacte al administrador.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerfil(int id)
        {
            try
            {
                await _perfilService.DeletePerfilAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Registrar el error usando el servicio
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al eliminar el perfil.");
            }
        }
    }
}