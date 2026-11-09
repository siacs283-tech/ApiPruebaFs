namespace ApiPruebaFs.Models
{
    public class OrdenDetalle
    {
        public int CodDetalleOrden { get; set; }

        public int CodOrden { get; set; }

        public int CodProducto { get; set; }

        public string NombreProducto { get; set; } = string.Empty;

        public decimal PrecioUnitario { get; set; }

        public int Cantidad { get; set; }

        public decimal Subtotal { get; set; }
    }
}
