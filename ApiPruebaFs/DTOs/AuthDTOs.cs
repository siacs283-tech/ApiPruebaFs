
namespace ApiPruebaFs.DTOs
{
    public class LoginRequest
    {
        public string usuario { get; set; } = string.Empty;
        public string clave { get; set; } = string.Empty;

    }

    public class RefreshTokenRequest
    {
        public string usuario { get; set; } = string.Empty;
        public string refreshToken { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string token { get; set; } = string.Empty;
        public string refreshToken { get; set; } = string.Empty;
        public DateTime expiresAt { get; set; }
        public DateTime refreshTokenExpiresAt { get; set; }

        public UsuarioSesionDTO usuario { get; set; } = new();
    }

    public class UsuarioSesionDTO
    {
        public string usuario { get; set; } = string.Empty;
        public string rucempresa { get; set; } = string.Empty;

    }
}

