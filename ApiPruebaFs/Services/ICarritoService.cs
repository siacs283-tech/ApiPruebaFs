using ApiPruebaFs.DTOs;

namespace ApiPruebaFs.Interfaces
{
    public interface ICarritoService
    {
        Task<IEnumerable<CarritoDto>> ObtenerCarrito(
            string usuario,
            string rucempresa);

        Task AgregarCarrito(
            string usuario,
            string rucempresa,
            AgregarCarritoDto dto);

        Task ActualizarCarrito(
            string usuario,
            string rucempresa,
            int codproducto,
            ActualizarCarritoDto dto);

        Task EliminarProductoCarrito(
            string usuario,
            string rucempresa,
            int codproducto);

        Task VaciarCarrito(
            string usuario,
            string rucempresa);
    }
}