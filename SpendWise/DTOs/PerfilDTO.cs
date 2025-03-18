using Microsoft.AspNetCore.Mvc;

namespace SpendWise.DTOs
{
    public class PerfilDTO
    {
        public int UsuarioId { get; set; }

        [FromForm]
        public string NombreCompleto { get; set; }

        [FromForm]
        public string Telefono { get; set; }

        [FromForm]
        public DateTime FechaNacimiento { get; set; }

        [FromForm]
        public string Genero { get; set; }

        [FromForm]
        public IFormFile Foto { get; set; }
    }
}
