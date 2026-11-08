namespace ApiPruebaFs.Models
{
    public class Producto
    {
        public int CodProducto { get; set; }
        public string Categoria { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Stock { get; set; }
        public string RucEmpresa { get; set; }
    }
}
