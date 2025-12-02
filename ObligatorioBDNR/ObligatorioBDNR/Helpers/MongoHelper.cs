using MongoDB.Bson;

namespace ObligatorioBDNR.Helpers;

/// <summary>
/// Helper para conversiones relacionadas con MongoDB
/// </summary>
public static class MongoHelper
{
    /// <summary>
    /// Convierte un string a ObjectId de MongoDB
    /// </summary>
    public static ObjectId ToObjectId(string id)
    {
        if (ObjectId.TryParse(id, out var objectId))
        {
            return objectId;
        }
        throw new ArgumentException("Invalid ObjectId format", nameof(id));
    }

    /// <summary>
    /// Convierte un ObjectId a string
    /// </summary>
    public static string ToString(ObjectId id)
    {
        return id.ToString();
    }

    /// <summary>
    /// Valida si un string es un ObjectId válido
    /// </summary>
    public static bool IsValidObjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}

