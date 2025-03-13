using System.ComponentModel.DataAnnotations;

namespace SpendWise.Models
{
    public class Perfil
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public Usuario Usuario { get; set; }
    }
}
