namespace EditorJS;

public partial class Editor
{
    [Inject] public required IJSRuntime JSRuntime { get; set; }
    [Parameter] public EventCallback<JsonObject> ValueChanged { get; set; }
    [Parameter] public JsonObject Value
    {
        get => _value;
        set => _value = value;
    }

    [Parameter] public required string Id { get; init; }
    [Parameter] public string? Name { get; init; }
    [Parameter] public string? Style { get; init; }

    [Parameter] public required JsonObject Tools { get; init; }

    /// <summary>
    /// Gets the default JSON configurations for editor tools as a string.
    /// </summary>
    /// <remarks>
    /// This property provides access to the default JSON configurations for various editor tools.
    /// The string is maintained based on the contents of the 'libman.json' file and is intended
    /// to help newer developers get started quickly with default configurations.
    /// More detailed information about customizing tool options can be found in the README.md files.
    /// </remarks>
    public static string DefaultEditorToolsConfigurationsJSON => _default_editor_tools_configurations_json;
    private static readonly string _default_editor_tools_configurations_json = """
            {"LinkTool":{"LoadActions":{"LoadProviderClassFunctionDefault":"LinkTool","OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"linkTools"},"options":null},"List":{"LoadActions":{"OptionsNamingScheme":"CamelCase"},"options":{"inlineToolbar":true,"shortcut":"CMD+SHIFT+L"}},"Header":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Warning":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Marker":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"NestedList":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"list"}},"Quote":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Embed":{"LoadActions":{"OptionsNamingScheme":"CamelCase"},"options":{"config":{"services":{"instagram":true}}}}}
        """;

    private JsonObject _value = new();
    private EditorJsInterop? _editor_js_interop;

    protected override async Task OnInitializedAsync()
    {
        ArgumentNullException.ThrowIfNull(JSRuntime);
        _editor_js_interop = new(Id, Value, Tools, JSRuntime, OnContentChangedRequestAsync);
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender is false) { return; }

        ArgumentNullException.ThrowIfNull(_editor_js_interop);
        await _editor_js_interop.InitAsync();
    }

    /// <summary>
    /// Handles changes made in the editorjs by updating the data model and invoking the user-defined event callback.
    /// </summary>
    /// <remarks>
    /// This method uses the <see cref="EqualityComparer{T}"/> to check for differences between the current and new values,
    /// in order to determine if the <see cref="ValueChanged">user-defined event callback</see> needs to be invoked and
    /// the data model needs to be updated.
    /// </remarks>
    /// <param name="jsob">The updated JSON object that represents the new value in the editorjs.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task OnContentChangedRequestAsync(JsonObject jsob)
    {
        if (EqualityComparer<JsonObject>.Default.Equals(Value, jsob) is true) { return; }

        await ValueChanged.InvokeAsync(jsob);
        Value = jsob;
    }

    /// <summary>
    /// Renders the editorjs with the new provided value, overriding the current value.
    /// This method uses <see cref="EqualityComparer{T}"/> to check if the new value is equal to the current value and if so, it will not execute the render function.
    /// </summary>
    /// <param name="jsob">The new value to be rendered in the editorjs</param>
    /// <returns>A task that represents the asynchronous rendering operation.</returns>
    public async Task RenderAsync(JsonObject jsob)
    {
        ArgumentNullException.ThrowIfNull(_editor_js_interop);
        if (EqualityComparer<JsonObject>.Default.Equals(Value, jsob) is true) { return; }

        await ValueChanged.InvokeAsync(jsob);
        await _editor_js_interop.RenderAsync(jsob);
        Value = jsob;
    }

    /// <summary>
    /// Parses a JSON string into a <see cref="JsonObject"/> for use with the <see cref="Editor.Value"/> parameter.
    /// </summary>
    /// <param name="json">The JSON string containing editor value.</param>
    /// <returns>
    /// A <see cref="JsonObject"/> representing the parsed editor value.
    /// If parsing fails or the input is null, it returns a new instance of <see cref="JsonObject"/>.
    /// </returns>
    public static JsonObject ParseJsonEditorValue(string json)
        => JsonNode.Parse(json)?.AsObject() ?? new();

    /// <summary>
    /// Provides a utility method for creating an empty <see cref="JsonObject"/> that can be used as the <see cref="Editor.Value"/> parameter for <see cref="Editor"/>.
    /// </summary>
    /// <remarks>
    /// The method creates a new instance of <see cref="JsonObject"/> with a single property "blocks" which is an empty JSON array.
    /// </remarks>
    /// <returns>
    /// A new instance of <see cref="JsonObject"/> that represents an empty JSON object.
    /// </returns>
    public static JsonObject CreateEmptyJsonObject()
        => JsonNode.Parse("{ \"blocks\": [] }")?.AsObject() ?? new();

    /// <summary>
    /// Parses a JSON string into a <see cref="JsonObject"/> for use with the <see cref="Editor.Tools"/> parameter.
    /// </summary>
    /// <param name="editor_tools_json">The JSON string containing editor tool options.</param>
    /// <returns>
    /// A <see cref="JsonObject"/> representing the parsed editor tool options.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the parsed JSON is null or required elements are missing.
    /// </exception>
    public static JsonObject ParseEditorJsonToolOptions(string editor_tools_json)
    {
        JsonObject? parsed_object = JsonNode.Parse(editor_tools_json)?.AsObject();

        return parsed_object is null || HasRequiredElements(parsed_object) is false
            ? throw new ArgumentException("Missing required elements in JSON structure.")
            : parsed_object;
    }

    /// <summary>
    /// Checks if the specified <see cref="JsonObject"/> has the required elements.
    /// </summary>
    /// <remarks>Missing required elements in JSON structure: checks for the presence of "LoadActions" and "OptionsNamingScheme".</remarks>
    /// <param name="json_object">The <see cref="JsonObject"/> to check for required elements.</param>
    /// <returns>
    /// <c>true</c> if the required elements are present in the <see cref="JsonObject"/>; otherwise, <c>false</c>.
    /// </returns>
    private static bool HasRequiredElements(JsonObject json_object) =>
        json_object.ContainsKey("LoadActions") is false || json_object.ContainsKey("OptionsNamingScheme") is false;

    /// <summary>
    /// Retrieves the default editor tool configurations as a <see cref="JsonObject"/>.
    /// </summary>
    /// <remarks>
    /// This method is designed to help newer developers get started quickly with default
    /// configurations for the editor tools. More detailed information about customizing
    /// tool options can be found in the README.md files. The string
    /// <see cref="DefaultEditorToolsConfigurationsJSON"/> should be
    /// maintained based on the contents of the 'libman.json'.
    /// </remarks>
    /// <returns>
    /// A <see cref="JsonObject"/> containing the default configurations for various editor tools.
    /// If parsing fails or the default configurations are not available, it returns an empty <see cref="JsonObject"/>.
    /// </returns>
    public static JsonObject DefaultEditorJsonToolOptions()
        => JsonNode.Parse(_default_editor_tools_configurations_json)?.AsObject() ?? new();

}
