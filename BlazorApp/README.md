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

For more advanced confgurations, look into the sample code.
- In the `Index.razor.cs` file, there is a example using the `embed` tool/plugin demostrating who to pass configurations into the editorjs library.


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
