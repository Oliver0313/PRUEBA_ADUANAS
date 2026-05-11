Tecnologías utilizadas
Backend
ASP.NET Core Web API
Entity Framework Core
SQL Server
JWT Authentication
Swagger

Frontend
Vue.js 3
JavaScript
HTML5
CSS3

-----

Funcionalidades
Autenticación
Inicio de sesión con JWT
Protección de endpoints mediante token
Validación de autorización

Gestión de Productos
Crear productos
Editar productos
Eliminar productos
Listar productos
Control de stock

Gestión de Clientes
Crear clientes
Editar clientes
Eliminar clientes
Listar clientes

Gestión de Ventas
Registrar ventas
Asociar productos y clientes
Consultar ventas

-----

Estructura del proyecto
Backend
Controllers/
Models/
Data/
DTOs/
Services/
Program.cs
appsettings.json

Frontend
views/
components/
router/
assets/
App.vue

-----

Configuración del proyecto
1. Clonar el repositorio
git clone https://github.com/usuario/repositorio.git

2. Configurar la base de datos
Abrir SQL Server.
Crear la base de datos.
Ejecutar los scripts SQL si existen.
Configurar la cadena de conexión en appsettings.json.

3. Ejecutar el backend

Desde Visual Studio o terminal:
dotnet restore
dotnet run

El backend se ejecutará normalmente en:
https://localhost:7226

Swagger:
https://localhost:7226/swagger

4. Ejecutar el frontend

Instalar dependencias:
npm install

Ejecutar proyecto:
npm run dev

El frontend se ejecutará normalmente en:
http://localhost:5173

Autenticación JWT
Para acceder a los endpoints protegidos:
Iniciar sesión desde el endpoint de autenticación.
Copiar el token JWT.
Utilizar el token en:
Authorization: Bearer TU_TOKEN

-----

Seguridad
Endpoints protegidos con JWT
Validación de tokens
Uso de HTTPS
Control de acceso mediante autorización

Desarrollado por Oliver Taveras.
