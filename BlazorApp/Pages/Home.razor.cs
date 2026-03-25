using EditorJs;
using EditorJs.Builders;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BlazorApp.Pages;
public partial class Home : ComponentBase
{
    [Inject] public required IJSRuntime JSRuntime { get; init; }
    [Inject] public required ProvisioningMonitor ProvisioningMonitor { get; init; }

    public Editor? Editor { get; set; }

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

    public Editor? EditorRtl { get; set; }

    public JsonObject? EditorValueRtl { get; set; }
    public JsonObject EditorToolsRtl { get; set; } = default!;
    public JsonObject EditorConfigurationsRtl { get; set; } = default!;
    public Task OnEditorValueRtlChanged(JsonObject value) => Task.FromResult(EditorValueRtl = value);

    public Editor? EditorShowcase { get; set; }

    public JsonObject? EditorValueShowcase { get; set; }
    public JsonObject EditorToolsShowcase { get; set; } = default!;
    public JsonObject EditorConfigurationsShowcase { get; set; } = default!;
    public Task OnEditorValueShowcaseChanged(JsonObject value) => Task.FromResult(EditorValueShowcase = value);

    public Editor? EditorExternal { get; set; }

    public JsonObject? EditorValueExternal { get; set; }
    public JsonObject EditorToolsExternal { get; set; } = default!;
    public JsonObject EditorConfigurationsExternal { get; set; } = default!;
    public Task OnEditorValueExternalChanged(JsonObject value) => Task.FromResult(EditorValueExternal = value);

    private bool _editors_have_initialised;

    protected override void OnAfterRender(bool first_render)
    {
        if (_editors_have_initialised is true)
        {
            return;
        }

        bool editors_have_initialised = ProvisioningMonitor.CheckEditorsProvisioningStates(ProvisioningMonitor.InitialisedState);
        if (editors_have_initialised is false)
        {
            StateHasChanged();
            return;
        }

        _editors_have_initialised = true;
        Editor02?.Focus(true);
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

    protected Task EnterKeyDownCaptureAsync(JsonObject json_obj)
        => Editor is null ? Task.CompletedTask : Editor.Focus(true);

    protected override void OnInitialized()
    {
        EditorValue = Editor.ParseJsonEditorValue("""{"time": 1717207275445, "blocks": [{"id": "qDEsgkmbL1", "data": {"text": "Heylo, World!", "wrap": "title"}, "type": "text"}], "version": "2.29.1"}""");

        EditorTools = EditorToolsBuilder.Create()
            .DisabledParagraph()
            .TextElement(options =>
            {
                options["inlineToolbar"] = true;
                options["config"] = new JsonObject
                {
                    ["placeholder"] = "...",
                    ["preserveBlank"] = false,
                    ["allowEnterKeyDown"] = false,
                    ["hidePopoverItem"] = true,
                    ["hideToolbar"] = true,
                    ["wrapElement"] = "title",
                    ["startMarginZero"] = true
                };
            })
            .Build();

        EditorConfigurations = EditorConfigBuilder.Create()
            .DefaultBlock("text")
            .CodexEditorRedactor(style =>
            {
                style["paddingBottom"] = "0px";
                style["maxHeight"] = "64px";
            })
            .Build();

        // If the browser receives the following error: "Saving failed due to the Error TypeError: Cannot read properties of undefined (reading 'sanitizeConfig')"
        // This is because EditorJS has certain dependencies caused by the `header.inlineToolbar` array values. EditorJS should have the appropriate tools/plugins enabled.
        // This error will also occur because of unsupported blocks in the editor and the whole editor document may need to be reset.
        EditorTools02 = EditorToolsBuilder.Create()
            .Header()
            .NestedList()
            .Marker()
            .Checklist()
            .CodeTool()
            .Delimiter()
            .SimpleImage()
            .Embed(options => options["config"] = new JsonObject
            {
                ["services"] = new JsonObject
                {
                    ["instagram"] = true,
                    ["youtube"] = true,
                    ["vimeo"] = true,
                    ["imgur"] = true,
                    ["twitter"] = true,
                    ["facebook"] = true
                }
            })
            .InlineCode()
            .Quote()
            .Table()
            .Build();

        EditorValue02 = Editor.CreateEmptyJsonObject();

        EditorConfigurations02 = EditorConfigBuilder.Create()
            .DefaultBlock("paragraph")
            .Build();

        EditorToolsRtl = EditorToolsBuilder.Create()
            .Header()
            .NestedList()
            .Marker()
            .Quote()
            .Delimiter()
            .Build();

        EditorValueRtl = Editor.ParseJsonEditorValue("""{"time": 1717207275445, "blocks": [{"id": "gkrqDlEts1", "data": {"text": "مرحبا بالعالم"}, "type": "paragraph"}], "version": "2.29.1"}""");

        EditorConfigurationsRtl = EditorConfigBuilder.Create()
            .DefaultBlock("paragraph")
            .Rtl()
            .Build();

        // Showcase editor — all available plugins with pre-populated sample data
        EditorToolsShowcase = EditorToolsBuilder.Rich()
            .Header(options =>
            {
                options["inlineToolbar"] = true;
                options["config"] = new JsonObject
                {
                    ["placeholder"] = "Enter a header",
                    ["levels"] = new JsonArray(1, 2, 3, 4, 5, 6),
                    ["defaultLevel"] = 2
                };
            })
            .NestedList(options =>
            {
                options["inlineToolbar"] = true;
                options["config"] = new JsonObject { ["defaultStyle"] = "unordered" };
            })
            .Checklist(options => options["inlineToolbar"] = true)
            .CodeTool(options => options["config"] = new JsonObject { ["placeholder"] = "Enter code here..." })
            .Quote(options =>
            {
                options["inlineToolbar"] = true;
                options["config"] = new JsonObject
                {
                    ["quotePlaceholder"] = "Enter a quote",
                    ["captionPlaceholder"] = "Quote author"
                };
            })
            .Table(options =>
            {
                options["inlineToolbar"] = true;
                options["config"] = new JsonObject
                {
                    ["rows"] = 2, ["cols"] = 3, ["withHeadings"] = true
                };
            })
            .Embed(options =>
            {
                options["inlineToolbar"] = true;
                options["config"] = new JsonObject
                {
                    ["services"] = new JsonObject
                    {
                        ["youtube"] = true, ["vimeo"] = true,
                        ["instagram"] = true, ["twitter"] = true,
                        ["facebook"] = true, ["imgur"] = true
                    }
                };
            })
            .RawTool(options =>
            {
                options["config"] = new JsonObject { ["placeholder"] = "Enter raw HTML..." };
            })
            .Build();

        EditorConfigurationsShowcase = EditorConfigBuilder.Create()
            .DefaultBlock("paragraph")
            .Build();

        string showcase_value = """
        {
          "time": 1717207275445,
          "version": "2.31.5",
          "blocks": [
            {
              "id": "sh_h1_001",
              "type": "header",
              "data": { "text": "EditorJS Plugin Showcase", "level": 1 }
            },
            {
              "id": "sh_p_001",
              "type": "paragraph",
              "data": { "text": "This editor demonstrates every available plugin with <mark class=\"cdx-marker\">highlighted text</mark>, <code class=\"inline-code\">inline code</code>, and <u class=\"cdx-underline\">underlined text</u> formatting." }
            },

            {
              "id": "sh_dlm_001",
              "type": "delimiter",
              "data": {}
            },

            {
              "id": "sh_h2_001",
              "type": "header",
              "data": { "text": "Headers (Levels 1\u20136)", "level": 2 }
            },
            {
              "id": "sh_h3_001",
              "type": "header",
              "data": { "text": "This is a level 3 heading", "level": 3 }
            },
            {
              "id": "sh_h4_001",
              "type": "header",
              "data": { "text": "This is a level 4 heading", "level": 4 }
            },
            {
              "id": "sh_h5_001",
              "type": "header",
              "data": { "text": "This is a level 5 heading", "level": 5 }
            },
            {
              "id": "sh_h6_001",
              "type": "header",
              "data": { "text": "This is a level 6 heading", "level": 6 }
            },

            {
              "id": "sh_dlm_002",
              "type": "delimiter",
              "data": {}
            },

            {
              "id": "sh_h2_002",
              "type": "header",
              "data": { "text": "Paragraphs & Inline Formatting", "level": 2 }
            },
            {
              "id": "sh_p_002",
              "type": "paragraph",
              "data": { "text": "A standard paragraph with <b>bold</b> and <i>italic</i> text. You can also combine them: <b><i>bold italic</i></b>." }
            },
            {
              "id": "sh_p_003",
              "type": "paragraph",
              "data": { "text": "Use the <mark class=\"cdx-marker\">Marker tool</mark> to highlight important phrases in any paragraph." }
            },
            {
              "id": "sh_p_004",
              "type": "paragraph",
              "data": { "text": "The <code class=\"inline-code\">InlineCode</code> tool wraps text in a monospace style \u2014 useful for referencing <code class=\"inline-code\">variable_names</code>, <code class=\"inline-code\">function()</code> calls, or <code class=\"inline-code\">CLI commands</code>." }
            },
            {
              "id": "sh_p_005",
              "type": "paragraph",
              "data": { "text": "The <u class=\"cdx-underline\">Underline tool</u> adds emphasis without using bold or italic, handy for <u class=\"cdx-underline\">proper nouns</u> or <u class=\"cdx-underline\">key terminology</u>." }
            },

            {
              "id": "sh_dlm_003",
              "type": "delimiter",
              "data": {}
            },

            {
              "id": "sh_h2_003",
              "type": "header",
              "data": { "text": "Nested Lists", "level": 2 }
            },
            {
              "id": "sh_p_006",
              "type": "paragraph",
              "data": { "text": "An unordered list with nested items:" }
            },
            {
              "id": "sh_lst_001",
              "type": "list",
              "data": {
                "style": "unordered",
                "items": [
                  {
                    "content": "Frontend frameworks",
                    "items": [
                      { "content": "Blazor (WebAssembly & Server)", "items": [] },
                      { "content": "React", "items": [] },
                      { "content": "Vue.js", "items": [] }
                    ]
                  },
                  {
                    "content": "Backend technologies",
                    "items": [
                      { "content": ".NET / ASP.NET Core", "items": [] },
                      { "content": "Node.js / Express", "items": [] }
                    ]
                  },
                  {
                    "content": "Databases",
                    "items": [
                      { "content": "PostgreSQL", "items": [] },
                      { "content": "SQLite", "items": [] },
                      { "content": "Redis (caching)", "items": [] }
                    ]
                  }
                ]
              }
            },
            {
              "id": "sh_p_007",
              "type": "paragraph",
              "data": { "text": "An ordered list with steps:" }
            },
            {
              "id": "sh_lst_002",
              "type": "list",
              "data": {
                "style": "ordered",
                "items": [
                  { "content": "Clone the repository", "items": [] },
                  {
                    "content": "Install dependencies",
                    "items": [
                      { "content": "Run <code class=\"inline-code\">dotnet restore</code>", "items": [] },
                      { "content": "Run <code class=\"inline-code\">libman restore</code>", "items": [] }
                    ]
                  },
                  { "content": "Build the solution with <code class=\"inline-code\">dotnet build</code>", "items": [] },
                  { "content": "Launch the BlazorApp project", "items": [] }
                ]
              }
            },

            {
              "id": "sh_dlm_004",
              "type": "delimiter",
              "data": {}
            },

            {
              "id": "sh_h2_004",
              "type": "header",
              "data": { "text": "Checklist", "level": 2 }
            },
            {
              "id": "sh_chk_001",
              "type": "checklist",
              "data": {
                "items": [
                  { "text": "Integrate EditorJS into Blazor component", "checked": true },
                  { "text": "Configure tool plugins via JSON", "checked": true },
                  { "text": "Add RTL support", "checked": true },
                  { "text": "Bundle all plugins into a single JS file", "checked": true },
                  { "text": "Add read-only toggle support", "checked": true },
                  { "text": "Write unit tests for interop layer", "checked": false },
                  { "text": "Publish NuGet package", "checked": false }
                ]
              }
            },

            {
              "id": "sh_dlm_005",
              "type": "delimiter",
              "data": {}
            },

            {
              "id": "sh_h2_005",
              "type": "header",
              "data": { "text": "Code Blocks", "level": 2 }
            },
            {
              "id": "sh_p_008",
              "type": "paragraph",
              "data": { "text": "A C# code sample:" }
            },
            {
              "id": "sh_code_001",
              "type": "code",
              "data": { "code": "public static JsonObject ParseJsonEditorValue(string json)\n    => JsonNode.Parse(json)?.AsObject() ?? [];" }
            },
            {
              "id": "sh_p_009",
              "type": "paragraph",
              "data": { "text": "A JavaScript code sample:" }
            },
            {
              "id": "sh_code_002",
              "type": "code",
              "data": { "code": "const editor = new EditorJS({\n  holder: 'editorjs',\n  tools: {\n    header: Header,\n    list: NestedList,\n    quote: Quote\n  },\n  data: savedData\n});" }
            },
            {
              "id": "sh_p_010",
              "type": "paragraph",
              "data": { "text": "An HTML snippet:" }
            },
            {
              "id": "sh_code_003",
              "type": "code",
              "data": { "code": "<Editor @ref=\"Editor\"\n        Id=\"my-editor\"\n        Value=\"EditorValue\"\n        Tools=\"EditorTools\"\n        Configurations=\"EditorConfigs\"\n        ValueChanged=\"OnValueChanged\" />" }
            },

            {
              "id": "sh_dlm_006",
              "type": "delimiter",
              "data": {}
            },

            {
              "id": "sh_h2_006",
              "type": "header",
              "data": { "text": "Quotes", "level": 2 }
            },
            {
              "id": "sh_qt_001",
              "type": "quote",
              "data": { "text": "The best way to predict the future is to invent it.", "caption": "Alan Kay", "alignment": "left" }
            },
            {
              "id": "sh_qt_002",
              "type": "quote",
              "data": { "text": "Simplicity is the ultimate sophistication.", "caption": "Leonardo da Vinci", "alignment": "center" }
            },

            {
              "id": "sh_dlm_007",
              "type": "delimiter",
              "data": {}
            },

            {
              "id": "sh_h2_007",
              "type": "header",
              "data": { "text": "Tables", "level": 2 }
            },
            {
              "id": "sh_p_011",
              "type": "paragraph",
              "data": { "text": "A table with headings:" }
            },
            {
              "id": "sh_tbl_001",
              "type": "table",
              "data": {
                "withHeadings": true,
                "stretched": false,
                "content": [
                  ["Plugin", "Type", "Description"],
                  ["Header", "Block", "Heading levels 1\u20136"],
                  ["NestedList", "Block", "Ordered and unordered lists with nesting"],
                  ["Checklist", "Block", "To-do style checkbox items"],
                  ["Quote", "Block", "Blockquote with caption and alignment"],
                  ["Table", "Block", "Tabular data with optional headings"],
                  ["Code", "Block", "Monospace code block"],
                  ["Delimiter", "Block", "Visual separator between sections"],
                  ["Warning", "Block", "Title and message callout"],
                  ["Raw HTML", "Block", "Arbitrary HTML content"],
                  ["SimpleImage", "Block", "Image from URL with caption"],
                  ["Embed", "Block", "YouTube, Vimeo, and social media embeds"],
                  ["Marker", "Inline", "Highlighted text"],
                  ["InlineCode", "Inline", "Monospace inline snippet"],
                  ["Underline", "Inline", "Underlined text"]
                ]
              }
            },
            {
              "id": "sh_p_012",
              "type": "paragraph",
              "data": { "text": "A stretched table without headings:" }
            },
            {
              "id": "sh_tbl_002",
              "type": "table",
              "data": {
                "withHeadings": false,
                "stretched": true,
                "content": [
                  ["Q1", "Revenue: $1.2M", "Growth: 12%"],
                  ["Q2", "Revenue: $1.5M", "Growth: 25%"],
                  ["Q3", "Revenue: $1.8M", "Growth: 20%"],
                  ["Q4", "Revenue: $2.1M", "Growth: 17%"]
                ]
              }
            },

            {
              "id": "sh_dlm_008",
              "type": "delimiter",
              "data": {}
            },

            {
              "id": "sh_h2_008",
              "type": "header",
              "data": { "text": "Warnings", "level": 2 }
            },
            {
              "id": "sh_wrn_001",
              "type": "warning",
              "data": { "title": "Breaking Change", "message": "The Rtl parameter has been moved from a component attribute into the Configurations JsonObject. Update your code accordingly." }
            },
            {
              "id": "sh_wrn_002",
              "type": "warning",
              "data": { "title": "Browser Compatibility", "message": "EditorJS requires a modern browser with ES2015+ support. Internet Explorer is not supported." }
            },

            {
              "id": "sh_dlm_009",
              "type": "delimiter",
              "data": {}
            },

            {
              "id": "sh_h2_009",
              "type": "header",
              "data": { "text": "Raw HTML", "level": 2 }
            },
            {
              "id": "sh_raw_001",
              "type": "raw",
              "data": { "html": "<div style=\"padding: 1rem; background: #f0f4f8; border-left: 4px solid #3182ce; border-radius: 4px;\">\n  <strong>Custom HTML Block</strong>\n  <p style=\"margin: 0.5rem 0 0 0;\">This is rendered from raw HTML. Useful for custom callouts, badges, or embedded widgets that other block types cannot express.</p>\n</div>" }
            },
            {
              "id": "sh_raw_002",
              "type": "raw",
              "data": { "html": "<details>\n  <summary style=\"cursor: pointer; font-weight: bold;\">Click to expand \u2014 Collapsible Section</summary>\n  <p style=\"padding: 0.5rem 0;\">This uses the native HTML <code>&lt;details&gt;</code> and <code>&lt;summary&gt;</code> elements. No JavaScript required.</p>\n</details>" }
            },

            {
              "id": "sh_dlm_010",
              "type": "delimiter",
              "data": {}
            },

            {
              "id": "sh_h2_010",
              "type": "header",
              "data": { "text": "Images (SimpleImage)", "level": 2 }
            },
            {
              "id": "sh_img_001",
              "type": "image",
              "data": {
                "url": "https://placehold.co/600x200/e2e8f0/475569?text=SimpleImage+with+Border",
                "caption": "A bordered image \u2014 using placehold.co",
                "withBorder": true,
                "withBackground": false,
                "stretched": false
              }
            },
            {
              "id": "sh_img_002",
              "type": "image",
              "data": {
                "url": "https://placehold.co/800x300/f0fdf4/166534?text=Stretched+Image+with+Background",
                "caption": "A stretched image with background \u2014 using placehold.co",
                "withBorder": false,
                "withBackground": true,
                "stretched": true
              }
            },

            {
              "id": "sh_dlm_011",
              "type": "delimiter",
              "data": {}
            },

            {
              "id": "sh_h2_011",
              "type": "header",
              "data": { "text": "Embeds", "level": 2 }
            },
            {
              "id": "sh_p_013",
              "type": "paragraph",
              "data": { "text": "The Embed plugin supports YouTube, Vimeo, Instagram, Twitter, Facebook, and Imgur. Paste a supported URL into a new block to create an embed." }
            },
            {
              "id": "sh_emb_001",
              "type": "embed",
              "data": {
                "service": "youtube",
                "source": "https://www.youtube.com/watch?v=jNQXAC9IVRw",
                "embed": "https://www.youtube.com/embed/jNQXAC9IVRw",
                "width": 580,
                "height": 320,
                "caption": "YouTube embed \u2014 \"Me at the zoo\" (first ever YouTube video)"
              }
            },
            {
              "id": "sh_emb_002",
              "type": "embed",
              "data": {
                "service": "instagram",
                "source": "https://www.instagram.com/p/CUbHfhpswxt/",
                "embed": "https://www.instagram.com/p/CUbHfhpswxt/embed",
                "width": 400,
                "height": 505,
                "caption": "Instagram photo post embed"
              }
            },
            {
              "id": "sh_emb_003",
              "type": "embed",
              "data": {
                "service": "vimeo",
                "source": "https://vimeo.com/427943407",
                "embed": "https://player.vimeo.com/video/427943407?title=0&byline=0",
                "width": 580,
                "height": 320,
                "caption": "Vimeo embed example"
              }
            },

            {
              "id": "sh_dlm_012",
              "type": "delimiter",
              "data": {}
            },

            {
              "id": "sh_h2_012",
              "type": "header",
              "data": { "text": "Delimiters", "level": 2 }
            },
            {
              "id": "sh_p_014",
              "type": "paragraph",
              "data": { "text": "The three-star delimiter above and below this paragraph separates sections visually. They produce no visible text, just a horizontal break between content areas." }
            },
            {
              "id": "sh_dlm_013",
              "type": "delimiter",
              "data": {}
            }
          ]
        }
        """;

        EditorValueShowcase = JsonNode.Parse(showcase_value)?.AsObject() ?? [];

        // External plugins editor — paragraph + quote base with three external CDN plugins
        EditorToolsExternal = EditorToolsBuilder.Create()
            .Quote(options => options["inlineToolbar"] = true)
            .Tool("Alert", "Alert", configure: options => options["config"] = new JsonObject
            {
                ["defaultType"] = "info",
                ["messagePlaceholder"] = "Enter alert message..."
            })
            .Tool("Color", "ColorPlugin", naming_scheme: NamingScheme.PascalCase, configure: options =>
            {
                options["config"] = new JsonObject
                {
                    ["type"] = "text",
                    ["defaultColor"] = "#d63031"
                };
            })
            .Tool("ColorMarker", "ColorPlugin", override_options_key: "ColorMarker", naming_scheme: NamingScheme.PascalCase, configure: options =>
            {
                options["config"] = new JsonObject
                {
                    ["type"] = "marker",
                    ["defaultColor"] = "#fdcb6e"
                };
            })
            .Tool("Strikethrough", "Strikethrough")
            .Build();

        EditorConfigurationsExternal = EditorConfigBuilder.Create()
            .DefaultBlock("paragraph")
            .Build();

        string external_value = """
        {
          "time": 1717207275445,
          "version": "2.31.5",
          "blocks": [
            {
              "id": "ext_h_001",
              "type": "paragraph",
              "data": { "text": "This editor uses three external plugins loaded from CDN alongside the bundled paragraph and quote tools." }
            },
            {
              "id": "ext_alert_001",
              "type": "alert",
              "data": { "type": "info", "align": "left", "text": "This is an <b>info</b> alert created by the <code>editorjs-alert</code> plugin." }
            },
            {
              "id": "ext_alert_002",
              "type": "alert",
              "data": { "type": "success", "align": "left", "text": "A <b>success</b> alert \u2014 useful for confirmation messages." }
            },
            {
              "id": "ext_alert_003",
              "type": "alert",
              "data": { "type": "warning", "align": "left", "text": "A <b>warning</b> alert \u2014 draws attention to important caveats." }
            },
            {
              "id": "ext_alert_004",
              "type": "alert",
              "data": { "type": "danger", "align": "left", "text": "A <b>danger</b> alert \u2014 signals critical errors or breaking changes." }
            },
            {
              "id": "ext_p_002",
              "type": "paragraph",
              "data": { "text": "The <code>editorjs-text-color-plugin</code> registers the same <code>ColorPlugin</code> class twice with different configs \u2014 once as a text colour tool and once as a colour marker. Select text in any paragraph to see both in the inline toolbar." }
            },
            {
              "id": "ext_p_003a",
              "type": "paragraph",
              "data": { "text": "This plugin uses <b>PascalCase</b> naming scheme via <code>NamingScheme.PascalCase</code> so the tool keys (<code>Color</code>, <code>ColorMarker</code>) are preserved as-is rather than being converted to camelCase." }
            },
            {
              "id": "ext_p_003",
              "type": "paragraph",
              "data": { "text": "Select text in any paragraph to see the <s>Strikethrough</s> inline tool in the toolbar." }
            },
            {
              "id": "ext_quote_001",
              "type": "quote",
              "data": { "text": "External plugins integrate seamlessly with the builder API via the <code>Tool()</code> method.", "caption": "blazor-editorjs", "alignment": "left" }
            },
            {
              "id": "ext_quote_002",
              "type": "quote",
              "data": { "text": "Any EditorJS plugin that exposes a global class can be loaded from a CDN and registered without modifying the library.", "caption": "extensibility by design", "alignment": "left" }
            }
          ]
        }
        """;

        EditorValueExternal = JsonNode.Parse(external_value)?.AsObject() ?? [];

    }

    private bool _editor_02_read_only;

    public async Task ToggleEditor02ReadOnlyAsync()
    {
        if (Editor02 is null)
        {
            return;
        }

        _editor_02_read_only = !_editor_02_read_only;
        await Editor02.ToggleAsync(_editor_02_read_only);
    }

    public async Task CheckEditorValueAsync()
    {
        JsonElement? json_element = EditorValue02?.ConvertToJsonElement();
        string? unicode_decoded_json_string = json_element?.ToString();
        await JSRuntime.InvokeVoidAsync("console.log", unicode_decoded_json_string);
    }

    public async Task CheckEditorValueRtlAsync()
    {
        JsonElement? json_element = EditorValueRtl?.ConvertToJsonElement();
        string? unicode_decoded_json_string = json_element?.ToString();
        await JSRuntime.InvokeVoidAsync("console.log", unicode_decoded_json_string);
    }
}
