namespace BlazorApp.Pages;

public partial class Index
{
    [Inject] public required IJSRuntime JSRuntime { get; set; }

    /// <summary>
    /// Use this to render new content into the editor
    /// </summary>
    public Editor? editor_02 { get; set; }

    public JsonObject? EditorValue { get; set; }
    public JsonObject EditorTools { get; set; } = default!;
    public Task OnEditorValueChanged(JsonObject value) => Task.FromResult(EditorValue = value);

    public JsonObject? EditorValue02 { get; set; }
    public JsonObject EditorTools02 { get; set; } = default!;
    public Task OnEditorValue02Changed(JsonObject value) => Task.FromResult(EditorValue02 = value);

    protected override void OnInitialized()
    {
        EditorValue = JsonObject.Parse(Resource.JSON)?.AsObject();
        EditorTools = JsonObject.Parse("{}")?.AsObject() ?? new();

        string editor_tools = """
            { "header": null, "warning": null, "marker": null, "linkTool": null, "nestedList": null, "quote": null, "embed": { "config": { "services" : { "youtube": true } } } }
        """;
        EditorTools = JsonObject.Parse(editor_tools)?.AsObject() ?? new();

        // If the browser recieves the following error: "Saving failed due to the Error TypeError: Cannot read properties of undefined (reading 'sanitizeConfig')"
        // This is because edtorjs has certain dependencies caused by the `header.inlineToolbar' array values. EditorJS should have the appropriate tools/plugins enabled.
        // This error will also occur because of unsupported blocks in the editor and the whole editor document may need to be reset.
        string editor_tools_02 = """
            { "header": null, "linkTool": null, "nestedList": null, "marker": null, "warning": null, "checklist": null, "code": null, "delimiter": null, "embed": null, "simpleImage": null, "inlineCode": null, "quote": null, "table": null }
        """;
        EditorTools02 = JsonObject.Parse(editor_tools_02)?.AsObject() ?? new();
    }

    public async Task CheckEditorValueAsync()
    {
        string value = EditorValue?.ToString() ?? string.Empty;
        Console.WriteLine(value);
        await JSRuntime.InvokeVoidAsync("console.log", value);
    }

    public async Task CopyValueAsync()
    {
        if (editor_02 is null || EditorValue is null) { return; }
        await editor_02.RenderAsync(EditorValue);
    }
}
