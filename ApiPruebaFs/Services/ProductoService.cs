using ApiPruebaFs.DTOs;
using ApiPruebaFs.DTOs.GeneralDataCommon;
using ApiPruebaFs.Repositories;


namespace ApiPruebaFs.Services
{
    public class ProductoService : IProductoService
    {

        private readonly IProductoRepository _repository;
        public ProductoService(IProductoRepository repository)
        {
            _repository = repository;
        }

        public async Task<GeneralPaginacionResponseDto<ProductoDto>> ListarProducto(
            string rucempresa, int pagina)
        {
            pagina = pagina < 1 ? 1 : pagina;        

            var resultado = await _repository.ListarProducto(
                rucempresa, pagina);

            return CrearRespuesta(
                resultado.Items,
                resultado.TotalRegistros,
                pagina,12
                
            );
        }

        public async Task<GeneralPaginacionResponseDto<ProductoDto>> BuscarProducto(
            string rucempresa, string buscar, int pagina)
        {
            pagina = pagina < 1 ? 1 : pagina;
           
            buscar = buscar?.Trim() ?? string.Empty;

            var resultado = await _repository.BuscarProducto(
                rucempresa, buscar, pagina);

            return CrearRespuesta(
                resultado.Items,
                resultado.TotalRegistros,
                pagina,12
            );
        }

        public async Task<ProductoDto> ObtenerProductoPorId(int codproducto, string rucempresa)
        {
            if (codproducto <= 0)
                throw new ArgumentException("Código de producto inválido.");

            var producto = await _repository.ObtenerProductoPorId(
                codproducto, rucempresa);

            if (producto == null)
                return null;

            return new ProductoDto
            {
                CodProducto = producto.CodProducto,
                Categoria = producto.Categoria,
                NombreProducto = producto.NombreProducto,
                PrecioUnitario = producto.PrecioUnitario,
                Stock = producto.Stock
            };
        }

        private GeneralPaginacionResponseDto<ProductoDto> CrearRespuesta(
            IEnumerable<Models.Producto> productos,
            int totalRegistros,
            int pagina, int registros)
        {
            return new GeneralPaginacionResponseDto<ProductoDto>
            {
                Pagina = pagina,               
                TotalRegistros = totalRegistros,
                TotalPaginas = (int)Math.Ceiling(
                    totalRegistros / (double)registros),

                Items = productos.Select(p => new ProductoDto
                {
                    CodProducto = p.CodProducto,
                    Categoria = p.Categoria,
                    NombreProducto = p.NombreProducto,
                    PrecioUnitario = p.PrecioUnitario,
                    Stock = p.Stock
                }).ToList()
            };
        }
    }
}
