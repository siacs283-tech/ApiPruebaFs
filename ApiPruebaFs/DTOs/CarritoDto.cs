namespace ApiPruebaFs.DTOs
{
    public class CarritoDto
    {
        public int Codpedito { get; set; }

        public int Codpedidodet { get; set; }

        public int CodProducto { get; set; }

        public string NombreProducto { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public decimal PrecioUnitario { get; set; }

        public int Stock { get; set; }

        public int Cantidad { get; set; }

        public decimal Subtotal { get; set; }
    }
}
