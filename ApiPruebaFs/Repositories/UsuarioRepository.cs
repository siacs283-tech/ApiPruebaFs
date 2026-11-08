using ApiPruebaFs.Models;
using ApiPruebaFs.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ApiPruebaFs.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<UsuarioRepository> _logger;

        public UsuarioRepository(IConfiguration configuration, ILogger<UsuarioRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection")!;
            _logger = logger;
        }

        public async Task<Usuario> ObtenerUsuarioPorCredenciales(string usuario)
        {
            try
            {
                var lista = new Usuario();

                using var conection = new SqlConnection(_connectionString);

                using var cmd = new SqlCommand("ObtenerUsuarioPorId", conection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idusu", usuario);


                await conection.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    lista.clave = reader["clave"].ToString();                  
                    lista.rucempresa = reader["rucempresa"].ToString();                    
                    lista.usuario = reader["usuario"].ToString();         
                }

                return lista;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar usuario por credenciales. Usuario: {Usuario}.", usuario);
                throw;
            }
        }

        public async Task<Usuario?> ObtenerUsuarioPorRefreshToken(string usuario, string refreshTokenHash)
        {
            try
            {
                _logger.LogInformation("Consultando usuario por refresh token vigente. Usuario: {Usuario}.", usuario);

                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand(@"
                    SELECT u.usuario, u.clave, u.nombre, u.nivel, u.idsucursal, u.nombresucursal, u.estado, u.idcaja, u.nombrecaja, u.rucempresa, u.estadopago
                    FROM usuario_refresh_tokens rt
                    INNER JOIN usuarios u
                        ON u.usuario = rt.usuario
                       AND u.rucempresa = rt.rucempresa
                    WHERE rt.usuario = @usuario
                      AND rt.token_hash = @token_hash
                      AND rt.revoked_at IS NULL
                      AND rt.expires_at > @fecha_actual
                      AND u.estado = 1
                    LIMIT 1;", connection);

                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@usuario", usuario);
                command.Parameters.AddWithValue("@token_hash", refreshTokenHash);
                command.Parameters.AddWithValue("@fecha_actual", DateTime.UtcNow);

                await connection.OpenAsync();
                await using var reader = await command.ExecuteReaderAsync();

                if (!await reader.ReadAsync())
                {
                    _logger.LogWarning("No existe refresh token vigente para usuario {Usuario}.", usuario);
                    return null;
                }

                var result = MapUsuario(reader);
                _logger.LogInformation("Refresh token vigente validado en base de datos para usuario {Usuario} y rucempresa {RucEmpresa}.", result.usuario, result.rucempresa);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar usuario por refresh token. Usuario: {Usuario}.", usuario);
                throw;
            }
        }

        public async Task GuardarRefreshToken(string usuario, string rucempresa, string refreshTokenHash, DateTime expiresAt)
        {
            try
            {
            
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                await using var transaction = (SqlTransaction) await connection.BeginTransactionAsync();

                try
                {
                    await using (var revokeCommand = new SqlCommand(@"
                        UPDATE usuario_refresh_tokens
                        SET revoked_at = @revoked_at
                        WHERE usuario = @usuario
                          AND rucempresa = @rucempresa
                          AND revoked_at IS NULL;", connection,transaction))
                    {
                        revokeCommand.CommandType = CommandType.Text;
                        revokeCommand.Parameters.AddWithValue("@revoked_at", DateTime.UtcNow);
                        revokeCommand.Parameters.AddWithValue("@usuario", usuario);
                        revokeCommand.Parameters.AddWithValue("@rucempresa", rucempresa);
                        await revokeCommand.ExecuteNonQueryAsync();
                    }

                    await using (var insertCommand = new SqlCommand(@"
                        INSERT INTO usuario_refresh_tokens (usuario, rucempresa, token_hash, expires_at)
                        VALUES (@usuario, @rucempresa, @token_hash, @expires_at);", connection, transaction))
                    {
                        insertCommand.CommandType = CommandType.Text;
                        insertCommand.Parameters.AddWithValue("@usuario", usuario);
                        insertCommand.Parameters.AddWithValue("@rucempresa", rucempresa);
                        insertCommand.Parameters.AddWithValue("@token_hash", refreshTokenHash);
                        insertCommand.Parameters.AddWithValue("@expires_at", expiresAt);
                        await insertCommand.ExecuteNonQueryAsync();
                    }

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar refresh token para usuario {Usuario} y rucempresa {RucEmpresa}.", usuario, rucempresa);
                throw;
            }
        }

        public async Task RevocarRefreshToken(string refreshTokenHash, string? replacedByTokenHash = null)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await using var command = new SqlCommand(@"
                    UPDATE usuario_refresh_tokens
                    SET revoked_at = COALESCE(revoked_at, @revoked_at),
                        replaced_by_token_hash = @replaced_by_token_hash
                    WHERE token_hash = @token_hash;", connection);

                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@revoked_at", DateTime.UtcNow);
                command.Parameters.AddWithValue("@replaced_by_token_hash", string.IsNullOrWhiteSpace(replacedByTokenHash) ? DBNull.Value : replacedByTokenHash);
                command.Parameters.AddWithValue("@token_hash", refreshTokenHash);

                await connection.OpenAsync();
                var rows = await command.ExecuteNonQueryAsync();

                _logger.LogInformation("Refresh token revocado. Registros afectados: {Rows}.", rows);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al revocar refresh token.");
                throw;
            }
        }

        private static Usuario MapUsuario(IDataRecord reader)
        {
            return new Usuario
            {
                usuario = GetString(reader, "usuario"),
                clave = GetString(reader, "clave"),               
                rucempresa = GetString(reader, "rucempresa")
              
            };
        }

        private static string GetString(IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            return value == DBNull.Value ? string.Empty : value.ToString() ?? string.Empty;
        }

        private static int GetInt(IDataRecord reader, string columnName)
        {
            var value = reader[columnName];
            return value == DBNull.Value ? 0 : Convert.ToInt32(value);
        }
    }
}
