# Basic Blazor App (2022)

This is  a basic blazor app created in Visual Studio 2022.

The only piece of code entered here is in the Pages/Index.razor

```csharp
@using EditorJS
<Editor Id="editorjs-blazor" Name="editorjs-blazor" @bind-Value="@EditorValue" Style="margin-top: 20px; border: thin dashed grey; padding: 0 20px 0 20px;" />
```


```javascript
    <script src="/_content/EditorJs/lib/editorjs/dist/editor.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/codex.editor.checklist/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/codex.editor.delimiter/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/codex.editor.embed/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/codex.editor.header/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/codex.editor.image/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/codex.editor.list/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/codex.editor.marker/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/codex.editor.quote/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/codex.editor.simple-image/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/codex.editor.table/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJs/lib/codex.editor.warning/dist/bundle.js" asp-append-version="true"></script>
```