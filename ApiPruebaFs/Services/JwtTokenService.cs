using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ApiPruebaFs.Configuration;
using ApiPruebaFs.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ApiPruebaFs.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(IOptions<JwtOptions> jwtOptions, ILogger<JwtTokenService> logger)
        {
            _jwtOptions = jwtOptions.Value;
            _logger = logger;
        }

        public TokenResult GenerateTokens(Usuario usuario)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
            if (keyBytes.Length < 32)
            {
                throw new InvalidOperationException("Jwt:SecretKey debe tener al menos 32 caracteres.");
            }

            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenMinutes);
            var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenDays);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuario.usuario),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, usuario.usuario),
                new(ClaimTypes.Name, usuario.nombre ?? string.Empty),
                new("nivel", usuario.nivel.ToString()),
                new("idsucursal", usuario.idsucursal.ToString()),
                new("idcaja", usuario.idcaja.ToString()),
                new("rucempresa", usuario.rucempresa ?? string.Empty)
            };

            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            var refreshToken = GenerateRefreshToken();

            _logger.LogInformation("Tokens generados para usuario {Usuario}. JWT expira {ExpiresAt:u}; refresh token expira {RefreshTokenExpiresAt:u}.", usuario.usuario, expiresAt, refreshTokenExpiresAt);

            return new TokenResult
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = refreshToken,
                refreshTokenHash = HashRefreshToken(refreshToken),
                expiresAt = expiresAt,
                refreshTokenExpiresAt = refreshTokenExpiresAt
            };
        }

        public string HashRefreshToken(string refreshToken)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
            return Convert.ToHexString(bytes);
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
