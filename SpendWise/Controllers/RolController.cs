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

        public RolController(RolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarRoles()
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
    }
}