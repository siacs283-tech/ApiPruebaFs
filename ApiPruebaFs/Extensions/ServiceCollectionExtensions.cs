
using System.Reflection;
using System.Text;
using ApiPruebaFs.Configuration;
using ApiPruebaFs.Interfaces;
using ApiPruebaFs.Repositories;
using ApiPruebaFs.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ApiPruebaFs.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiPruebaFsServices(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioService, UsuarioService>();

            services.AddScoped<IProductoRepository, ProductoRepository>();
            services.AddScoped<IProductoService, ProductoService>();

            services.AddScoped<ICarritoRepository, CarritoRepository>();
            services.AddScoped<ICarritoService, CarritoService>();

            services.AddScoped<IJwtTokenService, JwtTokenService>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("Jwt");
            services.Configure<JwtOptions>(jwtSection);

            var jwtOptions = jwtSection.Get<JwtOptions>()
                ?? throw new InvalidOperationException("La seccion Jwt no esta configurada.");

            var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);
            if (jwtKeyBytes.Length < 32)
            {
                throw new InvalidOperationException("Jwt:SecretKey debe tener al menos 32 caracteres.");
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(jwtKeyBytes),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ApiPruebaFs",
                    Version = "v1",
                    Description = "API Prueba Fernando Sosa"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingrese solo el token JWT. Swagger enviara el encabezado Authorization: Bearer {token}."
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
                options.DocumentFilter<SwaggerTagsDocumentFilter>();
                options.CustomSchemaIds(type => type.FullName?.Replace('+', '.') ?? type.Name);
                options.TagActionsBy(api => new[] { ObtenerGrupoSwagger(api.ActionDescriptor.RouteValues["controller"]) });
                options.OrderActionsBy(api => $"{ObtenerOrden(api.ActionDescriptor.RouteValues["controller"])}_{api.HttpMethod}_{api.RelativePath}");

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });

            return services;
        }

        public static WebApplication UseSwaggerDocumentation(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiPruebaFs v1");
                options.DocumentTitle = "ApiPruebaFs - Swagger";
                options.DocExpansion(DocExpansion.None);
                options.DisplayRequestDuration();
                options.EnablePersistAuthorization();
                options.EnableFilter();
            });

            return app;
        }

        private static string ObtenerGrupoSwagger(string? controller)
        {
            return controller switch
            {
                "Auth" => "Autenticacion",               
                "Productos" => "Productos",               
                "Carrito" => "Carrito",
                _ => controller ?? "General"
            };
        }

        private static int ObtenerOrden(string? controller)
        {
            return controller switch
            {
                "Auth" => 1,               
                "Productos" => 2,               
                "Carrito" => 3,                
                _ => 99
            };
        }
    }
}


