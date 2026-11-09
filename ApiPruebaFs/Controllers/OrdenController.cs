
using ApiPruebaFs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiPruebaFs.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrdenController : ControllerBase
    {
        private readonly IOrdenService _ordenService;

        public OrdenController(IOrdenService ordenService)
        {
            _ordenService = ordenService;
        }



        private bool ObtenerDatosUsuario(
            out string usuario,
            out string rucempresa)
        {
            usuario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? string.Empty;

            rucempresa = User.FindFirst("rucempresa")?.Value
                         ?? string.Empty;

            return !string.IsNullOrWhiteSpace(usuario)
                   && !string.IsNullOrWhiteSpace(rucempresa);
        }


        [HttpPost]
        public async Task<IActionResult> CrearOrden()
        {
            try { 
            if (!ObtenerDatosUsuario(out string usuario, out string rucempresa))
                return Unauthorized(new
                {
                    mensaje = "No se pudo identificar al usuario."
                });

            var resultado = await _ordenService.CrearOrden(
                usuario,
                rucempresa
            );
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"ERROR INTERNO: {ex.Message}");
          }

        }

       

        [HttpGet]
        public async Task<IActionResult> ListarOrden()
        {
            try { 
            if (!ObtenerDatosUsuario(out string usuario, out string rucempresa))
                return Unauthorized(new
                {
                    mensaje = "No se pudo identificar al usuario."
                });

            var resultado = await _ordenService.ListarOrden(
                usuario,
                rucempresa
            );

            return Ok(resultado);
        }
            catch (Exception ex)
            {
                return StatusCode(500, $"ERROR INTERNO: {ex.Message}");
    }
}



        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerOrdenId(int id)
        {
            try { 
            if (!ObtenerDatosUsuario(out string usuario, out string rucempresa))
                return Unauthorized(new
                {
                    mensaje = "No se pudo identificar al usuario."
                });

            var resultado = await _ordenService.ObtenerOrdenId(
                id,
                usuario,
                rucempresa
            );

            if (resultado == null)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró la orden solicitada."
                });
            }

            return Ok(resultado);
        }
            catch (Exception ex)
            {
                return StatusCode(500, $"ERROR INTERNO: {ex.Message}");
    }
}
    }
}
