# Basic Blazor App (2022)

This is a basic blazor app created in Visual Studio 2022.


```csharp
@using EditorJS
<Editor Id="editorjs-blazor" Name="editorjs-blazor" Value="EditorValue" ValueChanged="OnEditorValueChanged" Style="margin-top: 20px; border: thin dashed grey; padding: 0 20px 0 20px;" />
```


```html
    <script src="/_content/EditorJs/lib/editorjs/editorjs/dist/editor.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/checklist/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/code/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/delimiter/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/embed/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/header/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/image/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/inline-code/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/list/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/marker/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/quote/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/simple-image/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/table/dist/table.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/editorjs/warning/dist/bundle.js" asp-append-version="true"></script></script>
```
