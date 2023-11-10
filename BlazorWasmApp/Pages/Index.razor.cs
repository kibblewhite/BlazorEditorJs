using EditorJS;
using System.Text.Json.Nodes;

namespace BlazorWasmApp.Pages;

public partial class Index
{
    public JsonObject? EditorValue { get; set; }
    public JsonObject EditorTools { get; set; } = default!;
    public Task OnEditorValueChanged(JsonObject value) => Task.FromResult(EditorValue = value);

    protected override void OnInitialized()
    {
        EditorValue = Editor.CreateEmptyJsonObject();

        // In this example the Toggle configurations have been dynamically loaded in from an external CDN -> https://github.com/kommitters/editorjs-toggle-block
        string editor_tools = """
            {"Toggle":{"LoadActions":{"LoadProviderClassFunctionDefault":"ToggleBlock","OptionsNamingScheme":"CamelCase"},"options":{"inlineToolbar":true}},"Header":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"LinkTool":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"NestedList":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"list"}},"Marker":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Warning":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Checklist":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"CodeTool":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"code"}},"Delimiter":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"SimpleImage":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"image"}},"Embed":{"LoadActions":{"OptionsNamingScheme":"CamelCase"},"options":{"config":{"services":{"instagram":true,"youtube":true,"vimeo":true,"imgur":true,"twitter":true,"facebook":true}}}},"InlineCode":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Quote":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Table":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}}}
        """;
        EditorTools = Editor.ParseEditorJsonToolOptions(editor_tools);
    }
}