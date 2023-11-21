namespace EditorJS;

internal class EditorJsInterop : IAsyncDisposable
{
    private readonly DotNetObjectReference<EditorJsInterop> _dot_net_object_reference;
    private readonly Lazy<Task<IJSObjectReference>> _module_task;
    private readonly Delegate _update_delegate;
    private readonly IJSRuntime _js_runtime;
    private readonly JsonObject? _jsob;
    private readonly string _id;
    private readonly JsonObject _tools;
    private readonly JsonObject _configurations;

    public EditorJsInterop(string id, JsonObject? jsob, JsonObject tools, JsonObject configurations, IJSRuntime js_runtime, Func<JsonObject, Task> on_change)
    {
        ArgumentNullException.ThrowIfNull(js_runtime, nameof(js_runtime));
        _js_runtime = js_runtime;
        _dot_net_object_reference = DotNetObjectReference.Create(this);

        _id = id;
        _jsob = jsob;
        _module_task = new Lazy<Task<IJSObjectReference>>(() =>
                 _js_runtime.InvokeAsync<IJSObjectReference>("import",
                     "./_content/EditorJS/lib/editorjs-interop.js").AsTask());

        _update_delegate = on_change;
        _tools = tools;
        _configurations = configurations;
    }

    public async ValueTask DisposeAsync()
    {
        _dot_net_object_reference.Dispose();
        IJSObjectReference module = await _module_task.Value;
        await module.DisposeAsync();
    }

    public async Task InitAsync()
    {
        if (_module_task is null) { return; }

        IJSObjectReference module = await _module_task.Value;
        await module.InvokeVoidAsync("editorjs.init", _id, _jsob, _tools, _configurations, DotNetObjectReference.Create(this), nameof(OnChangeAsync));
    }

    public async Task RenderAsync(JsonObject jsob)
    {
        if (_module_task is null) { return; }

        IJSObjectReference module = await _module_task.Value;
        await module.InvokeVoidAsync("editorjs.render", _id, jsob);
    }

    [JSInvokable]
    public async Task<bool> OnChangeAsync(JsonObject jsob)
    {
        Task? invoked_delegate = _update_delegate.DynamicInvoke(jsob) as Task;
        return await Task.FromResult(invoked_delegate?.IsCompletedSuccessfully ?? false);
    }
}
