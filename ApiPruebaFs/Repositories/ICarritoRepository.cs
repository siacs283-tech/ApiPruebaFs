using ApiPruebaFs.Models;
namespace ApiPruebaFs.Repositories
{
    public interface ICarritoRepository
    {
        Task<IEnumerable<Carrito>> ObtenerCarrito(
            string usuario,
            string rucempresa);

        Task AgregarCarrito(
            string usuario,
            string rucempresa,
            int codproducto,
            int cantidad);

        Task ActualizarCarrito(
            string usuario,
            string rucempresa,
            int codproducto,
            int cantidad);

        Task EliminarProductoCarrito(
            string usuario,
            string rucempresa,
            int codproducto);

        Task VaciarCarrito(
            string usuario,
            string rucempresa);
    }
}
