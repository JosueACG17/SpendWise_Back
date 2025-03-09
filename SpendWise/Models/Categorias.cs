using System.ComponentModel.DataAnnotations;

namespace SpendWise.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
        public ICollection<Presupuesto> Presupuestos { get; set; } = new List<Presupuesto>();
    }
}
