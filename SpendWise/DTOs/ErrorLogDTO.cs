namespace SpendWise.DTOs
{
    public class ErrorLogDTO
    {
        public int Id { get; set; }
        public string Mensaje_error { get; set; }
        public string Enlace_error { get; set; }
        public DateTime Fecha_error { get; set; }
    }
}