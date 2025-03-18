using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SpendWise.Models
{
    public class Gasto
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        //[Precision(18, 2)]
        public decimal Monto { get; set; }
        
        public int CategoriaId { get; set; }
        public DateTime Fecha { get; set; }
        public string? Descripcion { get; set; }
        //public Usuario? Usuario { get; set; }
        //public Categoria? Categoria { get; set; }
        //public ICollection<Etiqueta>? Etiquetas { get; set; } = new List<Etiqueta>();
    }
}
