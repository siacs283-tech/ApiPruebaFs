namespace ApiPruebaFs.DTOs.Orden
{
    public class OrdenDetalleDto
    {
        public int CodProducto { get; set; }

        public string NombreProducto { get; set; } = string.Empty;

        public decimal PrecioUnitario { get; set; }

        public int Cantidad { get; set; }

        public decimal Subtotal { get; set; }
    }
}
