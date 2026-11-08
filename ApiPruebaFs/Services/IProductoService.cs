using ApiPruebaFs.DTOs;
using ApiPruebaFs.DTOs.GeneralDataCommon;


namespace ApiPruebaFs.Services
{
    public interface IProductoService
    {
        Task<GeneralPaginacionResponseDto<ProductoDto>> ListarProducto(string rucempresa, int pagina);
        Task<GeneralPaginacionResponseDto<ProductoDto>> BuscarProducto(string rucempresa, string buscar, int pagina);
        Task<ProductoDto> ObtenerProductoPorId(int codproducto, string rucempresa);
    }
}
