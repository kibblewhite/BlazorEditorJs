namespace EditorJs;

/// <summary>
/// Provides extension methods for working with <see cref="JsonObject"/>.
/// </summary>
public static class JsonObjectExtensions
{
    private static readonly JsonSerializerOptions _json_serializer_options = new()
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>
    /// Converts a <see cref="JsonObject"/> to a <see cref="JsonElement"/> and removes any/all Unicode encoding.
    /// </summary>
    /// <remarks>Decode Unicode-encoded strings within <see cref="JsonObject"/>.</remarks>
    /// <param name="json_object">The <see cref="JsonObject"/> to convert.</param>
    /// <returns>
    /// A <see cref="JsonElement"/> representing the content of the <see cref="JsonObject"/>.
    /// The Unicode encoding is removed during the conversion.
    /// </returns>
    public static JsonElement ConvertToJsonElement(this JsonObject json_object)
    {
        object deserialised_editor_value = json_object.Deserialize<object>(_json_serializer_options) ?? new();
        byte[] deserialised_editor_byte_array = Encoding.Default.GetBytes(deserialised_editor_value.ToString() ?? string.Empty);
        string unicode_decoded_json_string = Encoding.UTF8.GetString(deserialised_editor_byte_array);
        JsonDocument unicode_decoded_json_document = JsonDocument.Parse(unicode_decoded_json_string);
        return unicode_decoded_json_document.RootElement;
    }
}
