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
        private readonly ErrorLogService _errorLogService;

        public UsuarioController(UsuariosService usuariosService, ErrorLogService errorLogService)
        {
            _usuariosService = usuariosService;
            _errorLogService = errorLogService;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioDTO usuarioDTO)
        {
            try
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
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al crear un usuario");
            }
        }

        [HttpGet("obtener/{id}")]
        public async Task<IActionResult> ObtenerUsuario(int id)
        {
            try
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
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al buscar un usuario");
            }
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = await _usuariosService.GetUsuarioByIdAsync(id);
                if (usuario == null)
                    return NotFound(new { message = "Usuario no encontrado" });

            usuario.Email = usuarioDTO.Email;
            usuario.RolId = usuarioDTO.RolId;

            if (!string.IsNullOrEmpty(usuarioDTO.Contraseña))
            {
                usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuarioDTO.Contraseña);
            }

                await _usuariosService.UpdateUsuarioAsync(usuario);
                return Ok(new { message = "Usuario actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al editar un usuario");
            }
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            try
            {
                var usuario = await _usuariosService.GetUsuarioByIdAsync(id);
                if (usuario == null)
                    return NotFound(new { message = "Usuario no encontrado" });

                await _usuariosService.DeleteUsuarioAsync(id);
                return Ok(new { message = "Usuario eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al eliminar un usuario");
            }
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarUsuarios()
        {
            try
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
            catch (Exception ex)
            {
                await _errorLogService.CreateErrorAsync(ex.Message, HttpContext.Request.Path);
                return StatusCode(500, "Ocurrió un error al buscar todos los usuarios");
            }
        }
    }
}