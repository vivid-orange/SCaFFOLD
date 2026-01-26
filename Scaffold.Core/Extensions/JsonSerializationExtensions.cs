using System.IO;
using System.Text.Json;

/// <summary>
/// Generic extension methods for JSON serialization and deserialization of types that implement
/// IParsable and IFormattable. These methods provide a consistent way to convert objects
/// to and from JSON format, and persist them to files.
/// </summary>
public static class JsonSerializationExtensions
{
    /// <summary>
    /// Converts an object to a JSON string.
    /// Only available on types that implement IFormattable.
    /// </summary>
    /// <typeparam name="T">The type to serialize. Must implement IFormattable.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="options">Optional JSON serializer options.</param>
    /// <returns>A JSON string representation of the object.</returns>
    public static string ToJson<T>(this T obj, JsonSerializerOptions? options = null)
        where T : IFormattable
    {
        return JsonSerializer.Serialize(obj, options) ?? string.Empty;
    }

    /// <summary>
    /// Converts a JSON string to an object of the specified type.
    /// Only available on types that implement IParsable.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to. Must implement IParsable.</typeparam>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <param name="options">Optional JSON serializer options.</param>
    /// <returns>The deserialized object, or default if deserialization fails.</returns>
    public static T? FromJson<T>(this string json, JsonSerializerOptions? options = null)
        where T : IParsable<T>
    {
        try
        {
            return JsonSerializer.Deserialize<T>(json, options);
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// Saves an object to a JSON file.
    /// Only available on types that implement IFormattable.
    /// </summary>
    /// <typeparam name="T">The type to serialize. Must implement IFormattable.</typeparam>
    /// <param name="obj">The object to save.</param>
    /// <param name="filePath">The file path where the JSON will be written.</param>
    /// <param name="options">Optional JSON serializer options.</param>
    public static void SaveToJsonFile<T>(this T obj, string filePath, JsonSerializerOptions? options = null)
        where T : IFormattable
    {
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string json = obj.ToJson(options);
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// Loads an object from a JSON file.
    /// Only available on types that implement IParsable.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to. Must implement IParsable.</typeparam>
    /// <param name="filePath">The file path to read from.</param>
    /// <param name="options">Optional JSON serializer options.</param>
    /// <returns>The deserialized object, or default if the file doesn't exist or deserialization fails.</returns>
    public static T? LoadFromJsonFile<T>(string filePath, JsonSerializerOptions? options = null)
        where T : IParsable<T>
    {
        if (!File.Exists(filePath))
        {
            return default;
        }

        try
        {
            string json = File.ReadAllText(filePath);
            return json.FromJson<T>(options);
        }
        catch
        {
            return default;
        }
    }
}
