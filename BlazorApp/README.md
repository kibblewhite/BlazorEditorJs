# Basic Blazor App (2022)

This is a basic blazor app created in Visual Studio 2022.


```csharp
@using EditorJS
<Editor Id="editorjs-blazor" Name="editorjs-blazor" Value="EditorValue" ValueChanged="OnEditorValueChanged" Tools="EditorTools" Style="margin-top: 20px; border: thin dashed grey; padding: 0 20px 0 20px;" />
```


The value `EdtorTools` is a csharp JsonObject containing the following in the code-behind:
```csharp
string editor_tools_json = """
    { "header": null, "linkTool": null, "nestedList": null, "marker": null, "warning": null, "checklist": null, "code": null, "delimiter": null, "embed": null, "simpleImage": null, "inlineCode": null, "quote": null, "table": null }
""";
EditorTools = JsonObject.Parse(editor_tools_json)?.AsObject() ?? new();
```

For more advanced configurations, look into the sample code.
- The `Index.razor.cs` file contains an example using the `embed` tool/plugin demostrating how to pass/inject configurations into the editorjs library.


To load all the available plugins (bundled)
```html
    <script src="/_content/EditorJs/lib/editorjs-bundle.js" asp-append-version="true"></script>
```

Or load only the plugins that is required (editorjs/dist/editor.js mandatory)
```html
    <script src="/_content/EditorJs/lib/editorjs/editorjs/dist/editor.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/checklist/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/code/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/delimiter/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/embed/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/header/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/nested-list/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/inline-code/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/marker/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/quote/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/simple-image/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/table/dist/table.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/warning/dist/bundle.js" asp-append-version="true"></script>
```


### Future development (forecasting breaking changes)

When passing in the configuraton options, the key for the options wll be the tool/plugin name, for example `window.Header` will be just `Header`, and this term is used to check in the browser's DOM to return that object.

```js
// untested
let class_fn = window["Header"];
```

The term `Header` will then go through a process of camel casing (to become `header`) in order to identity the tool options by it's key provided by the developer or fall back to defaults if not present.

A better example of camel casing, would be `LinkTool` to `linkTool`

By allowing the tools/plugin to be loaded in this manner, it means any name/class function and options can be loaded and so will allow for custom tooling/plugins.

Be aware that it is uncertain how this will affect the current configurations and may cause breaking changes. In this change, it is possible that:

```json
{ "header": null, "linkTool": null }
```

Will then become:

```json
{ "Header": null, "LinkTool": null }
```

And camel casing may no longer be a requirement and thus, camel casing should just be ignored in order to handle the configuration injection.
