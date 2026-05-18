# 🗃️ Inventario API

API RESTful para gestión de inventario construida con **.NET 8**, **Entity Framework Core** y **SQL Server**, protegida mediante autenticación **JWT**.

---

## 📐 Arquitectura

```
InventarioAPI/
├── Controllers/          # Capa de presentación – endpoints HTTP
│   ├── AuthController.cs
│   └── ProductosController.cs
├── Services/             # Capa de lógica de negocio
│   ├── IProductoService.cs / ProductoService.cs
│   └── IAuthService.cs  / AuthService.cs
├── Data/                 # Capa de acceso a datos (EF Core)
│   └── AppDbContext.cs
├── Models/               # Entidades de dominio
│   ├── Producto.cs
│   └── Usuario.cs
├── DTOs/                 # Objetos de transferencia de datos
│   ├── ProductoDto.cs
│   └── AuthDto.cs
├── Helpers/              # Middleware y utilidades transversales
│   └── ExceptionHandlerMiddleware.cs
├── Scripts/
│   └── init_db.sql       # Script SQL de inicialización
├── appsettings.json
└── Program.cs
```

---

## ⚙️ Requisitos previos

| Herramienta | Versión mínima |
|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download) | 8.0 |
| [SQL Server](https://www.microsoft.com/sql-server) | 2019 / Express / LocalDB |
| [EF Core Tools](https://learn.microsoft.com/ef/core/cli/dotnet) | 8.0 |

Instalar EF Core Tools (si no los tienes):
```bash
dotnet tool install --global dotnet-ef
```

---

## 🚀 Configuración y ejecución

### 1. Clonar el repositorio
```bash
git clone https://github.com/Joanclav/InvAPI_ManSystem.git
cd InventarioAPI
```

### 2. Configurar la cadena de conexión

Edita `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=Inventario;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

> Para SQL Server con autenticación por usuario/contraseña:
> `Server=localhost;Database=Inventario;User Id=sa;Password=Password;TrustServerCertificate=True;`

### 3. Configurar JWT

En `appsettings.json`, reemplaza la clave secreta:
```json
"JwtSettings": {
  "SecretKey": "CLAVE_SECRETA_MINIMO_32_CARACTERES",
  "Issuer": "InventarioAPI",
  "Audience": "InventarioAPIClient",
  "ExpirationHours": "8"
}
```

### 4. Aplicar migraciones (Code First)

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> **Alternativa:** ejecutar el script SQL directamente:
> ```bash
> sqlcmd -S localhost -i Scripts/init_db.sql
> ```

### 5. Ejecutar la API

```bash
dotnet run
```

La API estará disponible en:
- `http://localhost:7XXX`
- `http://localhost:5XXX`

---

## 📖 Pruebas y Exploración (Postman)

Navega a `http://www.postman.com/` y crea un nuevo espacio de trabajo. Para explorar, documentar y probar todos los endpoints de la API utilizaremos Postman.

---

## 🔐 Flujo de autenticación

### 1. Registrar un usuario
```http
POST http://localhost:5XXX/api/auth/register
Content-Type: application/json

{
  "nombreUsuario": "admin",
  "password": "Admin123!",
  "rol": "Admin"
}
```

### 2. Obtener el token JWT
```http
POST http://localhost:5XXX/api/auth/login
Content-Type: application/json

{
  "nombreUsuario": "admin",
  "password": "Admin123!"
}
```

**Respuesta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "nombreUsuario": "admin",
  "rol": "Admin"
}
```

### 3. Usar el token obtenido en las peticiones
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

---

## 📦 Endpoints de Productos

> Todos requieren autenticación con el token JWT.

| Método | Ruta | Descripción | Roles |
|--------|------|-------------|-------|
| `GET` | `/api/products` | Lista todos los productos | User, Admin |
| `GET` | `/api/products/{id}` | Obtiene producto por Id | User, Admin |
| `POST` | `/api/products` | Crea un nuevo producto | User, Admin |
| `PUT` | `/api/products/{id}` | Actualiza un producto | User, Admin |
| `DELETE` | `/api/products/{id}` | Elimina un producto | **Admin** |

### Ejemplo – Crear producto
```http
POST http://localhost:5000/api/products
Authorization: Bearer {token}
Content-Type: application/json

{
  "nombre": "Laptop HP Pavilion",
  "descripcion": "Procesador Intel Core i7, 16GB RAM",
  "stock": 10,
  "precio": 899.99
}
```

---

## 🛡️ Decisiones de seguridad

| Aspecto | Implementación |
|---------|---------------|
| Hash de contraseñas | **BCrypt** (BCrypt.Net-Next) |
| Autenticación | JWT Bearer con HMAC-SHA256 |
| Autorización | Claims-based (`ClaimTypes.Role`) |
| Eliminación de productos | Solo rol **Admin** |
| Expiración del token | 8 horas (configurable) |

---

## 🧰 Tecnologías utilizadas

- **.NET 8** – Framework principal
- **ASP.NET Core Web API** – Capa HTTP
- **Entity Framework Core 8** – ORM (Code First)
- **SQL Server** – Persistencia
- **BCrypt.Net-Next** – Hashing de contraseñas
- **Microsoft.AspNetCore.Authentication.JwtBearer** – Autenticación JWT
- **Swashbuckle** – Documentación de la API

---

## 🗄️ Modelo de datos

```sql
Producto (Id, Nombre, Descripcion, Stock, Precio, FechaCreacion)
Usuario  (Id, NombreUsuario [UNIQUE], PasswordHash, Rol)
```
