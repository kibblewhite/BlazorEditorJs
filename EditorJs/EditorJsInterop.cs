namespace EditorJs;

/// <summary>
/// Manages the JavaScript interop bridge between Blazor and the EditorJS library.
/// Handles editor initialisation, content rendering, focus, read-only toggling, and bidirectional event callbacks.
/// Registered as a scoped service via <see cref="Extensions.AddScopedEditorJsInterop"/>.
/// </summary>
public sealed class EditorJsInterop(IJSRuntime js_runtime) : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _module_task = new(() =>
            js_runtime.InvokeAsync<IJSObjectReference>("import", "./_content/EditorJs/lib/editorjs-interop.js").AsTask());

    private Lazy<DotNetObjectReference<EditorJsInterop>>? _dot_net_object_reference;
    private readonly Dictionary<string, Delegate> _update_delegates = [];
    private readonly Dictionary<string, Delegate> _enterkey_down_delegates = [];

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        _dot_net_object_reference?.Value.Dispose();
        IJSObjectReference module = await _module_task.Value;
        await module.DisposeAsync();
    }

    private static string FormatElementSelectorKey(string id, string element_id) => $"{id}.{element_id}";

    /// <summary>
    /// Initialises an EditorJS instance in the browser, binding it to the specified holder element.
    /// </summary>
    /// <param name="element_reference">The Blazor <see cref="ElementReference"/> for the holder div.</param>
    /// <param name="id">The unique identifier of the editor instance.</param>
    /// <param name="jsob">The initial editor content as a <see cref="JsonObject"/>.</param>
    /// <param name="tools">The tool configurations produced by <see cref="Builders.EditorToolsBuilder"/>.</param>
    /// <param name="configurations">The editor configurations produced by <see cref="Builders.EditorConfigBuilder"/>.</param>
    /// <param name="on_change">Callback invoked when editor content changes.</param>
    /// <param name="on_enterkey_down">Callback invoked when the Enter key is pressed (for single-line editors).</param>
    public async Task InitAsync(ElementReference element_reference, string id, JsonObject jsob, JsonObject tools, JsonObject configurations, Func<JsonObject, Task> on_change, Func<Task> on_enterkey_down)
    {
        if (_dot_net_object_reference?.IsValueCreated is not true)
        {
            _dot_net_object_reference = new(() => DotNetObjectReference.Create(this));
        }

        string identifier = FormatElementSelectorKey(id, element_reference.Id);
        if (_update_delegates.ContainsKey(identifier) is true)
        {
            return;
        }

        _update_delegates.Add(identifier, on_change);
        _enterkey_down_delegates.Add(identifier, on_enterkey_down);

        IJSObjectReference module = await _module_task.Value;
        await module.InvokeVoidAsync("editorjs.init", id, element_reference.Id, jsob, tools, configurations, _dot_net_object_reference.Value, nameof(OnChangeAsync), nameof(OnEnterKeyDownAsync));
    }

    /// <summary>
    /// Replaces the editor content with the provided data, clearing existing blocks first.
    /// </summary>
    /// <param name="element_reference">The Blazor <see cref="ElementReference"/> for the holder div.</param>
    /// <param name="id">The unique identifier of the editor instance.</param>
    /// <param name="jsob">The new editor content as a <see cref="JsonObject"/>.</param>
    public async Task RenderAsync(ElementReference element_reference, string id, JsonObject jsob)
    {
        IJSObjectReference module = await _module_task.Value;
        await module.InvokeVoidAsync("editorjs.render", id, element_reference.Id, jsob);
    }

    /// <summary>
    /// Clears all blocks from the editor.
    /// </summary>
    /// <param name="element_reference">The Blazor <see cref="ElementReference"/> for the holder div.</param>
    /// <param name="id">The unique identifier of the editor instance.</param>
    public async Task ClearAsync(ElementReference element_reference, string id)
    {
        IJSObjectReference module = await _module_task.Value;
        await module.InvokeVoidAsync("editorjs.clear", id, element_reference.Id);
    }

    /// <summary>
    /// Sets focus to the editor.
    /// </summary>
    /// <param name="element_reference">The Blazor <see cref="ElementReference"/> for the holder div.</param>
    /// <param name="id">The unique identifier of the editor instance.</param>
    /// <param name="on_last"><c>true</c> to focus on the last block; <c>false</c> to focus on the first.</param>
    public async Task FocusAsync(ElementReference element_reference, string id, bool on_last)
    {
        IJSObjectReference module = await _module_task.Value;
        await module.InvokeVoidAsync("editorjs.focus", id, element_reference.Id, on_last);
    }

    /// <summary>
    /// Toggles the read-only state of the editor.
    /// </summary>
    /// <param name="element_reference">The Blazor <see cref="ElementReference"/> for the holder div.</param>
    /// <param name="id">The unique identifier of the editor instance.</param>
    /// <param name="read_only"><c>true</c> to set read-only mode; <c>false</c> to enable editing.</param>
    public async Task ToggleAsync(ElementReference element_reference, string id, bool read_only)
    {
        IJSObjectReference module = await _module_task.Value;
        await module.InvokeVoidAsync("editorjs.toggle", id, element_reference.Id, read_only);
    }

    /// <summary>
    /// Called from JavaScript when editor content changes. Routes the update to the registered delegate.
    /// </summary>
    [JSInvokable]
    public Task<bool> OnChangeAsync(string id, string element_id, JsonObject jsob)
    {
        string identifier = FormatElementSelectorKey(id, element_id);
        Delegate? update_delegate = _update_delegates.GetValueOrDefault(identifier);
        Task? invoked_delegate = update_delegate?.DynamicInvoke(jsob) as Task;
        return Task.FromResult(invoked_delegate?.IsCompletedSuccessfully ?? false);
    }

    /// <summary>
    /// Called from JavaScript when the Enter key is pressed. Routes the event to the registered delegate.
    /// </summary>
    [JSInvokable]
    public Task<bool> OnEnterKeyDownAsync(string id, string element_id)
    {
        string identifier = FormatElementSelectorKey(id, element_id);
        Delegate? enterkey_down_delegate = _enterkey_down_delegates.GetValueOrDefault(identifier);
        Task? invoked_delegate = enterkey_down_delegate?.DynamicInvoke() as Task;
        return Task.FromResult(invoked_delegate?.IsCompletedSuccessfully ?? false);
    }
}
