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
    public class UsuarioController : ControllerBase
    {
        private readonly UsuariosService _usuariosService;

        public UsuarioController(UsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioDTO usuarioDTO)
        {
            var usuario = new Usuario
            {
                Email = usuarioDTO.Email,
                Contraseña = BCrypt.Net.BCrypt.HashPassword(usuarioDTO.Contraseña),
                FechaRegistro = DateTime.UtcNow,
                RolId = usuarioDTO.RolId
            };

            await _usuariosService.AddUsuarioAsync(usuario);
            return Ok(new { message = "Usuario creado exitosamente" });
        }

        [HttpGet("obtener/{id}")]
        public async Task<IActionResult> ObtenerUsuario(int id)
        {
            var usuario = await _usuariosService.GetUsuarioByIdAsync(id);
            if (usuario == null)
                return NotFound(new { message = "Usuario no encontrado" });

            var usuarioResponse = new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Email = usuario.Email,
                FechaRegistro = usuario.FechaRegistro,
                RolId = usuario.RolId,
                RolNombre = usuario.Rol.Nombre
            };

            return Ok(usuarioResponse);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] UsuarioDTO usuarioDTO)
        {
            var usuario = await _usuariosService.GetUsuarioByIdAsync(id);
            if (usuario == null)
                return NotFound(new { message = "Usuario no encontrado" });

            usuario.Email = usuarioDTO.Email;
            usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuarioDTO.Contraseña);
            usuario.RolId = usuarioDTO.RolId;

            await _usuariosService.UpdateUsuarioAsync(usuario);
            return Ok(new { message = "Usuario actualizado exitosamente" });
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _usuariosService.GetUsuarioByIdAsync(id);
            if (usuario == null)
                return NotFound(new { message = "Usuario no encontrado" });

            await _usuariosService.DeleteUsuarioAsync(id);
            return Ok(new { message = "Usuario eliminado exitosamente" });
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _usuariosService.GetAllUsuariosAsync();
            var usuariosResponse = new List<UsuarioResponseDTO>();

            foreach (var usuario in usuarios)
            {
                usuariosResponse.Add(new UsuarioResponseDTO
                {
                    Id = usuario.Id,
                    Email = usuario.Email,
                    FechaRegistro = usuario.FechaRegistro,
                    RolId = usuario.RolId,
                    RolNombre = usuario.Rol.Nombre
                });
            }

            return Ok(usuariosResponse);
        }
    }
}