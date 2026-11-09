using ApiPruebaFs.DTOs.Orden;
using ApiPruebaFs.Repositories;

namespace ApiPruebaFs.Services
{
    public class OrdenService : IOrdenService
    {
        private readonly IOrdenRepository _ordenRepository;

        public OrdenService(IOrdenRepository ordenRepository)
        {
            _ordenRepository = ordenRepository;
        }




        public async Task<OrdenResponseDto> CrearOrden(
          string usuario,
          string rucempresa)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("El usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(rucempresa))
                throw new ArgumentException("La empresa es obligatoria.");

            return await _ordenRepository.CrearOrden(
                usuario,
                rucempresa
            );
        }


        public async Task<IEnumerable<OrdenDto>> ListarOrden(
            string usuario,
            string rucempresa)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("El usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(rucempresa))
                throw new ArgumentException("La empresa es obligatoria.");

            return await _ordenRepository.ListarOrden(
                usuario,
                rucempresa
            );
        }



        public async Task<OrdenDto?> ObtenerOrdenId(
            int codorden,
            string usuario,
            string rucempresa)
        {
            if (codorden <= 0)
                throw new ArgumentException("El código de orden no es válido.");

            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("El usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(rucempresa))
                throw new ArgumentException("La empresa es obligatoria.");

            return await _ordenRepository.ObtenerOrdenId(
                codorden,
                usuario,
                rucempresa
            );
        }


    }
}
