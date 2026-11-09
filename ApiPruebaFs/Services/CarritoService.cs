using ApiPruebaFs.DTOs;
using ApiPruebaFs.Models;
using ApiPruebaFs.Repositories;

namespace ApiPruebaFs.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly ICarritoRepository _carritoRepository;

        public CarritoService(ICarritoRepository carritoRepository)
        {
            _carritoRepository = carritoRepository;
        }


        public async Task<IEnumerable<CarritoDto>> ObtenerCarrito(
            string usuario,
            string rucempresa)
        {
            var carrito = await _carritoRepository.ObtenerCarrito(
                usuario,
                rucempresa);

            return carrito.Select(p => new CarritoDto
            {
                Codpedito = p.Codpedido,
                Codpedidodet = p.Codpedidodet,
                CodProducto = p.CodProducto,
                NombreProducto = p.NombreProducto,
                Categoria = p.Categoria,
                PrecioUnitario = p.PrecioUnitario,
                Stock = p.Stock,
                Cantidad = p.Cantidad,
                Subtotal = p.Subtotal
            });
        }


        public async Task AgregarCarrito(
            string usuario,
            string rucempresa,
            AgregarCarritoDto dto)
        {
            if (dto.CodProducto <= 0)
                throw new ArgumentException("El código del producto no es válido.");

            if (dto.Cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor que cero.");

            await _carritoRepository.AgregarCarrito(
                usuario,
                rucempresa,
                dto.CodProducto,
                dto.Cantidad);
        }


        public async Task ActualizarCarrito(
            string usuario,
            string rucempresa,
            int codproducto,
            ActualizarCarritoDto dto)
        {
            if (codproducto <= 0)
                throw new ArgumentException("El código del producto no es válido.");

            if (dto.Cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor que cero.");

            await _carritoRepository.ActualizarCarrito(
                usuario,
                rucempresa,
                codproducto,
                dto.Cantidad);
        }


        public async Task EliminarProductoCarrito(
            string usuario,
            string rucempresa,
            int codproducto)
        {
            if (codproducto <= 0)
                throw new ArgumentException("El código del producto no es válido.");

            await _carritoRepository.EliminarProductoCarrito(
                usuario,
                rucempresa,
                codproducto);
        }


        public async Task VaciarCarrito(
            string usuario,
            string rucempresa)
        {
            await _carritoRepository.VaciarCarrito(
                usuario,
                rucempresa);
        }

    }
}
