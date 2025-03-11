using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace SpendWise.Models
{
    public class Presupuesto
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int CategoriaId { get; set; }

        [Precision(18, 2)]
        public decimal Monto { get; set; }
        
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public Usuario Usuario { get; set; }
        public Categoria Categoria { get; set; }
    }
}
