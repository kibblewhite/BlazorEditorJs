namespace BlazorApp.Pages;

public partial class Index
{
    public Editor? editor_02 { get; set; }

    public JsonObject? EditorValue { get; set; }
    public Task OnEditorValueChanged(JsonObject value) => Task.FromResult(EditorValue = value);

    public JsonObject? EditorValue02 { get; set; }
    public Task OnEditorValue02Changed(JsonObject value) => Task.FromResult(EditorValue02 = value);

    protected override void OnInitialized() => EditorValue = JsonObject.Parse(Resource.JSON)?.AsObject();

    public void CheckEditorValue()
    {
        string value = EditorValue?.ToString() ?? string.Empty;
        Console.WriteLine(value);
    }

    public async Task CopyValueAsync()
    {
        if (editor_02 is null || EditorValue is null) { return; }
        await editor_02.RenderAsync(EditorValue);
    }
}
