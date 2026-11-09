namespace ApiPruebaFs.Models
{
    public class Orden
    {
        public int CodOrden { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public string RucEmpresa { get; set; } = string.Empty;

        public DateTime FechaOrden { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Descuento { get; set; }

        public decimal Total { get; set; }

        public string Estado { get; set; } = string.Empty;

        public List<OrdenDetalle> Detalles { get; set; } = new();
    }
}
