# Basic Blazor App (2022)

This is a basic blazor app created in Visual Studio 2022.


```csharp
@using EditorJS
<Editor Id="editorjs-blazor" Name="editorjs-blazor" Value="EditorValue" ValueChanged="OnEditorValueChanged" Tools="EditorTools" Style="margin-top: 20px; border: thin dashed grey; padding: 0 20px 0 20px;" />
```


The value `EdtorTools` is a csharp JsonObject containing the following in the code-behind:
```csharp
string editor_tools_json = """
    {"Header":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"LinkTool":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"NestedList":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"list"}},"Marker":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Warning":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Checklist":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"CodeTool":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"code"}},"Delimiter":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"SimpleImage":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"image"}},"Embed":{"LoadActions":{"OptionsNamingScheme":"CamelCase"},"options":{"config":{"services":{"instagram":true,"youtube":true,"vimeo":true,"imgur":true,"twitter":true,"facebook":true}}}},"InlineCode":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Quote":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Table":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}}}
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


### Supporting tool loading options from component


```json
{
    "LinkTool": {
        "LoadActions": {
            "LoadProviderClassFunctionDefault": "LinkTool", // This can just be null or undefined, if you want to use the provider options below. Otherwise this value will override the options by looking in the browser's DOM for that existing value.
            "OptionsNamingScheme": "CamelCase",             // PascalCase and SnakeCase // this will convert the class name, the root name identifier here is "LinkTool", and convert this in the string name that is used as the key for the final configuration options.
            "OverrideOptionsKey": "linkTools"               // When not null this will override the `OptionsNamingScheme` and the value coming in from the root name identifier, and use this exactly how it is defined here.
        },
        "options": null
    }
}
```

With the above config, the output might look a litle like this:
```json
{
    "linkTools": {
        "Class": LinkTool
    }
}
```

---

```json
{
    "List": {
        "LoadActions": {
            "OptionsNamingScheme": "CamelCase"
        },
        "options": {
            "inlineToolbar": true,
            "shortcut": "CMD+SHIFT+L"
        }
    }
}
```

With the above config, the output might look a litle like this:
```json
{
    "list": {
        "Class": List,
        "inlineToolbar": true,
        "shortcut": "CMD+SHIFT+L"
    }
}
```

To see an examples of this, inspect the `Index.razor.cs` file.
