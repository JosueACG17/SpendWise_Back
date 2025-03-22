using DotNetEnv; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CloudinaryDotNet;
using SpendWise.Repositories;
using SpendWise.Services;
using SpendWise.Services;
using SpendWise.Mappings;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno desde el archivo .env
Env.Load();

// Configuraci�n de la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
        Environment.GetEnvironmentVariable("DefaultConnection")));

// Configuraci�n de JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");

var key = Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? Environment.GetEnvironmentVariable("JWT__Key") ?? "ClavePorDefecto");
var issuer = jwtSettings["Issuer"] ?? Environment.GetEnvironmentVariable("JWT__Issuer") ?? "IssuerPorDefecto";
var audience = jwtSettings["Audience"] ?? Environment.GetEnvironmentVariable("JWT__Audience") ?? "AudiencePorDefecto";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Configuraci�n de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Inyecciones de dependencias
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<UsuariosRepository>();
builder.Services.AddScoped<UsuariosService>();
builder.Services.AddScoped<ErrorLogRepository>();
builder.Services.AddScoped<ErrorLogService>();
builder.Services.AddHostedService<TokenCleanupService>();
builder.Services.AddScoped<UserTokenService>();
builder.Services.AddScoped<IPerfilRepository, PerfilRepository>();
builder.Services.AddScoped<IPerfilService, PerfilService>();
builder.Services.AddScoped<CloudinaryService>();
builder.Services.AddScoped<PresupuestoRepository>();
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<GastoRepository>();
builder.Services.AddScoped<EtiquetaRepository>();
builder.Services.AddScoped<PresupuestoService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<GastoService>();
builder.Services.AddScoped<EtiquetaService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<RolesRepository>();
builder.Services.AddScoped<RolesService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowAllOrigins");

app.UseMiddleware<SpendWise.Middlewares.ErrorLoggingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();