using ApiPruebaFs.DTOs;
using ApiPruebaFs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiPruebaFs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _service;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUsuarioService service, ILogger<AuthController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (request is null || string.IsNullOrWhiteSpace(request.usuario) || string.IsNullOrWhiteSpace(request.clave))
                { 
                    return BadRequest("Usuario y clave son obligatorios.");
                }

              
                var auth = await _service.Login(request);
                if (auth is null)
                {
                    return Unauthorized("Usuario o clave incorrectos");

                }

                return Ok(auth);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"ERROR INTERNO: {ex.Message}");
            }
        }

    }
}

