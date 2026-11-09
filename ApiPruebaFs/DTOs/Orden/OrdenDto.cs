namespace ApiPruebaFs.DTOs.Orden
{
    public class OrdenDto
    {
        public int CodOrden { get; set; }

        public DateTime FechaOrden { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Descuento { get; set; }

        public decimal Total { get; set; }

        public string Estado { get; set; } = string.Empty;

        public List<OrdenDetalleDto> Detalles { get; set; } = new();
    }
}
