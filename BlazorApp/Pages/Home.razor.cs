using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json.Nodes;
using System.Text.Json;
using EditorJs;

namespace BlazorApp.Pages;
public partial class Home : ComponentBase
{
    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    private JsonObject? EditorValue { get; set; }
    public JsonObject EditorTools { get; set; } = default!;
    public JsonObject EditorConfigurations { get; set; } = default!;
    public Task OnEditorValueChanged(JsonObject value) => Task.FromResult(EditorValue = value);

    /// <summary>
    /// Use this to render new content into the editor
    /// </summary>
    public Editor? Editor02 { get; set; }

    public JsonObject? EditorValue02 { get; set; }
    public JsonObject EditorTools02 { get; set; } = default!;
    public JsonObject EditorConfigurations02 { get; set; } = default!;
    public Task OnEditorValue02Changed(JsonObject value) => Task.FromResult(EditorValue02 = value);

    protected override void OnAfterRender(bool first_render)
    {
        if (first_render)
        {
            StateHasChanged();
        }
    }

    public async Task InvokeInsertAsync()
    {
        if (Editor02 is null)
        {
            return;
        }

        string json_t = """
            {
              "time": 1717207275445,
              "blocks": [
                {
                  "id": "mhTl6ghSkV",
                  "type": "paragraph",
                  "data": {
                    "text": "Hey. Meet the new Editor. On this picture you can see it in action. Then, try a demo"
                  }
                }
              ],
              "version": "2.29.1"
            }
            """;
        await Editor02.RenderAsync(JsonNode.Parse(json_t)?.AsObject() ?? []);
    }

    protected override void OnInitialized()
    {
        string json_t = """{"time": 1717207275445, "blocks": [{"id": "qDEsgkmbL1", "data": {"text": "Heylo, World!", "wrap": "title"}, "type": "text"}], "version": "2.29.1"}""";
        EditorValue = JsonNode.Parse(json_t)?.AsObject() ?? [];

        // In this example the Toggle configurations have been dynamically loaded in from an external CDN -> https://github.com/kommitters/editorjs-toggle-block
        // string editor_tools = """{"Toggle":{"LoadActions":{"LoadProviderClassFunctionDefault":"ToggleBlock","OptionsNamingScheme":"CamelCase"},"options":{"inlineToolbar":true}},"Header":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"LinkTool":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"NestedList":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"list"}},"Marker":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Warning":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Checklist":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"CodeTool":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"code"}},"Delimiter":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"SimpleImage":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"image"}},"Embed":{"LoadActions":{"OptionsNamingScheme":"CamelCase"},"options":{"config":{"services":{"instagram":true,"youtube":true,"vimeo":true,"imgur":true,"twitter":true,"facebook":true}}}},"InlineCode":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Quote":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Table":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}}}""";

        string editor_tools = """
        {
          "Paragraph": {
            "LoadActions": {
              "LoadProviderClassFunctionDefault": false,
              "OptionsNamingScheme": "CamelCase"
            }
          },
          "Text": {
            "LoadActions": {
              "LoadProviderClassFunctionDefault": "TextElement",
              "OptionsNamingScheme": "CamelCase"
            },
            "options": {
              "inlineToolbar": true,
              "config": {
                "placeholder": "...",
                "preserveBlank": false,
                "allowEnterKeyDown": false,
                "hidePopoverItem": true,
                "hideToolbar": true,
                "wrapElement": "title"
              }
            }
          }
        }
        """;

        EditorTools = Editor.ParseEditorJsonToolOptions(editor_tools);
        EditorConfigurations = JsonNode.Parse("""{ "DefaultBlock": "text", "CodexEditorRedactor" : { "style": { "paddingBottom": "0px", "maxHeight": "64px", "overflow": "hidden" } } }""")?.AsObject() ?? [];

        // If the browser recieves the following error: "Saving failed due to the Error TypeError: Cannot read properties of undefined (reading 'sanitizeConfig')"
        // This is because edtorjs has certain dependencies caused by the `header.inlineToolbar' array values. EditorJS should have the appropriate tools/plugins enabled.
        // This error will also occur because of unsupported blocks in the editor and the whole editor document may need to be reset.
        string editor_tools_02 = """
        {
          "Header": {
            "LoadActions": {
              "OptionsNamingScheme": "CamelCase"
            }
          },
          "LinkTool": {
            "LoadActions": {
              "OptionsNamingScheme": "CamelCase"
            }
          },
          "NestedList": {
            "LoadActions": {
              "OptionsNamingScheme": "CamelCase",
              "OverrideOptionsKey": "list"
            }
          },
          "Marker": {
            "LoadActions": {
              "OptionsNamingScheme": "CamelCase"
            }
          },
          "Checklist": {
            "LoadActions": {
              "OptionsNamingScheme": "CamelCase"
            }
          },
          "CodeTool": {
            "LoadActions": {
              "OptionsNamingScheme": "CamelCase",
              "OverrideOptionsKey": "code"
            }
          },
          "Delimiter": {
            "LoadActions": {
              "OptionsNamingScheme": "CamelCase"
            }
          },
          "SimpleImage": {
            "LoadActions": {
              "OptionsNamingScheme": "CamelCase",
              "OverrideOptionsKey": "image"
            }
          },
          "Embed": {
            "LoadActions": {
              "OptionsNamingScheme": "CamelCase"
            },
            "options": {
              "config": {
                "services": {
                  "instagram": true,
                  "youtube": true,
                  "vimeo": true,
                  "imgur": true,
                  "twitter": true,
                  "facebook": true
                }
              }
            }
          },
          "InlineCode": {
            "LoadActions": {
              "OptionsNamingScheme": "CamelCase"
            }
          },
          "Quote": {
            "LoadActions": {
              "OptionsNamingScheme": "CamelCase"
            }
          },
          "Table": {
            "LoadActions": {
              "OptionsNamingScheme": "CamelCase"
            }
          }
        }
        """; // "Warning":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}}

        EditorTools02 = Editor.ParseEditorJsonToolOptions(editor_tools_02);
        EditorValue02 = Editor.CreateEmptyJsonObject();
        EditorConfigurations02 = JsonNode.Parse("""{ "DefaultBlock": "paragraph" }""")?.AsObject() ?? [];

    }

    public async Task CheckEditorValueAsync()
    {
        JsonElement? json_element = EditorValue02?.ConvertToJsonElement();
        string? unicode_decoded_json_string = json_element?.ToString();
        await JSRuntime.InvokeVoidAsync("console.log", unicode_decoded_json_string);
    }
}
