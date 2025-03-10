using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SpendWise.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Perfil Perfil { get; set; }
        public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
        public ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>();
        public ICollection<Etiqueta> Etiquetas { get; set; } = new List<Etiqueta>();
    }
}
