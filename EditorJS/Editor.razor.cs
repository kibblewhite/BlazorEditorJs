namespace EditorJS;

public partial class Editor
{
    [Inject] public IJSRuntime? JSRuntime { get; set; }
    [Parameter] public EventCallback<JsonObject> ValueChanged { get; set; }

    [Parameter] public string Id { get; init; } = default!;
    [Parameter] public string? Name { get; init; }
    [Parameter] public string? Style { get; init; }
    [Parameter] public JsonObject Value
    {
        get => _value ??= new JsonObject();
        set
        {
            if (_value == value) { return; }
            _value = value;

            // https://github.com/dotnet/aspnetcore/issues/22394
            Task render = Task.Run(() => _editor_js_interop?.Render(value));
        }
    }

    private JsonObject? _value;
    private EditorJsInterop? _editor_js_interop;

    protected override async Task OnInitializedAsync()
    {
        ArgumentNullException.ThrowIfNull(JSRuntime);
        _editor_js_interop = new(Id, _value, JSRuntime, OnContentChangedRequestAsync);
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender is false) { return; }
        ArgumentNullException.ThrowIfNull(_editor_js_interop);
        await _editor_js_interop.Init();
    }

    protected async Task OnContentChangedRequestAsync(JsonObject jsob)
    {
        if (_value == jsob) { return; }
        _value = jsob;
        await ValueChanged.InvokeAsync(jsob);
    }
}
