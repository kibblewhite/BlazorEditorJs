namespace EditorJs;

public partial class Editor : ComponentBase
{
    [Inject] public required EditorJsInterop EditorJsInterop { get; init; }
    [Parameter] public EventCallback<JsonObject> ValueChanged { get; init; }
    [Parameter] public EventCallback<JsonObject> EnterKeyDownCapture { get; init; }
    [Parameter] public EventCallback<ProvisioningStatus> ProvisioningCallbacks { get; init; }
    [Parameter] public JsonObject Value { get => _value; set => _value = value; }
    [Parameter] public required string Id { get; set; }
    [Parameter] public string? Name { get; init; }
    [Parameter] public string? Style { get; init; }
    [Parameter] public string? Class { get; init; }
    [Parameter] public string? Title { get; init; }
    [Parameter] public required JsonObject Tools { get; init; }
    [Parameter] public required JsonObject Configurations { get; init; }

    private JsonObject _value = [];
    public ElementReference ElementReference;

    private bool _should_render;
    protected override bool ShouldRender() => _should_render;

    protected override Task OnInitializedAsync()
        => ProvisioningCallbacks.InvokeAsync(new ProvisioningStatus
        {
            Id = Id,
            State = ProvisioningState.Constructed
        });

    protected override async Task OnAfterRenderAsync(bool first_render)
    {
        if (first_render is true && _should_render is false)
        {
            _should_render = true;
            return;
        }

        await EditorJsInterop.InitAsync(ElementReference, Id, Value, Tools, Configurations, OnContentChangedRequestAsync, OnEnterKeyDownRequestAsync);
        _should_render = false;

        await ProvisioningCallbacks.InvokeAsync(new ProvisioningStatus
        {
            Id = Id,
            State = ProvisioningState.Initialised
        });
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
    public async Task OnContentChangedRequestAsync(JsonObject jsob)
    {
        if (EqualityComparer<JsonObject>.Default.Equals(Value, jsob) is true)
        {
            return;
        }

        await ValueChanged.InvokeAsync(jsob);
        Value = jsob;
    }

    /// <summary>
    /// Handles the Enter key down event and triggers the associated callback asynchronously.
    /// </summary>
    /// <remarks>This method invokes the <see cref="EnterKeyDownCapture"/> callback with the current <see
    /// cref="Value"/>. Ensure that the callback is properly configured to handle the event.</remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task OnEnterKeyDownRequestAsync()
        => EnterKeyDownCapture.InvokeAsync(Value);

    /// <summary>
    /// Renders the editorjs with the new provided value, overriding the current value.
    /// This method uses <see cref="EqualityComparer{T}"/> to check if the new value is equal to the current value and if so, it will not execute the render function.
    /// </summary>
    /// <param name="jsob">The new value to be rendered in the editorjs</param>
    /// <returns>A task that represents the asynchronous rendering operation.</returns>
    public async Task RenderAsync(JsonObject jsob)
    {
        if (EqualityComparer<JsonObject>.Default.Equals(Value, jsob) is true)
        {
            return;
        }

        await ValueChanged.InvokeAsync(jsob);
        await EditorJsInterop.RenderAsync(ElementReference, Id, jsob);
        Value = jsob;

        await ProvisioningCallbacks.InvokeAsync(new ProvisioningStatus
        {
            Id = Id,
            State = ProvisioningState.Rendered
        });
    }

    /// <summary>
    /// Clears the content of the editor, resetting it to an empty state.
    /// </summary>
    /// <remarks>This method asynchronously clears the editor's content. After calling this method,  the
    /// editor will be empty and ready for new input.</remarks>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task Clear() => EditorJsInterop.ClearAsync(ElementReference, Id);

    /// <summary>
    /// Sets focus to the editor element.
    /// </summary>
    /// <param name="on_last">A boolean value indicating whether to focus at the end of the last element within the editor. <see langword="true"/> focuses
    /// on the last element; <see langword="false"/> focuses on the first element.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation of setting focus.</returns>
    public Task Focus(bool on_last = false) => EditorJsInterop.FocusAsync(ElementReference, Id, on_last);

    /// <summary>
    /// Toggles the read-only state of the editor.
    /// </summary>
    /// <param name="read_only"><see langword="true"/> to set the editor to read-only mode; <see langword="false"/> to enable editing.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task ToggleAsync(bool read_only) => EditorJsInterop.ToggleAsync(ElementReference, Id, read_only);

    /// <summary>
    /// Parses a JSON string into a <see cref="JsonObject"/> for use with the <see cref="Value"/> parameter.
    /// </summary>
    /// <param name="json">The JSON string containing editor value.</param>
    /// <returns>
    /// A <see cref="JsonObject"/> representing the parsed editor value.
    /// If parsing fails or the input is null, it returns a new instance of <see cref="JsonObject"/>.
    /// </returns>
    public static JsonObject ParseJsonEditorValue(string json)
        => JsonNode.Parse(json)?.AsObject() ?? [];

    /// <summary>
    /// Provides a utility method for creating an empty <see cref="JsonObject"/> that can be used as the <see cref="Value"/> parameter.
    /// </summary>
    /// <remarks>
    /// The method creates a new instance of <see cref="JsonObject"/> with a single property "blocks" which is an empty JSON array.
    /// </remarks>
    /// <returns>
    /// A new instance of <see cref="JsonObject"/> that represents an empty JSON object.
    /// </returns>
    public static JsonObject CreateEmptyJsonObject()
        => JsonNode.Parse("{ \"blocks\": [] }")?.AsObject() ?? [];

    /// <summary>
    /// Parses a JSON string into a <see cref="JsonObject"/> for use with the <see cref="Tools"/> parameter.
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
    /// Checks that every tool entry in the root <see cref="JsonObject"/> contains a
    /// <c>LoadActions</c> child with an <c>OptionsNamingScheme</c> property.
    /// </summary>
    /// <param name="json_object">The root tool configurations <see cref="JsonObject"/>.</param>
    /// <returns>
    /// <c>true</c> if all tool entries have the required structure; otherwise, <c>false</c>.
    /// </returns>
    private static bool HasRequiredElements(JsonObject json_object)
    {
        if (json_object.Count is 0)
        {
            return false;
        }

        foreach ((string _, JsonNode? node) in json_object)
        {
            if (node is not JsonObject tool_entry)
            {
                return false;
            }

            if (tool_entry.TryGetPropertyValue("LoadActions", out JsonNode? load_actions_node) is false
                || load_actions_node is not JsonObject load_actions)
            {
                return false;
            }

            if (load_actions.ContainsKey("OptionsNamingScheme") is false)
            {
                return false;
            }
        }

        return true;
    }
}
