using ApiPruebaFs.Repositories;
using ApiPruebaFs.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiPruebaFs.Repositories
{
    public class CarritoRepository : ICarritoRepository
    {
        private readonly string _connectionString;

        public CarritoRepository(IConfiguration configuration)
        {
            _connectionString = configuration
                .GetConnectionString("MySqlConnection")!;
        }


      
        public async Task<IEnumerable<Carrito>> ObtenerCarrito(
            string usuario,
            string rucempresa)
        {
            using var connection = new SqlConnection(_connectionString);

            var parametros = new
            {
                usuario,
                rucempresa
            };

            var resultado = await connection.QueryAsync<Carrito>(
                "SP_Obtener_Pedido",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            return resultado;
        }

        public async Task AgregarCarrito(
            string usuario,
            string rucempresa,
            int codproducto,
            int cantidad)
        {
            using var connection = new SqlConnection(_connectionString);

            var parametros = new
            {
                usuario,
                rucempresa,
                codproducto,
                cantidad
            };

            await connection.ExecuteAsync(
                "SP_Agregar_Pedido",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task ActualizarCarrito(
            string usuario,
            string rucempresa,
            int codproducto,
            int cantidad)
        {
            using var connection = new SqlConnection(_connectionString);

            var parametros = new
            {
                usuario,
                rucempresa,
                codproducto,
                cantidad
            };

            await connection.ExecuteAsync(
                "SP_Actualizar_Pedido",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task EliminarProductoCarrito(
            string usuario,
            string rucempresa,
            int codproducto)
        {
            using var connection = new SqlConnection(_connectionString);

            var parametros = new
            {
                usuario,
                rucempresa,
                codproducto
            };

            await connection.ExecuteAsync(
                "SP_Emilinar_Producto_Pedido",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task VaciarCarrito(
            string usuario,
            string rucempresa)
        {
            using var connection = new SqlConnection(_connectionString);

            var parametros = new
            {
                usuario,
                rucempresa
            };

            await connection.ExecuteAsync(
                "SP_Vaciar_Pedido",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
