# Motor de Recomendaciones - Duolingo

Sistema de recomendaciones basado en Neo4j para la plataforma Duolingo, implementado con ASP.NET Core y Blazor Server.

## Estructura del Proyecto

```
ObligatorioBDNR/
├── Components/
│   ├── App.razor                    # Componente raíz de Blazor
│   ├── _Imports.razor               # Imports globales
│   ├── Layout/
│   │   ├── MainLayout.razor         # Layout principal
│   │   └── NavMenu.razor            # Menú de navegación
│   └── Pages/
│       ├── Index.razor              # Página de inicio
│       ├── CargarDatos.razor        # Cargar datos en Neo4j
│       └── ConsultarRecomendaciones.razor  # Consultar recomendaciones
├── Data/
│   └── Neo4j/
│       └── Neo4jContext.cs          # Contexto de conexión a Neo4j
├── Models/
│   └── Recomendacion/               # Modelos de datos
│       ├── Usuario.cs
│       ├── Idioma.cs
│       ├── Unidad.cs
│       ├── Habilidad.cs
│       ├── Ejercicio.cs
│       ├── Error.cs
│       └── RecomendacionResultado.cs
├── Services/
│   └── Neo4j/
│       ├── INeo4jRecomendacionService.cs
│       └── Neo4jRecomendacionService.cs
└── appsettings.json                 # Configuración de conexión
```

## Requisitos Previos

1. **.NET 8.0 SDK** - [Descargar aquí](https://dotnet.microsoft.com/download)
2. **Neo4j** - Instalar Neo4j Desktop o usar Neo4j Aura
   - Neo4j Desktop: [Descargar aquí](https://neo4j.com/download/)
   - Neo4j Aura: [Crear cuenta gratuita](https://neo4j.com/cloud/aura/)

## Configuración

1. **Configurar Neo4j:**
   - Inicia Neo4j Desktop o conecta a Neo4j Aura
   - Anota la URI, usuario y contraseña

2. **Configurar la aplicación:**
   - Edita `appsettings.json` con tus credenciales de Neo4j:
   ```json
   {
     "Neo4j": {
       "Uri": "bolt://localhost:7687",
       "Username": "neo4j",
       "Password": "tu_contraseña"
     }
   }
   ```

3. **Instalar dependencias:**
   ```bash
   dotnet restore
   ```

## Ejecutar la Aplicación

```bash
dotnet run
```

La aplicación estará disponible en:
- HTTP: `http://localhost:5188`
- HTTPS: `https://localhost:7281`

## Funcionalidades

### 1. Cargar Datos

La página `/cargar-datos` permite crear:
- **Nodos:** Usuario, Idioma, Unidad, Habilidad, Ejercicio
- **Relaciones:**
  - Usuario ESTUDIA Idioma
  - Unidad DEL_IDIOMA Idioma
  - Habilidad DEL_IDIOMA Idioma
  - Ejercicio PERTENECE_A Unidad
  - Ejercicio REFUERZA Habilidad
  - Usuario REALIZA Ejercicio
  - Usuario FALLA_EN Habilidad
  - Usuario SIMILAR_A Usuario
  - Unidad PRECEDE_A Unidad

### 2. Consultar Recomendaciones

La página `/consultar-recomendaciones` permite consultar 5 tipos de recomendaciones:

1. **Basada en dificultades del usuario:** Recomienda ejercicios para reforzar habilidades donde el usuario tiene más fallas.
2. **Basada en similitud de usuarios:** Recomienda ejercicios que han sido útiles para usuarios con perfiles similares.
3. **Basada en contenido del curso:** Recomienda ejercicios según la estructura y contenido del curso de un idioma.
4. **Basada en idioma estudiado:** Recomienda ejercicios del idioma que el usuario está estudiando.
5. **Recomendación combinada:** Combina múltiples criterios para ofrecer recomendaciones más precisas.

## Ejemplo de Datos

Puedes usar el ejemplo del documento `MotorDeRecomendaciones.md` para cargar datos de prueba:

1. Crear un usuario con ID `u1` y username `Laura`
2. Crear un idioma con ID `en` y nombre `Inglés`
3. Crear la relación ESTUDIA entre el usuario y el idioma
4. Crear unidades, habilidades y ejercicios
5. Crear las relaciones correspondientes

## Tecnologías Utilizadas

- **ASP.NET Core 8.0** - Framework web
- **Blazor Server** - Framework de UI
- **Neo4j Driver 5.15.0** - Cliente para Neo4j
- **Bootstrap 5** - Framework CSS
- **Cypher** - Lenguaje de consulta de Neo4j

## Estructura de la Base de Datos

El modelo de datos incluye:

- **Nodos:** Usuario, Idioma, Unidad, Habilidad, Ejercicio, Error
- **Relaciones:** ESTUDIA, DEL_IDIOMA, PERTENECE_A, REFUERZA, REALIZA, FALLA_EN, SIMILAR_A, PRECEDE_A

Ver `MotorDeRecomendaciones.md` para más detalles sobre el modelo de datos.

## Notas

- Asegúrate de que Neo4j esté ejecutándose antes de iniciar la aplicación
- Las consultas Cypher están optimizadas para los patrones de acceso identificados
- La aplicación usa Blazor Server, por lo que requiere una conexión persistente al servidor

