namespace SpendWise.DTOs
{
    public class PerfilDTO
    {
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public IFormFile Foto { get; set; }
    }
}
