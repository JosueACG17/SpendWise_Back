using System.ComponentModel.DataAnnotations;

namespace SpendWise.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        public ICollection<Usuario> Usuarios { get; set; }
    }
}
