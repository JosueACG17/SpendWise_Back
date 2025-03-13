using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpendWise.DTOs;
using SpendWise.Models;
using SpendWise.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UsuariosService _usuariosService;
    private readonly JwtService _jwtService;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private readonly UserTokenService _userTokenService;


    public AuthController(UsuariosService usuariosService, JwtService jwtService, UserTokenService userTokenService, IConfiguration configuration, AppDbContext context)
    {
        _usuariosService = usuariosService;
        _jwtService = jwtService;
        _configuration = configuration;
        _context = context;
        _userTokenService = userTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthDTO authDTO)
    {
        try
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
                FechaRegistro = DateTime.UtcNow,
                RolId = 2 
            };

            await _usuariosService.AddUsuarioAsync(usuario);
            return Ok(new { message = "Usuario registrado exitosamente" });
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthDTO authDTO)
    {
        try
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

            var existingToken = await _context.Tokens
                .FirstOrDefaultAsync(t => t.UsuarioId == usuario.Id && t.FechaExpiracion > DateTime.UtcNow);

            if (existingToken != null)
            {
                return Ok(new { token = existingToken.JwtToken });
            }

            var token = _jwtService.GenerateToken(usuario.Id, usuario.Email, usuario.Rol.Nombre);

            var tokenEntity = new Token
            {
                JwtToken = token,
                UsuarioId = usuario.Id,
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]))
            };

            _context.Tokens.Add(tokenEntity);
            await _context.SaveChangesAsync();

            return Ok(new { token });
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
            return StatusCode(500, new { message = "Ocurrió un error interno al iniciar sesión." });
        }
    }

    [Authorize]
    [HttpGet("validate-token")]
    public async Task<IActionResult> ValidateToken()
    {
        try
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            var token = authHeader.Substring(7);

            var tokenEntity = await _context.Tokens
                .FirstOrDefaultAsync(t => t.JwtToken == token && t.FechaExpiracion > DateTime.UtcNow);

            if (tokenEntity == null)
            {
                return Unauthorized(new { message = "Token inválido o expirado" });
            }

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
            return Unauthorized(new { message = "Token inválido o expirado" });
        }
    }

    [HttpDelete("logout")]
    public async Task<IActionResult> Logout([FromBody] TokenDto tokenDto)
    {
        try
        {
            var tokenEntity = await _context.Tokens
                .FirstOrDefaultAsync(t => t.JwtToken == tokenDto.Token);
            if (tokenEntity == null)
            {
                return NotFound(new { message = "Token no encontrado" });
            }
            _context.Tokens.Remove(tokenEntity);
            await _context.SaveChangesAsync();
            return NoContent();
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

            return StatusCode(500, new { message = "Ocurrió un error al cerrar la sesión." });
        }
    }
}