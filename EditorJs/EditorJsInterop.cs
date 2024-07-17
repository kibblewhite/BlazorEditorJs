namespace EditorJs;

public sealed class EditorJsInterop(IJSRuntime js_runtime, ILoggerFactory logger_factory) : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _module_task = new(() =>
            js_runtime.InvokeAsync<IJSObjectReference>("import", "./_content/EditorJs/lib/editorjs-interop.js").AsTask());

    private readonly ILogger<EditorJsInterop> _logger = logger_factory.CreateLogger<EditorJsInterop>();

    private Lazy<DotNetObjectReference<EditorJsInterop>>? _dot_net_object_reference;
    private readonly Dictionary<string, Delegate> _update_delegates = [];
    public readonly DateTime InitialisedUtcDateTime = DateTime.UtcNow;

    public async ValueTask DisposeAsync()
    {
        _dot_net_object_reference?.Value.Dispose();
        IJSObjectReference module = await _module_task.Value;
        await module.DisposeAsync();
    }

    private static string FormatElementSelectorKey(string id, string element_id) => $"{id}.{element_id}";

    public async Task InitAsync(ElementReference element_reference, string id, JsonObject jsob, JsonObject tools, JsonObject configurations, Func<JsonObject, Task> on_change)
    {
        if (_dot_net_object_reference?.IsValueCreated is not true)
        {
            _dot_net_object_reference = new(() => DotNetObjectReference.Create(this));
        }

        string identifier = FormatElementSelectorKey(id, element_reference.Id);
        if (_update_delegates.ContainsKey(identifier))
        {
            return;
        }

        _update_delegates.Add(identifier, on_change);

        IJSObjectReference module = await _module_task.Value;
        await module.InvokeVoidAsync("editorjs.init", id, element_reference.Id, jsob, tools, configurations, _dot_net_object_reference.Value, nameof(OnChangeAsync));
    }

    public async Task RenderAsync(ElementReference element_reference, string id, JsonObject jsob)
    {
        IJSObjectReference module = await _module_task.Value;
        await module.InvokeVoidAsync("editorjs.render", id, element_reference.Id, jsob);
    }

    [JSInvokable]
    public async Task<bool> OnChangeAsync(string id, string element_id, JsonObject jsob)
    {
        string identifier = FormatElementSelectorKey(id, element_id);
        Delegate? update_delegate = _update_delegates.GetValueOrDefault(identifier);
        Task? invoked_delegate = update_delegate?.DynamicInvoke(jsob) as Task;
        return await Task.FromResult(invoked_delegate?.IsCompletedSuccessfully ?? false);
    }
}
