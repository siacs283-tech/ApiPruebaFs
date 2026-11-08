using ApiPruebaFs.Models;

namespace ApiPruebaFs.Services
{
    public interface IJwtTokenService
    {
        TokenResult GenerateTokens(Usuario usuario);
        string HashRefreshToken(string refreshToken);
    }

    public class TokenResult
    {
        public string token { get; set; } = string.Empty;
        public string refreshToken { get; set; } = string.Empty;
        public string refreshTokenHash { get; set; } = string.Empty;
        public DateTime expiresAt { get; set; }
        public DateTime refreshTokenExpiresAt { get; set; }
    }
}