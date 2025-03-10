using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendWise.DTOs;
using SpendWise.Models;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UsuariosService _usuariosService;
    private readonly JwtService _jwtService;

    public AuthController(UsuariosService usuariosService, JwtService jwtService)
    {
        _usuariosService = usuariosService;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthDTO authDTO)
    {
        // Verificar si el correo ya existe
        var existingUser = await _usuariosService.GetUsuarioByEmailAsync(authDTO.Email);
        if (existingUser != null)
        {
            return BadRequest(new { message = "El correo ya está en uso." });
        }

        var usuario = new Usuario
        {
            Email = authDTO.Email,
            Contraseña = BCrypt.Net.BCrypt.HashPassword(authDTO.Contraseña),
            FechaRegistro = DateTime.UtcNow
        };

        await _usuariosService.AddUsuarioAsync(usuario);
        return Ok(new { message = "Usuario registrado exitosamente" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthDTO authDTO)
    {
        if (authDTO == null)
        {
            return BadRequest(new { message = "Datos de inicio de sesión inválidos" });
        }

        var usuario = await _usuariosService.GetUsuarioByEmailAsync(authDTO.Email);
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(authDTO.Contraseña, usuario.Contraseña))
        {
            return Unauthorized(new { message = "Credenciales inválidas" });
        }

        var token = _jwtService.GenerateToken(usuario.Id, usuario.Email);
        return Ok(new { token });
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        // Los JWT son stateless, no se pueden invalidar directamente.
        // El "logout" se maneja en el frontend eliminando el token de la memoria.
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok(new { message = "Sesión cerrada exitosamente", userId });
    }
}
