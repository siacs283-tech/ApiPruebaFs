using ApiPruebaFs.DTOs;
using ApiPruebaFs.Models;
using ApiPruebaFs.Repositories;


namespace ApiPruebaFs.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(IUsuarioRepository repository, IJwtTokenService jwtTokenService, ILogger<UsuarioService> logger)
        {
            _repository = repository;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        public async Task<AuthResponse?> Login(LoginRequest request)
        {
            var usuarioLogin = request.usuario.Trim();


            var usuario = await _repository.ObtenerUsuarioPorCredenciales(usuarioLogin);

            if (usuario is null)
            {

                return null;
            }
            else
            {
                if (usuario.clave != request.clave)
                {
                    return null;
                }
            }

            var response = await CrearSesion(usuario);
            _logger.LogInformation("Sesion creada para usuario {Usuario}. JWT expira {ExpiresAt:u}; refresh token expira {RefreshTokenExpiresAt:u}.", usuario.usuario, response.expiresAt, response.refreshTokenExpiresAt);

            return response;
        }

        public async Task<AuthResponse?> RefreshToken(RefreshTokenRequest request)
        {
            var usuarioLogin = request.usuario.Trim();
            _logger.LogInformation("Validando refresh token para usuario {Usuario}.", usuarioLogin);

            var refreshTokenHash = _jwtTokenService.HashRefreshToken(request.refreshToken);
            var usuario = await _repository.ObtenerUsuarioPorRefreshToken(usuarioLogin, refreshTokenHash);

            if (usuario is null)
            {
                _logger.LogWarning("Refresh token rechazado para usuario {Usuario}.", usuarioLogin);
                return null;
            }

            var tokens = _jwtTokenService.GenerateTokens(usuario);
            await _repository.RevocarRefreshToken(refreshTokenHash, tokens.refreshTokenHash);
            await _repository.GuardarRefreshToken(usuario.usuario, usuario.rucempresa, tokens.refreshTokenHash, tokens.refreshTokenExpiresAt);

            _logger.LogInformation("Refresh token rotado para usuario {Usuario}. Nuevo JWT expira {ExpiresAt:u}.", usuario.usuario, tokens.expiresAt);
            return CrearRespuesta(usuario, tokens);
        }

        private async Task<AuthResponse> CrearSesion(Usuario usuario)
        {
            var tokens = _jwtTokenService.GenerateTokens(usuario);
            await _repository.GuardarRefreshToken(usuario.usuario, usuario.rucempresa, tokens.refreshTokenHash, tokens.refreshTokenExpiresAt);

            return CrearRespuesta(usuario, tokens);
        }

        private static AuthResponse CrearRespuesta(Usuario usuario, TokenResult tokens)
        {
            return new AuthResponse
            {
                token = tokens.token,
                refreshToken = tokens.refreshToken,
                expiresAt = tokens.expiresAt,
                refreshTokenExpiresAt = tokens.refreshTokenExpiresAt,
                usuario = new UsuarioSesionDTO
                {
                    usuario = usuario.usuario,
                    nombre = usuario.nombre,
                    nivel = usuario.nivel,
                    idsucursal = usuario.idsucursal,
                    nombresucursal = usuario.nombresucursal,
                    idcaja = usuario.idcaja,
                    nombrecaja = usuario.nombrecaja,
                    estado = usuario.estado,
                    rucempresa = usuario.rucempresa,
                    estadopago = usuario.estadopago
                }
            };
        }
    }
}

