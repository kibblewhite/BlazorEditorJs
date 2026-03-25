namespace EditorJs.Provisioning;

/// <summary>
/// Represents the initialisation lifecycle states of an EditorJS instance.
/// Values are powers of two for bitwise composition (e.g. <c>Constructed | Initialised</c>).
/// </summary>
public enum ProvisioningState
{
    /// <summary>
    /// The Blazor component has been constructed and its parameters are set.
    /// </summary>
    Constructed = 2,

    /// <summary>
    /// The EditorJS JavaScript instance has been created and is ready for interaction.
    /// </summary>
    Initialised = 4,

    /// <summary>
    /// Content has been rendered into the editor via <see cref="Editor.RenderAsync"/>.
    /// </summary>
    Rendered = 8
}
