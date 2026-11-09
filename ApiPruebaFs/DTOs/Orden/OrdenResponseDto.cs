namespace ApiPruebaFs.DTOs.Orden
{
    public class OrdenResponseDto
    {
        public int CodOrden { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Descuento { get; set; }

        public decimal Total { get; set; }

        public string Mensaje { get; set; } = string.Empty;
    }
}
