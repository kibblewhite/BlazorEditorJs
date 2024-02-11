
[![GitHub Repo](https://img.shields.io/badge/GitHub-Repo-green?logo=github&style=flat-square)](https://github.com/kibblewhite/BlazorEditorJs)
[![GitHub Licence](https://img.shields.io/github/license/kibblewhite/BlazorEditorJs?logo=github&style=flat-square)](https://github.com/kibblewhite/BlazorEditorJs/blob/master/LICENSE)
[![GitHub Stars](https://img.shields.io/github/stars/kibblewhite/BlazorEditorJs?style=flat-square&logo=github)](https://github.com/kibblewhite/BlazorEditorJs/stargazers)
[![Nuget Version](https://img.shields.io/nuget/v/EditorJs?label=nuget%20version&logo=nuget&style=flat-square)](https://www.nuget.org/packages/EditorJs/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/EditorJs?label=nuget%20downloads&logo=nuget&style=flat-square)](https://www.nuget.org/packages/EditorJs/)

# BlazorEditorJs

A simple editorjs implementation as a blazor component.
- https://editorjs.io/

All I ask for is a github star for me and one for the editorjs repository if you use this, thank you.

---

To see it's usage, please visit the `BlazorApp` project and inspect the files:
- BlazorEditorJs.App\BlazorEditorJs.App.Client\Pages\Home.razor
- BlazorEditorJs.App\BlazorEditorJs.App.Client\Pages\Home.razor.cs
- BlazorEditorJs.Lib\README.md

---

If you feel like giving support, please send some towards codex editorjs team at:
- https://opencollective.com/editorjs

---

Think there is a missing offical tool/plugin that should be included. The tool should meet the following requirements:
- It is present in the `jsdelivr` CDN and can be included via the libman tool (Microsoft.Web.LibraryManager.Build)
- Their configuration schema should be correct, sample. `tools: { plugin: { class: class_fn, ... } }`

Let me know and I'll do my best for it to be included into the project.

Did you know that you can dynamically load plugin-tools from external CDN and this project supports it? Check out the [edtorjs-toggle-block](https://github.com/kommitters/editorjs-toggle-block) example being loaded in externally in the `BlazorWasmApp` project.

`MinimumVisualStudioVersion = 17.4.33213.308`
- https://learn.microsoft.com/en-us/visualstudio/install/visual-studio-build-numbers-and-release-dates

## Next steps...

- UI Unit Testing using Microsoft's [playwright](https://playwright.dev/)
- Better exception handling, either: catch and log / bubble up to the application layer / handles error by passing exception message up to calling method
- Use of Monads to handle results and data internally (TBC)

Notes:

```bash
dotnet nuget push EditorJS/bin/Release/net8.0/publish/EditorJS.*.nupkg -k <api-key /> -s https://api.nuget.org/v3/index.json
```
