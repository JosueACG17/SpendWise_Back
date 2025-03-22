namespace SpendWise.DTOs
{
    public class UsuarioResponseDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int RolId { get; set; }
        public string RolNombre { get; set; }
    }
}