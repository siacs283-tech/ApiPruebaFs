using ApiPruebaFs.Models;

namespace ApiPruebaFs.Repositories
{
    public interface IProductoRepository
    {
        Task<(IEnumerable<Producto> Items, int TotalRegistros)>ListarProducto(string rucempresa, int pagina);
        Task<(IEnumerable<Producto> Items, int TotalRegistros)>BuscarProducto(string rucempresa, string buscar, int pagina);
        Task<Producto> ObtenerProductoPorId(int codproducto, string rucempresa);
    }
}
