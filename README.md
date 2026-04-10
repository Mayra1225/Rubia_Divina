# Rubia Divina - Login y CRUD con .NET 8 + Vue 3

Aplicación web desarrollada con **ASP.NET Core** y **Vue.js** para demostrar un flujo completo de **autenticación** y una **sección protegida con operaciones CRUD**.

## Objetivo del proyecto

Implementar una solución que permita:

- Registrar usuarios.
- Iniciar sesión con correo y contraseña.
- Restringir el acceso a URLs protegidas sin autenticación.
- Gestionar productos mediante operaciones **Crear, Leer, Actualizar y Eliminar**.
- Organizar el código de forma clara para su entrega en Git.

## Arquitectura utilizada

La solución está separada en dos capas:

### Backend - ASP.NET Core Web API
Responsable de:
- Modelo de datos.
- Lógica de autenticación.
- Generación de JWT.
- Endpoints protegidos.
- CRUD de productos.

### Frontend - Vue 3
Responsable de:
- Pantallas de Login y Registro.
- Protección de rutas.
- Consumo de la API.
- Interfaz del panel CRUD.

## Tecnologías usadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- JWT Authentication
- Vue 3
- Vue Router
- Axios
- Vite

## Estructura general

```text
Proyecto/
├── Backend/
│   └── Rubia_Divina/
│       └── Rubia_Divina/
├── Frontend/
│   └── rubia_divina/
└── README.md
```

## Funcionalidades implementadas

### Autenticación
- Registro de usuario.
- Login con validación de credenciales.
- Generación de token JWT.
- Cierre de sesión.

### Seguridad
- Las rutas del panel requieren autenticación.
- Si no existe token, el usuario es redirigido al login.
- Los endpoints del CRUD están protegidos con `[Authorize]`.

### CRUD de productos
- Crear producto.
- Visualizar listado de productos.
- Editar producto.
- Eliminar producto.

## Usuario de prueba

Al iniciar el backend se crea automáticamente un usuario de demostración:

- **Correo:** `admin@rubiadivina.com`
- **Contraseña:** `Admin123*`

## Requisitos previos

Antes de ejecutar el proyecto debe tener instalado:

- [.NET SDK 8](https://dotnet.microsoft.com/)
- [Node.js 20 o superior](https://nodejs.org/)

## Cómo ejecutar el backend

Abra una terminal dentro de:

```bash
Backend/Rubia_Divina/Rubia_Divina
```

Ejecute:

```bash
dotnet restore
dotnet run --urls=http://localhost:5055
```

El backend quedará disponible en:

```text
http://localhost:5055
```

Swagger:

```text
http://localhost:5055/swagger
```

## Cómo ejecutar el frontend

Abra otra terminal dentro de:

```bash
Frontend/rubia_divina
```

Ejecute:

```bash
npm install
npm run dev
```

El frontend quedará disponible en:

```text
http://localhost:5173
```

## Endpoints principales

### Autenticación
- `POST /api/auth/register`
- `POST /api/auth/login`

### Productos protegidos
- `GET /api/productos`
- `GET /api/productos/{id}`
- `POST /api/productos`
- `PUT /api/productos/{id}`
- `DELETE /api/productos/{id}`

## Guion sugerido para el video (máx. 3 minutos)

1. Mostrar la pantalla de login.
2. Intentar acceder manualmente a `/dashboard` sin iniciar sesión.
3. Evidenciar la redirección automática al login.
4. Iniciar sesión con el usuario de prueba.
5. Mostrar el panel protegido.
6. Crear un producto.
7. Editar el producto.
8. Eliminar el producto.
9. Cerrar sesión.
10. Intentar volver a entrar al panel sin token.

## Buenas prácticas aplicadas

- Separación entre frontend y backend.
- Uso de DTOs para entrada de datos.
- Contraseñas almacenadas con hash.
- Rutas protegidas en cliente y servidor.
- Repositorio limpio con `.gitignore`.
- README orientado a instalación y evaluación académica.

## Notas finales

Este proyecto fue preparado como una entrega académica enfocada en demostrar de manera clara:

- Autenticación funcional.
- Control de acceso.
- CRUD operativo.
- Organización del código.
- Facilidad de ejecución para revisión del docente.
