using ApiPruebaFs.Models;
using ApiPruebaFs.Repositories;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiPruebaFs.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly string _connectionString;

        public ProductoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection")!;
        }

        public async Task<(IEnumerable<Producto> Items, int TotalRegistros)>
            ListarProducto(string rucempresa, int pagina)
        {
            using var connection = new SqlConnection(_connectionString);

            var parametros = new
            {
                rucempresa,
                pagina
                
            };

            var resultado = await connection.QueryAsync<ProductoPaginado>(
                "SP_Listado_Producto",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            var totalRegistros = resultado.FirstOrDefault()?.TotalRegistros ?? 0;

            return (resultado.Cast<Producto>(), totalRegistros);
        }

        public async Task<(IEnumerable<Producto> Items, int TotalRegistros)>BuscarProducto(string rucempresa, string buscar, int pagina)
        {
            using var connection = new SqlConnection(_connectionString);

            var parametros = new
            {
                rucempresa,
                buscar,
                pagina
                
            };

            var resultado = await connection.QueryAsync<ProductoPaginado>(
                "SP_Buscar_Producto",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            var totalRegistros = resultado.FirstOrDefault()?.TotalRegistros ?? 0;

            return (resultado.Cast<Producto>(), totalRegistros);
        }

        public async Task<Producto> ObtenerProductoPorId(
            int codproducto, string rucempresa)
        {
            using var connection = new SqlConnection(_connectionString);

            var parametros = new
            {
                codproducto,
                rucempresa
            };

            return await connection.QueryFirstOrDefaultAsync<Producto>(
                "SP_Obtener_Producto_Id",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        private class ProductoPaginado : Producto
        {
            public int TotalRegistros { get; set; }
        }
    }
}