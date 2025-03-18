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
    public class GastosController : ControllerBase
    {
        private readonly IGastosService _gastosService;
        private readonly ErrorLogService _errorLogService;

        public GastosController(IGastosService gastosService, ErrorLogService errorLogService)
        {
            _gastosService = gastosService;
            _errorLogService = errorLogService;
        }

        //GET ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gasto>>> GetAllGastos()
        {
            var gastos = await _gastosService.GetAllGastosAsync();
            return Ok(gastos);
        }

        //GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Gasto>> GetGastoById(int id)
        {
            try
            {
                var gasto = await _gastosService.GetGastosByIdAsync(id);
                if (gasto == null)
                {
                    return NotFound();
                }
                return Ok(gasto);
            }
            catch (Exception ex)
            {
                // Registrar el error usando el servicio
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al buscar el perfil por usuario");
            }
        }

        //CREATE NEW
        [HttpPost]
        public async Task<ActionResult<Gasto>> CreateGasto([FromForm] GastoDTO gastoDTO)
        {
            try
            {
                string folderName = "gastos";
                var gasto = await _gastosService.CreateGastosAsync(gastoDTO, folderName);
                return CreatedAtAction(nameof(GetGastoById), new { id = gasto.Id }, gasto);
            }
            catch (KeyNotFoundException ex)
            {
                // Registrar el error usando el servicio
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return NotFound("Perfil no encontrado");
            }
        }

        //UPDATE BY ID
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerfil(int id, [FromBody] GastoDTO gastoDTO)
        {
            try
            {
                string folderName = "gastos";
                await _gastosService.UpdateGastosAsync(id, gastoDTO, folderName);
                return NoContent();
            }
            catch (Exception ex) // Captura cualquier excepción
            {
                // Registrar el error usando el servicio
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error interno. Por favor, contacte al administrador.");
            }
        }

        //DELETE BY ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGastos(int id)
        {
            try
            {
                await _gastosService.DeleteGastosAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Registrar el error usando el servicio
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al eliminar el gasto.");
            }
        }
    }
}