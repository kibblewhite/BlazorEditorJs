namespace EditorJs;

public static class Extensions
{
    public static void AddScopedEditorJsInterop(this IServiceCollection services)
        => services.AddScoped<EditorJsInterop>();
}
