namespace EditorJs.Builders;

/// <summary>
/// Fluent builder for constructing EditorJS editor-level configurations as a <see cref="JsonObject"/>.
/// Controls settings such as the default block type, RTL layout, and content area styling.
/// </summary>
public sealed class EditorConfigBuilder
{
    private string? _default_block;
    private bool? _rtl;
    private JsonObject? _codex_editor_redactor_style;

    private EditorConfigBuilder() { }

    /// <summary>
    /// Creates an empty configuration builder.
    /// </summary>
    public static EditorConfigBuilder Create() => new();

    /// <summary>
    /// Sets the default block type created when the user presses Enter.
    /// </summary>
    /// <param name="block_name">The tool key for the default block (e.g. <c>"paragraph"</c>, <c>"text"</c>).</param>
    public EditorConfigBuilder DefaultBlock(string block_name)
    {
        _default_block = block_name;
        return this;
    }

    /// <summary>
    /// Enables or disables right-to-left layout. When enabled, sets <c>direction: rtl</c> on the
    /// holder element and configures EditorJS's internal <c>i18n.direction</c> to <c>"rtl"</c>.
    /// </summary>
    /// <param name="enabled"><c>true</c> to enable RTL; <c>false</c> to leave as default LTR.</param>
    public EditorConfigBuilder Rtl(bool enabled = true)
    {
        _rtl = enabled;
        return this;
    }

    /// <summary>
    /// Applies inline styles to the <c>.codex-editor__redactor</c> element (the content area where blocks live).
    /// Toolbars and popovers are siblings of this element and remain unaffected by these styles.
    /// Use this to constrain the content area height or padding without clipping editor chrome.
    /// </summary>
    /// <param name="configure_style">An action that populates a <see cref="JsonObject"/> with CSS property names and values (e.g. <c>style["maxHeight"] = "64px"</c>).</param>
    public EditorConfigBuilder CodexEditorRedactor(Action<JsonObject> configure_style)
    {
        _codex_editor_redactor_style = [];
        configure_style(_codex_editor_redactor_style);
        return this;
    }

    /// <summary>
    /// Builds the configuration into a <see cref="JsonObject"/> suitable for the <c>Configurations</c> parameter of the Editor component.
    /// </summary>
    /// <returns>A <see cref="JsonObject"/> containing the editor-level configuration.</returns>
    public JsonObject Build()
    {
        JsonObject result = [];

        if (_default_block is not null)
        {
            result["DefaultBlock"] = _default_block;
        }

        if (_rtl is true)
        {
            result["Rtl"] = true;
        }

        if (_codex_editor_redactor_style is not null)
        {
            result["CodexEditorRedactor"] = new JsonObject
            {
                ["style"] = _codex_editor_redactor_style
            };
        }

        return result;
    }
}
