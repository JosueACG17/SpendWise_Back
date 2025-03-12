using System.ComponentModel.DataAnnotations;

namespace SpendWise.Models
{
    public class ErrorLogs
    {
        [Key]
        public int Id { get; set; }
        public string Mensaje_error {  get; set; }
        public string Enlace_error { get; set; }
        public DateTime Fecha_error { get; set; }
    }
}
