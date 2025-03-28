using Microsoft.AspNetCore.Mvc;
using SpendWise.DTOs;
using SpendWise.Models;
using SpendWise.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendWise.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly RolesService _rolesService;
        private readonly ErrorLogService _errorLogService;

        public RolController(RolesService rolesService, ErrorLogService errorLogService)
        {
            _rolesService = rolesService;
            _errorLogService = errorLogService;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarRoles()
        {
            try
            {
                var roles = await _rolesService.GetAllRolesAsync();
                var rolesResponse = new List<RolDTO>();

                foreach (var rol in roles)
                {
                    rolesResponse.Add(new RolDTO
                    {
                        Id = rol.Id,
                        Nombre = rol.Nombre
                    });
                }

                return Ok(rolesResponse);
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al buscar los roles");
            }
        }
    }
}