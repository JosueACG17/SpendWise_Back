using DotNetEnv; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CloudinaryDotNet;
using SpendWise.Services;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno desde el archivo .env
Env.Load();

// Configuraci�n de la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
        Environment.GetEnvironmentVariable("DefaultConnection")));

// Configuración de Cloudinary
var cloudinarySettings = builder.Configuration.GetSection("CloudinarySettings");
var account = new Account(
    cloudinarySettings["CloudName"],
    cloudinarySettings["ApiKey"],
    cloudinarySettings["ApiSecret"]
);
var cloudinary = new Cloudinary(account);
builder.Services.AddSingleton(cloudinary);

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();