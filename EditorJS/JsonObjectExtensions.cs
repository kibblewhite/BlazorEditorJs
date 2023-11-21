namespace EditorJS;

/// <summary>
/// Provides extension methods for working with <see cref="JsonObject"/>.
/// </summary>
public static class JsonObjectExtensions
{

    private static readonly JsonSerializerOptions _json_serializer_options = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    // todo (2023-11-21|kibble): Unit testing and benchmarking (when there is something to benchmark it against) required on this method `ConvertToJsonElement`
    /// <summary>
    /// Converts a <see cref="JsonObject"/> to a <see cref="JsonElement"/> and removes any/all Unicode encoding.
    /// </summary>
    /// <remarks>Decode unicoded encoded strings within System.Text.Json.Nodes.JsonObject</remarks>
    /// <param name="json_object">The <see cref="JsonObject"/> to convert.</param>
    /// <returns>
    /// A <see cref="JsonElement"/> representing the content of the <see cref="JsonObject"/>.
    /// The Unicode encoding is removed during the conversion.
    /// </returns>
    public static JsonElement ConvertToJsonElement(this JsonObject json_object)
    {
        // Deserialize JsonObject to object
        object deserialised_editor_value = json_object.Deserialize<object>(_json_serializer_options) ?? new();

        // Convert object to UTF-8 encoded byte array
        byte[] deserialised_editor_byte_array = Encoding.Default.GetBytes(deserialised_editor_value.ToString() ?? string.Empty);

        // Decode the byte array to a UTF-8 encoded string
        string? unicode_decoded_json_string = Encoding.UTF8.GetString(deserialised_editor_byte_array);

        // Convert the string json value `unicode_decoded_json_string` into a JsonDocument and return it
        JsonDocument unicode_decoded_json_document = JsonDocument.Parse(unicode_decoded_json_string);

        return unicode_decoded_json_document.RootElement;
    }
}