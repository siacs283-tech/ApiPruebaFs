using ApiPruebaFs.DTOs.Orden;

namespace ApiPruebaFs.Services
{
    public interface IOrdenService
    {
        Task<OrdenResponseDto> CrearOrden(
          string usuario,
          string rucempresa);

        Task<IEnumerable<OrdenDto>> ListarOrden(
            string usuario,
            string rucempresa);

        Task<OrdenDto?> ObtenerOrdenId(
            int codorden,
            string usuario,
            string rucempresa);
    }
}
