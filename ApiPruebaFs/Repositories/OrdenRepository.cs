using ApiPruebaFs.DTOs.Orden;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiPruebaFs.Repositories
{
    public class OrdenRepository : IOrdenRepository
    {
        private readonly string _connectionString;

        public OrdenRepository(IConfiguration configuration)
        {
            _connectionString = configuration
               .GetConnectionString("MySqlConnection")!;
        }

        public async Task<OrdenResponseDto> CrearOrden(
            string usuario,
            string rucempresa)
        {
            await using var connection =
                new SqlConnection(_connectionString);

            var parametros = new DynamicParameters();

            parametros.Add("@usuario", usuario);
            parametros.Add("@rucempresa", rucempresa);

            var resultado = await connection.QuerySingleAsync<OrdenResponseDto>(
                "SP_CREAR_ORDEN",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            return resultado;
        }

        public async Task<IEnumerable<OrdenDto>> ListarOrden(
          string usuario,
          string rucempresa)
        {
            await using var connection =
                new SqlConnection(_connectionString);

            var parametros = new DynamicParameters();

            parametros.Add("@usuario", usuario);
            parametros.Add("@rucempresa", rucempresa);

            var resultado = await connection.QueryAsync<OrdenDto>(
                "SP_LISTAR_ORDEN",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            return resultado;
        }



        public async Task<OrdenDto?> ObtenerOrdenId(
            int codorden,
            string usuario,
            string rucempresa)
        {
            await using var connection =
                new SqlConnection(_connectionString);

            var parametros = new DynamicParameters();

            parametros.Add("@codorden", codorden);
            parametros.Add("@usuario", usuario);
            parametros.Add("@rucempresa", rucempresa);

            using var resultado = await connection.QueryMultipleAsync(
                "SP_OBTENER_ORDEN_ID",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            var orden = await resultado.ReadSingleOrDefaultAsync<OrdenDto>();

            if (orden == null)
                return null;

            var detalles = await resultado.ReadAsync<OrdenDetalleDto>();

            orden.Detalles = detalles.ToList();

            return orden;
        }

    }
}
