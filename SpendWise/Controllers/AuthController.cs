using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SpendWise.DTOs;
using SpendWise.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UsuariosService _usuariosService;
    private readonly JwtService _jwtService;
    private readonly IConfiguration _configuration;

    public AuthController(UsuariosService usuariosService, JwtService jwtService, IConfiguration configuration)
    {
        _usuariosService = usuariosService;
        _jwtService = jwtService;
        _configuration = configuration; 
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthDTO authDTO)
    {
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

    [Authorize]
    [HttpGet("validate-token")]
    public IActionResult ValidateToken()
    {
        try
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            var token = authHeader.Substring(7);
            var jwtSettings = _configuration.GetSection("Jwt"); 
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out _);
            return Ok(new { message = "Token válido" });
        }
        catch
        {
            return Unauthorized(new { message = "Token inválido o expirado" });
        }
    }
}