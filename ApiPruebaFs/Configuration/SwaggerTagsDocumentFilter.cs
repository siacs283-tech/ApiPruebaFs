
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiPruebaFs.Configuration
{
    public class SwaggerTagsDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new List<OpenApiTag>
            {
                new() { Name = "Autenticacion", Description = "Inicio de sesion, refresh token y validacion del JWT." },     
                new() { Name = "Productos", Description = "Consultas de productos." },              
                new() { Name = "Usuarios", Description = "Operaciones de usuarios." }
            };
        }
    }
}
