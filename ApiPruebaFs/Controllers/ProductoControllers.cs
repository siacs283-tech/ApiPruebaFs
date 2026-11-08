using ApiPruebaFs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiPruebaFs.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _service;

        public ProductosController(IProductoService service)
        {
            _service = service;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> Listar(
            [FromQuery] int pagina = 1,
            [FromQuery] int registros = 10,
            [FromQuery] string buscar = null)
        {
            var rucempresa = User.FindFirst("rucempresa")?.Value;

            if (string.IsNullOrWhiteSpace(rucempresa))
                return Unauthorized(new
                {
                    mensaje = "No se encontró la empresa del usuario."
                });

            var resultado = string.IsNullOrWhiteSpace(buscar)
                ? await _service.ListarAsync(rucempresa, pagina, registros)
                : await _service.BuscarAsync(rucempresa, buscar, pagina, registros);

            return Ok(resultado);
        }

        // GET: api/products/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var rucempresa = User.FindFirst("rucempresa")?.Value;

            if (string.IsNullOrWhiteSpace(rucempresa))
                return Unauthorized();

            var producto = await _service.ObtenerPorIdAsync(id, rucempresa);

            if (producto == null)
                return NotFound(new
                {
                    mensaje = "Producto no encontrado."
                });

            return Ok(producto);
        }
    }
}