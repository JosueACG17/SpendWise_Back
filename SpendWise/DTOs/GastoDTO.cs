namespace SpendWise.DTOs
{
    public class GastoDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public decimal Monto { get; set; }
        public int CategoriaId { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
    }
}
