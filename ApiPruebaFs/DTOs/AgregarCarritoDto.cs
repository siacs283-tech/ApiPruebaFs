using System.ComponentModel.DataAnnotations;
namespace ApiPruebaFs.DTOs
{
    public class AgregarCarritoDto
    {
        [Required(ErrorMessage = "El código del producto es obligatorio.")]
        //[Range(1, int.MaxValue, ErrorMessage = "El código del producto debe ser válido.")]
        public int CodProducto { get; set; }

        [Required]
        //[Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public int Cantidad { get; set; }
    }
}
