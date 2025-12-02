# Duolingo - Gestión de Usuarios y Perfiles

Sistema completo de gestión de usuarios y perfiles para Duolingo implementado con Blazor Server, MongoDB y ASP.NET Core.

## Estructura del Proyecto

```
ObligatorioBDNR/
├── Domain/                    # Clases de dominio
│   ├── Usuario.cs
│   ├── LogroDefinicion.cs
│   └── ActividadUsuario.cs
├── DataAccess/               # Acceso a datos
│   ├── MongoContext.cs
│   └── Repositories/
│       ├── UsuarioRepository.cs
│       ├── LogroRepository.cs
│       └── ActividadUsuarioRepository.cs
└── ObligatorioBDNR/          # Aplicación Blazor Server
    ├── Pages/
    │   ├── Index.razor       # Dashboard
    │   ├── Usuarios/
    │   │   ├── ListaUsuarios.razor
    │   │   ├── CrearUsuario.razor
    │   │   ├── EditarUsuario.razor
    │   │   └── DetalleUsuario.razor
    │   ├── Logros/
    │   │   ├── ListaLogros.razor
    │   │   ├── CrearLogro.razor
    │   │   └── EditarLogro.razor
    │   └── Actividad/
    │       ├── ListaActividad.razor
    │       └── RegistrarActividad.razor
    ├── Models/DTOs/          # DTOs para formularios
    ├── Services/             # Servicios (SeedService)
    └── Program.cs
```

## Requisitos Previos

- .NET 8.0 SDK
- MongoDB (versión 4.4 o superior)
- Visual Studio 2022 o VS Code

## Configuración

### 1. MongoDB

Asegúrate de que MongoDB esté ejecutándose en `localhost:27017`.

### 2. Configuración de la Base de Datos

La configuración se encuentra en `appsettings.json`:

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "Duolingo"
  }
}
```

### 3. Restaurar Paquetes NuGet

```bash
dotnet restore
```

## Ejecución

1. Asegúrate de que MongoDB esté ejecutándose
2. Ejecuta la aplicación:

```bash
dotnet run --project ObligatorioBDNR/ObligatorioBDNR.csproj
```

3. Abre el navegador en `https://localhost:5001` o `http://localhost:5000`

## Funcionalidades

### Usuarios

- **Listar Usuarios** (`/usuarios`): Ver todos los usuarios registrados
- **Crear Usuario** (`/usuarios/crear`): Formulario completo para crear nuevos usuarios
- **Editar Usuario** (`/usuarios/editar/{id}`): Modificar información del usuario
- **Detalle Usuario** (`/usuarios/detalle/{id}`): Ver perfil completo con logros, amigos, progreso, etc.

### Logros

- **Listar Logros** (`/logros`): Ver catálogo de logros disponibles
- **Crear Logro** (`/logros/crear`): Definir nuevos logros
- **Editar Logro** (`/logros/editar/{id}`): Modificar logros existentes

### Actividad

- **Listar Actividad** (`/actividad/{idUsuario}`): Ver actividades paginadas de un usuario
- **Registrar Actividad** (`/actividad/registrar/{idUsuario}`): Agregar nueva actividad

### Dashboard

- Vista general con estadísticas:
  - Total de usuarios
  - Total de logros
  - Total de actividades
  - XP total acumulado
  - Selección de usuario para ver detalles

## Base de Datos

### Colecciones

- **usuarios**: Perfiles completos de usuarios
- **logros_definicion**: Catálogo de logros
- **actividad_usuario**: Registro de actividades diarias

### Seed de Datos

En modo desarrollo, la aplicación ejecuta automáticamente un seed con datos de ejemplo:
- 2 usuarios de prueba
- 4 logros predefinidos
- Actividades de ejemplo

## Características Técnicas

- **Blazor Server**: Interfaz interactiva con actualizaciones en tiempo real
- **MongoDB**: Base de datos NoSQL para almacenamiento flexible
- **Inyección de Dependencias**: Arquitectura limpia y testeable
- **Validación de Formularios**: Validación del lado del cliente y servidor
- **DTOs**: Separación entre modelos de dominio y modelos de vista
- **Repositorios**: Patrón Repository para acceso a datos

## Notas

- Las contraseñas se hashean usando BCrypt
- Los IDs de usuarios son GUIDs
- Los IDs de logros son strings
- La paginación está implementada para actividades
- El seed solo se ejecuta si la base de datos está vacía

## Estructura de Clases de Dominio

Las clases de dominio (`Usuario`, `LogroDefinicion`, `ActividadUsuario`) ya están definidas y no deben modificarse, excepto para agregar anotaciones de MongoDB si es necesario.

## Licencia

Este proyecto es parte de un trabajo obligatorio académico.

