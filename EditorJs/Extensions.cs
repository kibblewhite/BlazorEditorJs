namespace EditorJs;

/// <summary>
/// Provides dependency injection extension methods for the EditorJS Blazor component library.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Registers <see cref="EditorJsInterop"/> as a scoped service and <see cref="ProvisioningMonitor"/> as a transient service.
    /// Call this in <c>Program.cs</c> to enable the <c>&lt;Editor /&gt;</c> component.
    /// </summary>
    /// <param name="services">The service collection to register services with.</param>
    public static void AddScopedEditorJsInterop(this IServiceCollection services)
    {
        services.AddScoped<EditorJsInterop>();
        services.AddTransient<ProvisioningMonitor>();
    }
}
