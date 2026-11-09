ApiPruebaFs 
API REST desarrollada con .NET 8, SQL Server y Dapper
1. Tecnologías utilizadas
•	C# / .NET 8
•	ASP.NET Core Web API
•	SQL Server
•	Dapper
•	JWT (JSON Web Token)
•	Swagger / OpenAPI
2. Requisitos previos
Antes de ejecutar el proyecto, debes tener instalado:
•	.NET 8 SDK
•	SQL Server (local)
•	SQL Server Management Studio (SSMS)
•	Visual Studio 2022 o Visual Studio 2026
3. Configuración de la base de datos
1.	Abrir SQL Server Management Studio.
2.	Conectarse a tu instancia local de SQL Server.
3.	Crear una base de datos llamada pruebafs.
4.	Abrir el archivo PruebaFs.sql.
5.	Ejecutar el script para crear las tablas, relaciones, índices, procedimientos almacenados y datos de prueba.
4. Configurar la conexión a SQL Server
Abrir el archivo appsettings.json y configurar la cadena de conexión.
Ejemplo utilizando autenticación de Windows:
{
  "ConnectionStrings": {
"MySqlConnection": "server=tu_servidor;database=pruebafs;user id=tu_usuario_sql;password=tu_clave;TrustServerCertificate=True;"  }
}
Reemplazar los valores según la configuración de tu SQL Server.
5. Ejecutar el proyecto
Abrir una terminal en la carpeta donde se encuentra el archivo .csproj.
Ejecutar:
dotnet restore
Después:
dotnet build
Finalmente:
dotnet run
6. Acceder a Swagger
Una vez iniciada la API, abrir en el navegador la dirección que aparece en la consola.
Ejemplo:
http://localhost:5000/swagger
Los puertos pueden variar según la configuración del proyecto.
7. Autenticación
Para probar los endpoints protegidos:
1.	Ejecutar el endpoint de login.
2.	Obtener el JWT.
3.	Presionar el botón Authorize en Swagger.
4.	Ingresar el token según el esquema configurado en Swagger.
5.	Probar los endpoints autorizados.
8. Endpoints principales
Autenticación
Método	Endpoint	Descripción
POST	/api/auth/login	Iniciar sesión
Productos
Método	Endpoint	Descripción
GET	/api/products	Listar productos
GET	/api/products/{id}	Consultar producto
Carrito
Método	Endpoint	Descripción
GET	/api/cart	Consultar carrito
POST	/api/cart/items	Agregar producto
PUT	/api/cart/items/{productId}	Actualizar cantidad
DELETE	/api/cart/items/{productId}	Eliminar producto
DELETE	/api/cart	Vaciar carrito
Órdenes
Método	Endpoint	Descripción
POST	/api/orders	Crear orden
GET	/api/orders	Listar órdenes
GET	/api/orders/{id}	Consultar detalle de orden
9. Reglas de negocio
•	El carrito se mantiene asociado al usuario autenticado.
•	El inventario se valida al agregar productos y al confirmar la compra.
•	El precio se obtiene directamente desde SQL Server.
•	Se aplica un descuento del 10 % cuando el subtotal supera los $100.
•	La creación de órdenes, el descuento de inventario y el vaciado del carrito se realizan dentro de una transacción SQL.
•	Las órdenes conservan el nombre y precio de los productos al momento de la compra.

Proyecto: ApiPruebaFs
Framework: .NET 8
Base de datos: SQL Server

