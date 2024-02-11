
[![GitHub Repo](https://img.shields.io/badge/GitHub-Repo-green?logo=github&style=flat-square)](https://github.com/kibblewhite/BlazorEditorJs)
[![GitHub Licence](https://img.shields.io/github/license/kibblewhite/BlazorEditorJs?logo=github&style=flat-square)](https://github.com/kibblewhite/BlazorEditorJs/blob/master/LICENSE)
[![GitHub Stars](https://img.shields.io/github/stars/kibblewhite/BlazorEditorJs?style=flat-square&logo=github)](https://github.com/kibblewhite/BlazorEditorJs/stargazers)
[![Nuget Version](https://img.shields.io/nuget/v/EditorJs?label=nuget%20version&logo=nuget&style=flat-square)](https://www.nuget.org/packages/EditorJs/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/EditorJs?label=nuget%20downloads&logo=nuget&style=flat-square)](https://www.nuget.org/packages/EditorJs/)

# Blazor / EditorJS

A Blazor component implementation for EditorJS.io a block styled editor.

```html
@using EditorJS
<Editor Id="editorjs-blazor" Name="editorjs-blazor" Value="EditorValue" ValueChanged="OnEditorValueChanged" Tools="EditorTools" Style="margin-top: 20px; border: thin dashed grey; padding: 0 20px 0 20px;" />
```


The value `EditorTools` is a csharp `JsonObject` containing the following in the code-behind:
```csharp
string editor_tools_json = """
    {"Header":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"LinkTool":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"NestedList":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"list"}},"Marker":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Warning":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Checklist":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"CodeTool":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"code"}},"Delimiter":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"SimpleImage":{"LoadActions":{"OptionsNamingScheme":"CamelCase","OverrideOptionsKey":"image"}},"Embed":{"LoadActions":{"OptionsNamingScheme":"CamelCase"},"options":{"config":{"services":{"instagram":true,"youtube":true,"vimeo":true,"imgur":true,"twitter":true,"facebook":true}}}},"InlineCode":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Quote":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}},"Table":{"LoadActions":{"OptionsNamingScheme":"CamelCase"}}}
""";
EditorTools = JsonObject.Parse(editor_tools_json)?.AsObject() ?? new();
```

There are more details below on the `LoadAction` property object and what the associated properties mean.

To load all the available plugins (bundled)
```html
    <script src="/_content/EditorJS/lib/editorjs-bundle.js" asp-append-version="true"></script>
```

Or load only the plugins that are required (`editorjs/dist/editor.js` mandatory)
```html
    <script src="/_content/EditorJS/lib/editorjs/editorjs/dist/editor.js" asp-append-version="true"></script>
    <script src="/_content/EditorJS/lib/editorjs/checklist/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJS/lib/editorjs/code/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJS/lib/editorjs/delimiter/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJS/lib/editorjs/embed/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJS/lib/editorjs/header/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJS/lib/editorjs/nested-list/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJS/lib/editorjs/inline-code/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJS/lib/editorjs/marker/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJS/lib/editorjs/quote/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJS/lib/editorjs/simple-image/dist/bundle.js" asp-append-version="true"></script>
    <script src="/_content/EditorJS/lib/editorjs/table/dist/table.js" asp-append-version="true"></script>
    <script src="/_content/EditorJS/lib/editorjs/warning/dist/bundle.js" asp-append-version="true"></script>
```


### Supporting tool loading options from component


```json
{
    "LinkTool": {
        "LoadActions": {

            // The default class function to be loaded from the provider.
            // This can be null or undefined to use the default options.
            // Otherwise, this value will override the options by looking 
            // in the browser's DOM for an existing value.
            "LoadProviderClassFunctionDefault": "LinkTool",

            // The naming scheme for the options.
            // This will convert the class name, using the root name identifier
            // "LinkTool", and convert it to a string that is used as the key
            // for the final configuration options.
            // Accepted values: "CamelCase", "PascalCase", "SnakeCase"
            "OptionsNamingScheme": "CamelCase",

            // When not null, this will override the `OptionsNamingScheme`
            // and the value coming in from the root name identifier
            // and use this exactly as it is defined here.
            "OverrideOptionsKey": "linkTools"
        },
        "options": null
    }
}
```

With the above config, the output might look a little like this:
```json
{
    "linkTools": {
        "Class": LinkTool
    }
}
```

More details on the output configurations for editorjs `linkTools` can be found here:
- https://github.com/editor-js/link

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
More details on the output configurations for editorjs `list` can be found here:
- https://github.com/editor-js/list

---

The approach described above, which utilises these configurations, allows the Blazor component to load almost any plugin, including custom EditorJS plugins.


### Setup/Installation Brief

1. Install the necessary NuGet package.
2. Ensure that the relevant JavaScript files have been included in the layout.
  - `<script src="/_content/EditorJS/lib/editorjs-bundle.js" asp-append-version="true"></script>`
3. Add the EditorValue and EditorTools JsonObject and create a callback method to handle changes coming from the editor.
  - `public Task OnEditorValueChanged(JsonObject value) => Task.FromResult(EditorValue = value);`
4. Include the Editor markup in the pages where you want it to appear.
  - `<Editor Id="editorjs-blazor" Name="editorjs-blazor" Value="EditorValue" ValueChanged="OnEditorValueChanged" Tools="EditorTools" />`
