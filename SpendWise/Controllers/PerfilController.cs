using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpendWise.DTOs;
using SpendWise.Models;
using SpendWise.Services;
using System;
using System.Threading.Tasks;

namespace SpendWise.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerfilController : ControllerBase
    {
        private readonly PerfilService _perfilService;
        private readonly AppDbContext _context;

        public PerfilController(PerfilService perfilService, AppDbContext context)
        {
            _perfilService = perfilService;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Perfil>> CreatePerfil([FromBody] PerfilDTO perfilDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var foto = Request.Form.Files["Foto"];
                var createdPerfil = await _perfilService.CreatePerfilAsync(perfilDto, foto);
                return CreatedAtAction(nameof(GetPerfil), new { id = createdPerfil.Id }, createdPerfil);
            }
            catch (Exception ex)
            {
                var errorLog = new ErrorLogs
                {
                    Mensaje_error = ex.Message,
                    Enlace_error = HttpContext.Request.Path,
                    Fecha_error = DateTime.UtcNow
                };
                _context.ErrorLogs.Add(errorLog);
                await _context.SaveChangesAsync();
                return StatusCode(500, new { message = "Ocurrió un error interno al registrar el usuario." });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Perfil>> GetPerfil(int id)
        {
            var perfil = await _perfilService.GetPerfilByIdAsync(id);
            if (perfil == null)
                return NotFound();

            return Ok(perfil);
        }
    }
}