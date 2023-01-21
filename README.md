
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
- BlazorApp\Pages\Index.razor
- BlazorApp\Pages\Index.razor.cs
- EditorJS\README.md

---

If you feel like giving support, please send some towards codex editorjs team at:
- https://opencollective.com/editorjs

---

Think there is a missing offical plugin that should be included and is present in the `jsdelivr` CDN, or some information that is out of date, let me know and I'll do my best for it to be included into the project.

`MinimumVisualStudioVersion = 17.4.33213.308`
- https://learn.microsoft.com/en-us/visualstudio/install/visual-studio-build-numbers-and-release-dates

Notes:

```bash
dotnet build -c Release
dotnet pack EditorJS/EditorJS.csproj -c Release
dotnet publish EditorJS/EditorJS.csproj /p:PublishProfile=EditorJS/Properties/PublishProfiles/EditorJS.pubxml -c Release
```
