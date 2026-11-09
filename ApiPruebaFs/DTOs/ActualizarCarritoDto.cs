using System.ComponentModel.DataAnnotations;

namespace ApiPruebaFs.DTOs
{
    public class ActualizarCarritoDto
    {
        [Required]
        //[Range(1, int.MinValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public int Cantidad { get; set; }
    }
}
