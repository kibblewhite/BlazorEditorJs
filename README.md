[![GitHub Repo](https://img.shields.io/badge/GitHub-Repo-green?logo=github&style=flat-square)](https://github.com/kibblewhite/BlazorEditorJs)
[![GitHub Licence](https://img.shields.io/github/license/kibblewhite/BlazorEditorJs?logo=github&style=flat-square)](https://github.com/kibblewhite/BlazorEditorJs/blob/main/LICENSE)
[![GitHub Stars](https://img.shields.io/github/stars/kibblewhite/BlazorEditorJs?style=flat-square&logo=github)](https://github.com/kibblewhite/BlazorEditorJs/stargazers)
[![Nuget Version](https://img.shields.io/nuget/v/EditorJs?label=nuget%20version&logo=nuget&style=flat-square)](https://www.nuget.org/packages/EditorJs/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/EditorJs?label=nuget%20downloads&logo=nuget&style=flat-square)](https://www.nuget.org/packages/EditorJs/)
[![Live Demo](https://img.shields.io/badge/Live-Demo-blue?style=flat-square&logo=blazor)](https://kibblewhite.github.io/BlazorEditorJs/)

# blazor-editorjs

A Blazor component library wrapping [EditorJS](https://editorjs.io/) — a block-styled editor with clean JSON output. This package provides an `<Editor />` component, a fluent builder API for tool and editor configuration, JavaScript interop for full editor lifecycle control, and provisioning state tracking for multi-editor pages.

### [Try the Live Demo](https://kibblewhite.github.io/BlazorEditorJs/)

## Packages

| Package | Description |
|---|---|
| **EditorJs** | The Blazor `<Editor />` component, JS interop, provisioning monitor, and all bundled EditorJS plugins. References `EditorJs.Configuration` automatically. |
| **EditorJs.Configuration** | Standalone builder API (`EditorToolsBuilder`, `EditorConfigBuilder`, `NamingScheme`) and `JsonObjectExtensions`. Use this package independently when you only need to build tool/config JSON without the Blazor component — for example, in a shared library or server-side code. |

## Installation

Install the **EditorJs** NuGet package (this also brings in **EditorJs.Configuration** as a dependency):

```
dotnet add package EditorJs
```

Register the required services in `Program.cs`:

```csharp
builder.Services.AddScopedEditorJsInterop();
```

This registers `EditorJsInterop` (scoped) and `ProvisioningMonitor` (transient).

Include the bundled EditorJS scripts in your `index.html` (WebAssembly) or `App.razor` (Server):

```html
<script src="_content/EditorJs/lib/editorjs-bundle.js"></script>
```

The bundle includes EditorJS core and all bundled plugins. Alternatively, load only the plugins you need:

```html
<script src="_content/EditorJs/lib/editorjs/editorjs/dist/editorjs.umd.min.js"></script>
<script src="_content/EditorJs/lib/editorjs/header/dist/header.umd.min.js"></script>
<!-- ... additional plugins as needed -->
```

## Quick Start

```razor
@using EditorJs
@using EditorJs.Builders

<Editor @ref="editor"
        Id="my-editor"
        Value="editorValue"
        ValueChanged="OnValueChanged"
        Tools="editorTools"
        Configurations="editorConfig"
        ProvisioningCallbacks="provisioningMonitor.EventsCallbacksHandlerAsync" />

@code {
    [Inject] public required ProvisioningMonitor provisioningMonitor { get; init; }

    private Editor? editor;
    private JsonObject editorValue = Editor.CreateEmptyJsonObject();

    private JsonObject editorTools = EditorToolsBuilder.Text().Build();

    private JsonObject editorConfig = EditorConfigBuilder.Create()
        .DefaultBlock("paragraph")
        .Build();

    private Task OnValueChanged(JsonObject value)
        => Task.FromResult(editorValue = value);
}
```

## Editor Component Parameters

| Parameter | Type | Required | Description |
|---|---|---|---|
| `Id` | `string` | Yes | Unique identifier for the editor instance. |
| `Tools` | `JsonObject` | Yes | Tool plugin configurations — use `EditorToolsBuilder` or raw JSON. |
| `Configurations` | `JsonObject` | Yes | Editor-level settings — use `EditorConfigBuilder` or raw JSON. |
| `Value` | `JsonObject` | No | Current editor content as an EditorJS data object. |
| `ValueChanged` | `EventCallback<JsonObject>` | No | Fires when editor content changes. |
| `EnterKeyDownCapture` | `EventCallback<JsonObject>` | No | Fires when Enter is pressed (for single-line editors using `TextElement`). |
| `ProvisioningCallbacks` | `EventCallback<ProvisioningStatus>` | No | Tracks editor initialisation lifecycle. |
| `Name` | `string?` | No | HTML `name` attribute on the holder div. |
| `Style` | `string?` | No | Inline CSS styles on the holder div. |
| `Class` | `string?` | No | CSS classes on the holder div. |
| `Title` | `string?` | No | HTML `title` attribute on the holder div. |

## Building Tool Configurations

The `EditorToolsBuilder` provides a fluent API that handles the internal `LoadActions` and `OptionsNamingScheme` boilerplate for you.

### Presets

```csharp
// Common text editing tools
JsonObject tools = EditorToolsBuilder.Text().Build();

// All bundled plugins
JsonObject tools = EditorToolsBuilder.Rich().Build();
```

**Text preset** includes: Header, NestedList, Quote, Delimiter, Marker, InlineCode.

**Rich preset** includes: Header, NestedList, Checklist, CodeTool, Quote, Table, SimpleImage, Embed, Delimiter, Warning, RawTool, Marker, InlineCode, Underline.

### Custom Composition

Pick only the tools you need:

```csharp
JsonObject tools = EditorToolsBuilder.Create()
    .Header()
    .NestedList()
    .Quote()
    .Table()
    .Marker()
    .InlineCode()
    .Build();
```

### Configuring Individual Tools

Each tool method accepts an optional `Action<JsonObject>` to set tool-specific options:

```csharp
JsonObject tools = EditorToolsBuilder.Create()
    .Header(options =>
    {
        options["inlineToolbar"] = true;
        options["config"] = new JsonObject
        {
            ["placeholder"] = "Enter a header",
            ["levels"] = new JsonArray(1, 2, 3, 4),
            ["defaultLevel"] = 2
        };
    })
    .Embed(options =>
    {
        options["config"] = new JsonObject
        {
            ["services"] = new JsonObject
            {
                ["youtube"] = true,
                ["vimeo"] = true
            }
        };
    })
    .Build();
```

### Extending Presets

Calling a tool method on a preset overwrites that tool's entry, allowing selective customisation:

```csharp
JsonObject tools = EditorToolsBuilder.Rich()
    .Header(options =>
    {
        options["config"] = new JsonObject { ["defaultLevel"] = 3 };
    })
    .Build();
```

### Single-Line Text Editor

Use `TextElement` with `DisabledParagraph` for constrained single-line inputs (titles, labels, chat fields):

```csharp
JsonObject tools = EditorToolsBuilder.Create()
    .DisabledParagraph()
    .TextElement(options =>
    {
        options["inlineToolbar"] = true;
        options["config"] = new JsonObject
        {
            ["placeholder"] = "Enter title...",
            ["hideToolbar"] = true,
            ["hidePopoverItem"] = true,
            ["allowEnterKeyDown"] = false,
            ["wrapElement"] = "title"
        };
    })
    .Build();
```

`DisabledParagraph()` registers a no-op replacement for the built-in EditorJS paragraph, preventing console warnings when the default paragraph is not loaded.

### Available Tool Methods

| Method | EditorJS Plugin | Notes |
|---|---|---|
| `Header()` | @editorjs/header | Heading levels 1-6 |
| `NestedList()` | @editorjs/nested-list | Ordered/unordered with nesting |
| `Checklist()` | @editorjs/checklist | Checkbox items |
| `CodeTool()` | @editorjs/code | Monospace code blocks |
| `Quote()` | @editorjs/quote | Blockquote with caption and alignment |
| `Table()` | @editorjs/table | Tabular data with optional headings |
| `SimpleImage()` | @editorjs/simple-image | Image from URL with caption |
| `Embed()` | @editorjs/embed | YouTube, Vimeo, Instagram, Twitter, Facebook, Imgur |
| `Delimiter()` | @editorjs/delimiter | Visual separator between sections |
| `Warning()` | @editorjs/warning | Title + message callout |
| `RawTool()` | @editorjs/raw | Raw HTML block |
| `Marker()` | @editorjs/marker | Highlighted inline text |
| `InlineCode()` | @editorjs/inline-code | Monospace inline text |
| `Underline()` | @editorjs/underline | Underlined inline text |
| `TextElement()` | editorjs-text | Single-line text block |
| `DisabledParagraph()` | editorjs-text | No-op replacement for built-in paragraph |
| `Tool(key, globalClassName)` | Any | Register a custom/external plugin (see below) |

### External Plugins

Third-party EditorJS plugins that are not bundled with the library can be loaded via a `<script>` tag and registered using the `Tool()` method.

**1. Add the plugin script to your `index.html`:**

```html
<script src="_content/EditorJs/lib/editorjs-bundle.js"></script>
<script src="https://cdn.jsdelivr.net/npm/editorjs-alert@latest/dist/bundle.js"></script>
```

**2. Register the plugin with `Tool()`:**

```csharp
JsonObject tools = EditorToolsBuilder.Text()
    .Tool("Alert", "Alert", configure: options =>
    {
        options["config"] = new JsonObject
        {
            ["defaultType"] = "info",
            ["messagePlaceholder"] = "Enter alert message..."
        };
    })
    .Build();
```

The `key` parameter (`"Alert"`) is automatically transformed to camelCase (`"alert"`) for the EditorJS tool registration. Control this with the `naming_scheme` parameter:

```csharp
// Tool key becomes "my_toggle_block" (snake_case)
.Tool("MyToggleBlock", "MyToggleBlock", naming_scheme: NamingScheme.SnakeCase)

// Tool key becomes "my-toggle-block" (kebab-case)
.Tool("MyToggleBlock", "MyToggleBlock", naming_scheme: NamingScheme.KebabCase)

// Tool key stays "MyToggleBlock" (PascalCase)
.Tool("MyToggleBlock", "MyToggleBlock", naming_scheme: NamingScheme.PascalCase)
```

If the plugin expects a specific key that doesn't follow any naming convention, use `override_options_key`:

```csharp
.Tool("MyCustomTool", "MyLibrary.ToolClass", override_options_key: "custom-tool")
```

The `global_class_name` parameter supports dot-notation paths for plugins that nest their class under a namespace (e.g. `"MyLibrary.ToolClass"` resolves to `window.MyLibrary.ToolClass`).

**Registering the same class twice with different configs:**

```csharp
JsonObject tools = EditorToolsBuilder.Create()
    .Tool("Color", "ColorPlugin", naming_scheme: NamingScheme.PascalCase, configure: options =>
    {
        options["config"] = new JsonObject { ["type"] = "text", ["defaultColor"] = "#d63031" };
    })
    .Tool("ColorMarker", "ColorPlugin", override_options_key: "ColorMarker",
        naming_scheme: NamingScheme.PascalCase, configure: options =>
    {
        options["config"] = new JsonObject { ["type"] = "marker", ["defaultColor"] = "#fdcb6e" };
    })
    .Build();
```

## Building Editor Configurations

```csharp
JsonObject config = EditorConfigBuilder.Create()
    .DefaultBlock("paragraph")
    .Build();
```

### Configuration Options

| Method | Description |
|---|---|
| `DefaultBlock(string)` | Block type created when pressing Enter (e.g. `"paragraph"`, `"text"`). |
| `Rtl(bool)` | Enable right-to-left layout and EditorJS i18n direction. |
| `CodexEditorRedactor(Action<JsonObject>)` | Apply inline styles to the editor content area. |

### RTL Editor

```csharp
JsonObject config = EditorConfigBuilder.Create()
    .DefaultBlock("paragraph")
    .Rtl()
    .Build();
```

When `Rtl` is enabled, the editor sets `direction: rtl` on the holder element and configures EditorJS's internal i18n direction.

### Constraining the Content Area

Use `CodexEditorRedactor` to style the `.codex-editor__redactor` element without affecting toolbars or popovers:

```csharp
JsonObject config = EditorConfigBuilder.Create()
    .DefaultBlock("text")
    .CodexEditorRedactor(style =>
    {
        style["paddingBottom"] = "0px";
        style["maxHeight"] = "64px";
    })
    .Build();
```

## Editor Methods

Control the editor programmatically via the `@ref` reference:

```csharp
// Render new content (replaces current blocks)
await editor.RenderAsync(newJsonObject);

// Clear all blocks
await editor.Clear();

// Set focus (optionally to the last block)
await editor.Focus(on_last: true);

// Toggle read-only mode
await editor.ToggleAsync(read_only: true);
```

## Handling the Enter Key

For single-line editors (titles, chat inputs), capture the Enter key to trigger custom behaviour instead of creating a new block:

```razor
<Editor Id="chat-input"
        Tools="chatTools"
        Configurations="chatConfig"
        Value="chatValue"
        ValueChanged="OnChatValueChanged"
        EnterKeyDownCapture="OnEnterKeyDownAsync"
        ProvisioningCallbacks="provisioningMonitor.EventsCallbacksHandlerAsync" />
```

```csharp
private async Task OnEnterKeyDownAsync(JsonObject value)
{
    await SubmitMessage(value);
    await editor.Clear();
    await editor.Focus();
}
```

This works with the `TextElement` plugin's `allowEnterKeyDown: false` config, which suppresses the default Enter behaviour and emits a `block:enter` event instead.

## Pre-Populating with Existing Data

Load saved content by parsing a JSON string into the `Value` parameter:

```csharp
JsonObject editorValue = Editor.ParseJsonEditorValue("""
    {
      "time": 1717207275445,
      "blocks": [
        {
          "id": "abc123",
          "type": "paragraph",
          "data": { "text": "Hello, world!" }
        },
        {
          "id": "def456",
          "type": "header",
          "data": { "text": "A heading", "level": 2 }
        }
      ],
      "version": "2.31.5"
    }
    """);
```

Each block has a `type` matching the tool key (e.g. `"paragraph"`, `"header"`, `"list"`, `"quote"`) and a `data` object specific to that block type. See the [EditorJS output data documentation](https://editorjs.io/saving-data/) for the full format.

## Reading Editor Content

Use `ConvertToJsonElement()` to extract editor content as a serialisable `JsonElement`:

```csharp
JsonElement jsonElement = editorValue.ConvertToJsonElement();
string jsonString = jsonElement.ToString();
```

This extension method (from `EditorJs.Configuration`) removes Unicode escape sequences that `System.Text.Json` introduces, producing clean JSON suitable for API requests or storage.

## Provisioning Lifecycle

Track editor initialisation state using `ProvisioningMonitor`. This is useful when you have multiple editors on a page and need to know when they are all ready:

```csharp
[Inject] public required ProvisioningMonitor ProvisioningMonitor { get; init; }

protected override void OnAfterRender(bool firstRender)
{
    bool ready = ProvisioningMonitor.CheckEditorsProvisioningStates(
        ProvisioningMonitor.InitialisedState
    );

    if (ready)
    {
        // All editors are constructed and initialised — safe to call Focus(), RenderAsync(), etc.
    }
}
```

### Provisioning States

States are bitwise flags that accumulate as the editor progresses through its lifecycle:

| State | Value | Description |
|---|---|---|
| `Constructed` | 2 | Blazor component created, parameters set. |
| `Initialised` | 4 | EditorJS JavaScript instance created, ready for interaction. |
| `Rendered` | 8 | Content has been rendered via `RenderAsync()`. |

Convenience fields on `ProvisioningMonitor`:

| Field | States Included | Description |
|---|---|---|
| `InitialisedState` | Constructed + Initialised | Editor is ready for interaction. |
| `FullState` | Constructed + Initialised + Rendered | Editor has had content rendered into it. |

Use `AreAllEditorsFullyProvisioned()` as a shorthand for checking `FullState` on all tracked editors.

## Static Utilities

```csharp
// Parse a JSON string into an editor value
JsonObject value = Editor.ParseJsonEditorValue(jsonString);

// Create an empty editor document (contains an empty "blocks" array)
JsonObject empty = Editor.CreateEmptyJsonObject();

// Parse raw JSON tool configurations (for backward-compatible JSON approach)
JsonObject tools = Editor.ParseEditorJsonToolOptions(jsonString);
```

## Raw JSON Tool Configuration

If you prefer raw JSON over the fluent builder API, pass a JSON string to `ParseEditorJsonToolOptions()`. Each tool entry requires a `LoadActions` object with at minimum an `OptionsNamingScheme` property:

```csharp
JsonObject tools = Editor.ParseEditorJsonToolOptions("""
    {
      "Header": {
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
      "Quote": {
        "LoadActions": {
          "OptionsNamingScheme": "CamelCase"
        },
        "options": {
          "inlineToolbar": true
        }
      }
    }
    """);
```

### LoadActions Properties

| Property | Type | Description |
|---|---|---|
| `OptionsNamingScheme` | `string` | **Required.** Naming convention for the tool key: `"CamelCase"`, `"PascalCase"`, `"SnakeCase"`, or `"KebabCase"`. |
| `OverrideOptionsKey` | `string?` | Explicit tool key, bypassing the naming scheme transformation entirely. |
| `LoadProviderClassFunctionDefault` | `string?` | Global class name on `window` to use instead of the tool key. Supports dot-notation (e.g. `"TextElement.DisabledParagraph"`). |

**Key mappings for bundled tools** — some EditorJS plugins use a different tool key than their class name:

| Builder Method | Key | OverrideOptionsKey |
|---|---|---|
| `NestedList()` | NestedList | `list` |
| `CodeTool()` | CodeTool | `code` |
| `SimpleImage()` | SimpleImage | `image` |
| `RawTool()` | RawTool | `raw` |

## Bundled Plugins

The library bundles these EditorJS plugins via [LibMan](https://learn.microsoft.com/en-us/aspnet/core/client-side/libman/), concatenated into a single `editorjs-bundle.js` during build:

| Plugin | Version |
|---|---|
| @editorjs/editorjs | 2.31.5 |
| @editorjs/header | 2.8.8 |
| @editorjs/nested-list | 1.4.3 |
| @editorjs/checklist | 1.6.0 |
| @editorjs/code | 2.9.4 |
| @editorjs/quote | 2.7.6 |
| @editorjs/table | 2.4.5 |
| @editorjs/simple-image | 1.6.0 |
| @editorjs/embed | 2.8.0 |
| @editorjs/delimiter | 1.4.2 |
| @editorjs/warning | 1.4.1 |
| @editorjs/raw | 2.5.1 |
| @editorjs/marker | 1.4.0 |
| @editorjs/inline-code | 1.5.2 |
| @editorjs/underline | 1.2.1 |
| @editorjs/paragraph | 2.11.7 |
| @editorjs/link | 2.6.2 |
| @editorjs/image | 2.10.3 |
| @editorjs/attaches | 1.3.2 |
| editorjs-text | 1.0.3 |

## License

MIT
