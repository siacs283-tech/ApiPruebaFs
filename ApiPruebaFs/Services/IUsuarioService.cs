using ApiPruebaFs.DTOs;


namespace ApiPruebaFs.Services
{
    public interface IUsuarioService
    {
        Task<AuthResponse?> Login(LoginRequest request);
        Task<AuthResponse?> RefreshToken(RefreshTokenRequest request);
    }
}
