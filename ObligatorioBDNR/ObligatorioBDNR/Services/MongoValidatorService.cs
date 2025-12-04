using MongoDB.Bson;
using MongoDB.Driver;

namespace ObligatorioBDNR.Services;

/// <summary>
/// Servicio para configurar validadores de esquema en MongoDB
/// </summary>
public class MongoValidatorService
{
    private readonly IMongoDatabase _database;

    public MongoValidatorService(IMongoDatabase database)
    {
        _database = database;
    }

    /// <summary>
    /// Configura todos los validadores de las colecciones
    /// </summary>
    public async Task ConfigurarValidadoresAsync()
    {
        await ConfigurarValidadorUsuariosAsync();
        await ConfigurarValidadorActividadUsuarioAsync();
        await ConfigurarValidadorLogrosDefinicionAsync();
        await ConfigurarValidadorEstadisticasUsuarioAsync();
    }

    /// <summary>
    /// Configura el validador para la colección de usuarios
    /// </summary>
    private async Task ConfigurarValidadorUsuariosAsync()
    {
        var validator = new BsonDocument
        {
            {
                "$jsonSchema", new BsonDocument
                {
                    { "bsonType", "object" },
                    { "required", new BsonArray { "_id", "Email", "Username", "FechaCreacion", "Autenticacion" } },
                    {
                        "properties", new BsonDocument
                        {
                            {
                                "_id", new BsonDocument
                                {
                                    { "bsonType", "string" },
                                    { "description", "ID único del usuario (GUID como string)" }
                                }
                            },
                            {
                                "Email", new BsonDocument
                                {
                                    { "bsonType", "string" },
                                    { "pattern", "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$" },
                                    { "description", "Email válido del usuario" }
                                }
                            },
                            {
                                "Username", new BsonDocument
                                {
                                    { "bsonType", "string" },
                                    { "minLength", 3 },
                                    { "maxLength", 50 },
                                    { "description", "Nombre de usuario (3-50 caracteres)" }
                                }
                            },
                            {
                                "FotoPerfil", new BsonDocument
                                {
                                    { "bsonType", "string" },
                                    { "description", "URL o ruta de la foto de perfil" }
                                }
                            },
                            {
                                "FechaCreacion", new BsonDocument
                                {
                                    { "bsonType", "date" },
                                    { "description", "Fecha de creación del usuario" }
                                }
                            },
                            {
                                "Autenticacion", new BsonDocument
                                {
                                    { "bsonType", "object" },
                                    { "required", new BsonArray { "PasswordHash" } },
                                    {
                                        "properties", new BsonDocument
                                        {
                                            {
                                                "PasswordHash", new BsonDocument
                                                {
                                                    { "bsonType", "string" },
                                                    { "minLength", 1 },
                                                    { "description", "Hash de la contraseña" }
                                                }
                                            },
                                            {
                                                "Tiene2FA", new BsonDocument
                                                {
                                                    { "bsonType", "bool" },
                                                    { "description", "Indica si tiene autenticación de dos factores" }
                                                }
                                            },
                                            {
                                                "Metodo2FA", new BsonDocument
                                                {
                                                    { "bsonType", "string" },
                                                    { "description", "Método de autenticación de dos factores" }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            {
                                "ConfiguracionPrivacidad", new BsonDocument
                                {
                                    { "bsonType", "object" },
                                    {
                                        "properties", new BsonDocument
                                        {
                                            {
                                                "PerfilVisible", new BsonDocument
                                                {
                                                    { "bsonType", "bool" }
                                                }
                                            },
                                            {
                                                "MostrarRacha", new BsonDocument
                                                {
                                                    { "bsonType", "bool" }
                                                }
                                            },
                                            {
                                                "PermitirMensajes", new BsonDocument
                                                {
                                                    { "bsonType", "bool" }
                                                }
                                            },
                                            {
                                                "CompartirActividad", new BsonDocument
                                                {
                                                    { "bsonType", "string" },
                                                    { "enum", new BsonArray { "todos", "amigos", "nadie", null } }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            {
                                "ProgresoGeneral", new BsonDocument
                                {
                                    { "bsonType", "object" },
                                    {
                                        "properties", new BsonDocument
                                        {
                                            {
                                                "IdiomaPrincipal", new BsonDocument
                                                {
                                                    { "bsonType", "string" }
                                                }
                                            },
                                            {
                                                "NivelesCompletados", new BsonDocument
                                                {
                                                    { "bsonType", "int" },
                                                    { "minimum", 0 }
                                                }
                                            },
                                            {
                                                "XpTotal", new BsonDocument
                                                {
                                                    { "bsonType", "int" },
                                                    { "minimum", 0 }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            {
                                "Logros", new BsonDocument
                                {
                                    { "bsonType", "array" },
                                    {
                                        "items", new BsonDocument
                                        {
                                            { "bsonType", "object" },
                                            { "required", new BsonArray { "IdLogro", "Nombre", "FechaObtencion" } },
                                            {
                                                "properties", new BsonDocument
                                                {
                                                    {
                                                        "IdLogro", new BsonDocument
                                                        {
                                                            { "bsonType", "string" }
                                                        }
                                                    },
                                                    {
                                                        "Nombre", new BsonDocument
                                                        {
                                                            { "bsonType", "string" }
                                                        }
                                                    },
                                                    {
                                                        "FechaObtencion", new BsonDocument
                                                        {
                                                            { "bsonType", "date" }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            {
                                "Amigos", new BsonDocument
                                {
                                    { "bsonType", "array" },
                                    {
                                        "items", new BsonDocument
                                        {
                                            { "bsonType", "object" },
                                            { "required", new BsonArray { "IdUsuario", "Username" } },
                                            {
                                                "properties", new BsonDocument
                                                {
                                                    {
                                                        "IdUsuario", new BsonDocument
                                                        {
                                                            { "bsonType", "string" }
                                                        }
                                                    },
                                                    {
                                                        "Username", new BsonDocument
                                                        {
                                                            { "bsonType", "string" },
                                                            { "minLength", 1 }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            {
                                "Cursos", new BsonDocument
                                {
                                    { "bsonType", "array" },
                                    {
                                        "items", new BsonDocument
                                        {
                                            { "bsonType", "object" },
                                            { "required", new BsonArray { "Idioma", "FechaInscripcion" } },
                                            {
                                                "properties", new BsonDocument
                                                {
                                                    {
                                                        "Idioma", new BsonDocument
                                                        {
                                                            { "bsonType", "string" },
                                                            { "minLength", 1 }
                                                        }
                                                    },
                                                    {
                                                        "XpAcumulado", new BsonDocument
                                                        {
                                                            { "bsonType", "int" },
                                                            { "minimum", 0 }
                                                        }
                                                    },
                                                    {
                                                        "UnidadesCompletadas", new BsonDocument
                                                        {
                                                            { "bsonType", "int" },
                                                            { "minimum", 0 }
                                                        }
                                                    },
                                                    {
                                                        "FechaInscripcion", new BsonDocument
                                                        {
                                                            { "bsonType", "date" }
                                                        }
                                                    },
                                                    {
                                                        "UnidadActual", new BsonDocument
                                                        {
                                                            { "bsonType", "int" },
                                                            { "minimum", 1 }
                                                        }
                                                    },
                                                    {
                                                        "NivelActual", new BsonDocument
                                                        {
                                                            { "bsonType", "int" },
                                                            { "minimum", 1 }
                                                        }
                                                    },
                                                    {
                                                        "Unidades", new BsonDocument
                                                        {
                                                            { "bsonType", "array" },
                                                            {
                                                                "items", new BsonDocument
                                                                {
                                                                    { "bsonType", "object" },
                                                                    {
                                                                        "properties", new BsonDocument
                                                                        {
                                                                            {
                                                                                "NumeroUnidad", new BsonDocument
                                                                                {
                                                                                    { "bsonType", "int" },
                                                                                    { "minimum", 1 }
                                                                                }
                                                                            },
                                                                            {
                                                                                "Nombre", new BsonDocument
                                                                                {
                                                                                    { "bsonType", "string" }
                                                                                }
                                                                            },
                                                                            {
                                                                                "NivelesTotales", new BsonDocument
                                                                                {
                                                                                    { "bsonType", "int" },
                                                                                    { "minimum", 1 }
                                                                                }
                                                                            },
                                                                            {
                                                                                "NivelesCompletados", new BsonDocument
                                                                                {
                                                                                    { "bsonType", "int" },
                                                                                    { "minimum", 0 }
                                                                                }
                                                                            },
                                                                            {
                                                                                "Completada", new BsonDocument
                                                                                {
                                                                                    { "bsonType", "bool" }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            {
                                "Suscripcion", new BsonDocument
                                {
                                    { "bsonType", "object" },
                                    {
                                        "properties", new BsonDocument
                                        {
                                            {
                                                "Tipo", new BsonDocument
                                                {
                                                    { "bsonType", "string" },
                                                    { "enum", new BsonArray { "FREE", "PLUS", null } }
                                                }
                                            },
                                            {
                                                "FechaInicio", new BsonDocument
                                                {
                                                    { "bsonType", "date" }
                                                }
                                            },
                                            {
                                                "FechaFin", new BsonDocument
                                                {
                                                    { "bsonType", "date" }
                                                }
                                            },
                                            {
                                                "AutoRenovacion", new BsonDocument
                                                {
                                                    { "bsonType", "bool" }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            {
                                "EstadisticasGenerales", new BsonDocument
                                {
                                    { "bsonType", "object" },
                                    {
                                        "properties", new BsonDocument
                                        {
                                            {
                                                "RachaActual", new BsonDocument
                                                {
                                                    { "bsonType", "int" },
                                                    { "minimum", 0 }
                                                }
                                            },
                                            {
                                                "RachaMaxima", new BsonDocument
                                                {
                                                    { "bsonType", "int" },
                                                    { "minimum", 0 }
                                                }
                                            },
                                            {
                                                "XpSemanaActual", new BsonDocument
                                                {
                                                    { "bsonType", "int" },
                                                    { "minimum", 0 }
                                                }
                                            },
                                            {
                                                "XpSemanaAnterior", new BsonDocument
                                                {
                                                    { "bsonType", "int" },
                                                    { "minimum", 0 }
                                                }
                                            },
                                            {
                                                "DiasActivosUltimos30", new BsonDocument
                                                {
                                                    { "bsonType", "int" },
                                                    { "minimum", 0 },
                                                    { "maximum", 30 }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        await AplicarValidadorAsync("usuarios", validator);
    }

    /// <summary>
    /// Configura el validador para la colección de actividad_usuario
    /// </summary>
    private async Task ConfigurarValidadorActividadUsuarioAsync()
    {
        var validator = new BsonDocument
        {
            {
                "$jsonSchema", new BsonDocument
                {
                    { "bsonType", "object" },
                    { "required", new BsonArray { "_id", "IdUsuario", "Fecha" } },
                    {
                        "properties", new BsonDocument
                        {
                            {
                                "_id", new BsonDocument
                                {
                                    { "bsonType", "string" },
                                    { "description", "ID único de la actividad (GUID como string)" }
                                }
                            },
                            {
                                "IdUsuario", new BsonDocument
                                {
                                    { "bsonType", "string" },
                                    { "description", "ID del usuario (GUID como string)" }
                                }
                            },
                            {
                                "Fecha", new BsonDocument
                                {
                                    { "bsonType", "date" },
                                    { "description", "Fecha de la actividad" }
                                }
                            },
                            {
                                "Acciones", new BsonDocument
                                {
                                    { "bsonType", "array" },
                                    {
                                        "items", new BsonDocument
                                        {
                                            { "bsonType", "object" },
                                            { "required", new BsonArray { "Tipo", "Timestamp" } },
                                            {
                                                "properties", new BsonDocument
                                                {
                                                    {
                                                        "Tipo", new BsonDocument
                                                        {
                                                            { "bsonType", "string" },
                                                            { "minLength", 1 },
                                                            { "description", "Tipo de acción realizada" }
                                                        }
                                                    },
                                                    {
                                                        "Timestamp", new BsonDocument
                                                        {
                                                            { "bsonType", "date" },
                                                            { "description", "Fecha y hora de la acción" }
                                                        }
                                                    },
                                                    {
                                                        "IdLeccion", new BsonDocument
                                                        {
                                                            { "bsonType", "string" },
                                                            { "description", "ID de la lección" }
                                                        }
                                                    },
                                                    {
                                                        "XpGanado", new BsonDocument
                                                        {
                                                            { "bsonType", "int" },
                                                            { "minimum", 0 },
                                                            { "description", "XP ganado en la acción" }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        await AplicarValidadorAsync("actividad_usuario", validator);
    }

    /// <summary>
    /// Configura el validador para la colección de logros_definicion
    /// </summary>
    private async Task ConfigurarValidadorLogrosDefinicionAsync()
    {
        var validator = new BsonDocument
        {
            {
                "$jsonSchema", new BsonDocument
                {
                    { "bsonType", "object" },
                    { "required", new BsonArray { "_id", "Nombre", "Descripcion", "XpRecompensa" } },
                    {
                        "properties", new BsonDocument
                        {
                            {
                                "_id", new BsonDocument
                                {
                                    { "bsonType", "string" },
                                    { "minLength", 1 },
                                    { "description", "ID único del logro" }
                                }
                            },
                            {
                                "Nombre", new BsonDocument
                                {
                                    { "bsonType", "string" },
                                    { "minLength", 1 },
                                    { "maxLength", 100 },
                                    { "description", "Nombre del logro" }
                                }
                            },
                            {
                                "Descripcion", new BsonDocument
                                {
                                    { "bsonType", "string" },
                                    { "minLength", 1 },
                                    { "maxLength", 500 },
                                    { "description", "Descripción del logro" }
                                }
                            },
                            {
                                "XpRecompensa", new BsonDocument
                                {
                                    { "bsonType", "int" },
                                    { "minimum", 0 },
                                    { "description", "XP de recompensa por obtener el logro" }
                                }
                            }
                        }
                    }
                }
            }
        };

        await AplicarValidadorAsync("logros_definicion", validator);
    }

    /// <summary>
    /// Configura el validador para la colección de estadisticas_usuario
    /// </summary>
    private async Task ConfigurarValidadorEstadisticasUsuarioAsync()
    {
        var validator = new BsonDocument
        {
            {
                "$jsonSchema", new BsonDocument
                {
                    { "bsonType", "object" },
                    { "required", new BsonArray { "_id", "IdUsuario", "Fecha" } },
                    {
                        "properties", new BsonDocument
                        {
                            {
                                "_id", new BsonDocument
                                {
                                    { "bsonType", "string" },
                                    { "description", "ID único de la estadística (GUID como string)" }
                                }
                            },
                            {
                                "IdUsuario", new BsonDocument
                                {
                                    { "bsonType", "string" },
                                    { "description", "ID del usuario (GUID como string)" }
                                }
                            },
                            {
                                "Fecha", new BsonDocument
                                {
                                    { "bsonType", "date" },
                                    { "description", "Fecha de la estadística" }
                                }
                            },
                            {
                                "XpTotalDia", new BsonDocument
                                {
                                    { "bsonType", "int" },
                                    { "minimum", 0 },
                                    { "description", "XP total del día" }
                                }
                            },
                            {
                                "TiempoEstudioMinutos", new BsonDocument
                                {
                                    { "bsonType", "int" },
                                    { "minimum", 0 },
                                    { "description", "Tiempo de estudio en minutos" }
                                }
                            },
                            {
                                "LeccionesCompletadas", new BsonDocument
                                {
                                    { "bsonType", "int" },
                                    { "minimum", 0 },
                                    { "description", "Número de lecciones completadas" }
                                }
                            },
                            {
                                "UnidadMasTrabajada", new BsonDocument
                                {
                                    { "bsonType", "string" },
                                    { "description", "Unidad más trabajada del día" }
                                }
                            },
                            {
                                "DetallePorCurso", new BsonDocument
                                {
                                    { "bsonType", "array" },
                                    {
                                        "items", new BsonDocument
                                        {
                                            { "bsonType", "object" },
                                            { "required", new BsonArray { "Idioma" } },
                                            {
                                                "properties", new BsonDocument
                                                {
                                                    {
                                                        "Idioma", new BsonDocument
                                                        {
                                                            { "bsonType", "string" },
                                                            { "minLength", 1 },
                                                            { "description", "Idioma del curso" }
                                                        }
                                                    },
                                                    {
                                                        "Xp", new BsonDocument
                                                        {
                                                            { "bsonType", "int" },
                                                            { "minimum", 0 },
                                                            { "description", "XP ganado en el curso" }
                                                        }
                                                    },
                                                    {
                                                        "Lecciones", new BsonDocument
                                                        {
                                                            { "bsonType", "int" },
                                                            { "minimum", 0 },
                                                            { "description", "Lecciones completadas en el curso" }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        await AplicarValidadorAsync("estadisticas_usuario", validator);
    }

    /// <summary>
    /// Aplica un validador a una colección. Si la colección no existe, la crea.
    /// </summary>
    private async Task AplicarValidadorAsync(string collectionName, BsonDocument validator)
    {
        try
        {
            // Verificar si la colección existe
            var collections = await _database.ListCollectionNamesAsync();
            var collectionNames = await collections.ToListAsync();
            var collectionExists = collectionNames.Contains(collectionName);

            if (collectionExists)
            {
                // Si la colección existe, actualizar el validador
                var command = new BsonDocument
                {
                    { "collMod", collectionName },
                    { "validator", validator },
                    { "validationLevel", "moderate" }, // Valida solo documentos nuevos y actualizados
                    { "validationAction", "error" } // Rechaza documentos que no cumplan el esquema
                };

                await _database.RunCommandAsync<BsonDocument>(command);
            }
            else
            {
                // Si la colección no existe, crearla con el validador usando un comando
                var createCommand = new BsonDocument
                {
                    { "create", collectionName },
                    { "validator", validator },
                    { "validationLevel", "moderate" },
                    { "validationAction", "error" }
                };

                await _database.RunCommandAsync<BsonDocument>(createCommand);
            }
        }
        catch (MongoCommandException ex)
        {
            // Si hay un error, intentar con validationLevel "strict" para documentos existentes
            // o simplemente loguear el error
            Console.WriteLine($"Error al aplicar validador a {collectionName}: {ex.Message}");
            // No lanzamos la excepción para que la aplicación pueda continuar
        }
    }
}

