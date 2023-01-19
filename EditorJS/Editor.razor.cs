namespace EditorJS;

public partial class Editor
{
    [Inject] public IJSRuntime? JSRuntime { get; set; }
    [Parameter] public EventCallback<JsonObject> ValueChanged { get; set; }
    [Parameter] public JsonObject Value
    {
        get => _value;
        set => _value = value;
    }

    [Parameter] public required string Id { get; init; }
    [Parameter] public string? Name { get; init; }
    [Parameter] public string? Style { get; init; }

    /// <summary>
    /// A comma separated string of the plugins to load.
    /// If left empty or null, the editor will load all available supported plugins
    /// </summary>
    [Parameter] public string? PluginsCsv { get; init; }

    private JsonObject _value = new();
    private EditorJsInterop? _editor_js_interop;

    protected override async Task OnInitializedAsync()
    {
        ArgumentNullException.ThrowIfNull(JSRuntime);
        _editor_js_interop = new(Id, Value, PluginsCsv, JSRuntime, OnContentChangedRequestAsync);
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
}
