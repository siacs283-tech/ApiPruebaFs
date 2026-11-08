using ApiPruebaFs.Models;

namespace ApiPruebaFs.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ObtenerUsuarioPorCredenciales(string usuario);
        Task<Usuario?> ObtenerUsuarioPorRefreshToken(string usuario, string refreshTokenHash);
        Task GuardarRefreshToken(string usuario, string rucempresa, string refreshTokenHash, DateTime expiresAt);
        Task RevocarRefreshToken(string refreshTokenHash, string? replacedByTokenHash = null);


    }
}
