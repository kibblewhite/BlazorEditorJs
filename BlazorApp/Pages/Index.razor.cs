using System.Text.Json;

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
    public JsonObject EditorConfigurations { get; set; } = default!;
    public Task OnEditorValueChanged(JsonObject value) => Task.FromResult(EditorValue = value);

    public JsonObject? EditorValue02 { get; set; }
    public JsonObject EditorTools02 { get; set; } = default!;
    public JsonObject EditorConfigurations02 { get; set; } = default!;
    public Task OnEditorValue02Changed(JsonObject value) => Task.FromResult(EditorValue02 = value);

    protected override void OnInitialized()
    {
        EditorTools = Editor.DefaultEditorJsonToolOptions();
        EditorValue = Editor.ParseJsonEditorValue(Resource.JSON);
        EditorConfigurations = JsonNode.Parse("""{ "DefaultBlock": "paragraph" }""")?.AsObject() ?? new();

        // If the browser recieves the following error: "Saving failed due to the Error TypeError: Cannot read properties of undefined (reading 'sanitizeConfig')"
        // This is because edtorjs has certain dependencies caused by the `header.inlineToolbar' array values. EditorJS should have the appropriate tools/plugins enabled.
        // This error will also occur because of unsupported blocks in the editor and the whole editor document may need to be reset.
        string editor_tools_02 = """
            {"Header":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"LinkTool":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"NestedList":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"list"}},"Marker":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Checklist":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"CodeTool":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"code"}},"Delimiter":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"SimpleImage":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"image"}},"Embed":{"LoadActions":{"OptionsNamingScheme":"CamelCase"},"options":{"config":{"services":{"instagram":true,"youtube":true,"vimeo":true,"imgur":true,"twitter":true,"facebook":true}}}},"InlineCode":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Quote":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Table":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}}}
        """; // "Warning":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}}

        EditorTools02 = Editor.ParseEditorJsonToolOptions(editor_tools_02);
        EditorValue02 = Editor.CreateEmptyJsonObject();
        EditorConfigurations02 = JsonNode.Parse("""{ "DefaultBlock": "paragraph" }""")?.AsObject() ?? new();
    }

    public async Task CheckEditorValueAsync()
    {
        JsonElement? json_element = EditorValue?.ConvertToJsonElement();
        string? unicode_decoded_json_string = json_element?.ToString();
        await JSRuntime.InvokeVoidAsync("console.log", unicode_decoded_json_string);
    }

    public async Task CopyValueAsync()
    {
        if (editor_02 is null || EditorValue is null) { return; }

        // todo (2023-10-17|kibble): Block comparison between the two editor values because maybe not all blocks are supported in the target editor.
        // Look at the `EditorTools` to check for blocks, in the future the tools/blocks should be explictly defined in order for this feature to work as intended.
        await editor_02.RenderAsync(EditorValue);
    }
}
