using ApiPruebaFs.DTOs;
using ApiPruebaFs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiPruebaFs.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/cart")]
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService _carritoService;

        public CarritoController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerCarrito()
        {
            try
            {
                if (!ObtenerDatosUsuario(out string usuario, out string rucempresa))
                    return Unauthorized(new { mensaje = "No se encontraron los datos del usuario en el token." });

                var carrito = await _carritoService.ObtenerCarrito(
                    usuario,
                    rucempresa);

                return Ok(carrito);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"ERROR INTERNO: {ex.Message}");
            }
        }


        [HttpPost("items")]
        public async Task<IActionResult> AgregarProducto(
            [FromBody] AgregarCarritoDto dto)
        {
            try { 
                if (!ObtenerDatosUsuario(out string usuario, out string rucempresa))
                    return Unauthorized(new { mensaje = "No se encontraron los datos del usuario en el token." });

                await _carritoService.AgregarCarrito(
                    usuario,
                    rucempresa,
                    dto);

                return Ok(new
                {
                    mensaje = "Producto agregado correctamente."
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"ERROR INTERNO: {ex.Message}");
    }
}


        [HttpPut("items/{productId:int}")]
        public async Task<IActionResult> ActualizarProducto(
            int productId,
            [FromBody] ActualizarCarritoDto dto)
        {
            try { 
                if (!ObtenerDatosUsuario(out string usuario, out string rucempresa))
                    return Unauthorized(new { mensaje = "No se encontraron los datos del usuario en el token." });

                await _carritoService.ActualizarCarrito(
                    usuario,
                    rucempresa,
                    productId,
                    dto);

                return Ok(new
                {
                    mensaje = "Cantidad actualizada correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"ERROR INTERNO: {ex.Message}");
            }
        }


        [HttpDelete("items/{productId:int}")]
        public async Task<IActionResult> EliminarProductoCarrito(int productId)
        {
            try { 
                if (!ObtenerDatosUsuario(out string usuario, out string rucempresa))
                    return Unauthorized(new { mensaje = "No se encontraron los datos del usuario en el token." });

                await _carritoService.EliminarProductoCarrito(
                    usuario,
                    rucempresa,
                    productId);

                return Ok(new
                {
                    mensaje = "Producto eliminado correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"ERROR INTERNO: {ex.Message}");
            }
        }


        [HttpDelete]
        public async Task<IActionResult> VaciarCarrito()
        {
            try { 
                if (!ObtenerDatosUsuario(out string usuario, out string rucempresa))
                    return Unauthorized(new { mensaje = "No se encontraron los datos del usuario en el token." });

                await _carritoService.VaciarCarrito(
                    usuario,
                    rucempresa);

                return Ok(new
                {
                    mensaje = "Carrito vaciado correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"ERROR INTERNO: {ex.Message}");
            }
        }


        private bool ObtenerDatosUsuario(
            out string usuario,
            out string rucempresa)
        {

            usuario = User.FindFirst("usuario")?.Value
                      ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? string.Empty;

            rucempresa = User.FindFirst("rucempresa")?.Value
                         ?? string.Empty;

            return !string.IsNullOrWhiteSpace(usuario)
                   && !string.IsNullOrWhiteSpace(rucempresa);
        }
    }
}