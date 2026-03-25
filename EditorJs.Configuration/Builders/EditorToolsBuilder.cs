namespace EditorJs.Builders;

/// <summary>
/// Fluent builder for constructing EditorJS tool configurations as a <see cref="JsonObject"/>.
/// Handles the internal <c>LoadActions</c> and <c>OptionsNamingScheme</c> boilerplate automatically.
/// </summary>
public sealed class EditorToolsBuilder
{
    private readonly Dictionary<string, ToolRegistration> _tools = [];

    private sealed record ToolRegistration(
        string OptionsNamingScheme,
        string? OverrideOptionsKey,
        string? LoadProviderClassFunctionDefault,
        JsonObject? Options
    );

    private EditorToolsBuilder() { }

    private EditorToolsBuilder AddTool(string key, string? override_options_key = null, string? load_provider_class_function_default = null, NamingScheme naming_scheme = NamingScheme.CamelCase, Action<JsonObject>? configure = null)
    {
        JsonObject? options = null;
        if (configure is not null)
        {
            options = [];
            configure(options);
        }

        _tools[key] = new ToolRegistration(
            OptionsNamingScheme: naming_scheme.ToString(),
            OverrideOptionsKey: override_options_key,
            LoadProviderClassFunctionDefault: load_provider_class_function_default,
            Options: options
        );

        return this;
    }

    /// <summary>
    /// Creates an empty builder with no tools registered.
    /// </summary>
    public static EditorToolsBuilder Create() => new();

    /// <summary>
    /// Creates a builder pre-configured with common text editing tools:
    /// Header, NestedList, Quote, Delimiter, Marker, and InlineCode.
    /// </summary>
    public static EditorToolsBuilder Text() => Create()
        .Header()
        .NestedList()
        .Quote()
        .Delimiter()
        .Marker()
        .InlineCode();

    /// <summary>
    /// Creates a builder pre-configured with all bundled block and inline tools:
    /// Header, NestedList, Checklist, CodeTool, Quote, Table, SimpleImage, Embed,
    /// Delimiter, Warning, RawTool, Marker, InlineCode, and Underline.
    /// </summary>
    public static EditorToolsBuilder Rich() => Create()
        .Header()
        .NestedList()
        .Checklist()
        .CodeTool()
        .Quote()
        .Table()
        .SimpleImage()
        .Embed()
        .Delimiter()
        .Warning()
        .RawTool()
        .Marker()
        .InlineCode()
        .Underline();

    /// <summary>
    /// Adds the Header block tool (@editorjs/header). Supports heading levels 1-6.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options such as <c>inlineToolbar</c>, <c>config.placeholder</c>, <c>config.levels</c>, and <c>config.defaultLevel</c>.</param>
    public EditorToolsBuilder Header(Action<JsonObject>? configure = null)
        => AddTool("Header", configure: configure);

    /// <summary>
    /// Adds the NestedList block tool (@editorjs/nested-list). Supports ordered and unordered lists with nesting.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options such as <c>inlineToolbar</c> and <c>config.defaultStyle</c> (<c>"ordered"</c> or <c>"unordered"</c>).</param>
    public EditorToolsBuilder NestedList(Action<JsonObject>? configure = null)
        => AddTool("NestedList", override_options_key: "list", configure: configure);

    /// <summary>
    /// Adds the Checklist block tool (@editorjs/checklist). Renders checkbox items.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options such as <c>inlineToolbar</c>.</param>
    public EditorToolsBuilder Checklist(Action<JsonObject>? configure = null)
        => AddTool("Checklist", configure: configure);

    /// <summary>
    /// Adds the Code block tool (@editorjs/code). Renders a monospace code block.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options such as <c>config.placeholder</c>.</param>
    public EditorToolsBuilder CodeTool(Action<JsonObject>? configure = null)
        => AddTool("CodeTool", override_options_key: "code", configure: configure);

    /// <summary>
    /// Adds the Quote block tool (@editorjs/quote). Renders a blockquote with caption and alignment.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options such as <c>inlineToolbar</c>, <c>config.quotePlaceholder</c>, and <c>config.captionPlaceholder</c>.</param>
    public EditorToolsBuilder Quote(Action<JsonObject>? configure = null)
        => AddTool("Quote", configure: configure);

    /// <summary>
    /// Adds the Table block tool (@editorjs/table). Renders tabular data with optional headings.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options such as <c>inlineToolbar</c>, <c>config.rows</c>, <c>config.cols</c>, and <c>config.withHeadings</c>.</param>
    public EditorToolsBuilder Table(Action<JsonObject>? configure = null)
        => AddTool("Table", configure: configure);

    /// <summary>
    /// Adds the SimpleImage block tool (@editorjs/simple-image). Renders an image from a URL with caption and display options.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options.</param>
    public EditorToolsBuilder SimpleImage(Action<JsonObject>? configure = null)
        => AddTool("SimpleImage", override_options_key: "image", configure: configure);

    /// <summary>
    /// Adds the Embed block tool (@editorjs/embed). Renders embedded content from YouTube, Vimeo, Instagram, and other services.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options. Use <c>config.services</c> to enable specific embed services (e.g. <c>["youtube"] = true</c>).</param>
    public EditorToolsBuilder Embed(Action<JsonObject>? configure = null)
        => AddTool("Embed", configure: configure);

    /// <summary>
    /// Adds the Delimiter block tool (@editorjs/delimiter). Renders a visual separator between content sections.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options.</param>
    public EditorToolsBuilder Delimiter(Action<JsonObject>? configure = null)
        => AddTool("Delimiter", configure: configure);

    /// <summary>
    /// Adds the Warning block tool (@editorjs/warning). Renders a callout with title and message.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options such as <c>config.titlePlaceholder</c> and <c>config.messagePlaceholder</c>.</param>
    public EditorToolsBuilder Warning(Action<JsonObject>? configure = null)
        => AddTool("Warning", configure: configure);

    /// <summary>
    /// Adds the Raw HTML block tool (@editorjs/raw). Renders arbitrary HTML content.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options such as <c>config.placeholder</c>.</param>
    public EditorToolsBuilder RawTool(Action<JsonObject>? configure = null)
        => AddTool("RawTool", override_options_key: "raw", configure: configure);

    /// <summary>
    /// Adds the Marker inline tool (@editorjs/marker). Highlights selected text.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options.</param>
    public EditorToolsBuilder Marker(Action<JsonObject>? configure = null)
        => AddTool("Marker", configure: configure);

    /// <summary>
    /// Adds the InlineCode inline tool (@editorjs/inline-code). Wraps selected text in monospace styling.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options.</param>
    public EditorToolsBuilder InlineCode(Action<JsonObject>? configure = null)
        => AddTool("InlineCode", configure: configure);

    /// <summary>
    /// Adds the Underline inline tool (@editorjs/underline). Underlines selected text.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options.</param>
    public EditorToolsBuilder Underline(Action<JsonObject>? configure = null)
        => AddTool("Underline", configure: configure);

    /// <summary>
    /// Adds the TextElement block tool (editorjs-text). A single-line plain text block designed for titles, labels, and constrained text inputs.
    /// </summary>
    /// <param name="configure">Optional configuration for tool options such as <c>inlineToolbar</c>, <c>config.placeholder</c>, <c>config.hideToolbar</c>, <c>config.hidePopoverItem</c>, <c>config.allowEnterKeyDown</c>, <c>config.wrapElement</c>, and <c>config.startMarginZero</c>.</param>
    public EditorToolsBuilder TextElement(Action<JsonObject>? configure = null)
        => AddTool("Text", load_provider_class_function_default: "TextElement", configure: configure);

    /// <summary>
    /// Registers a no-op paragraph tool that cleanly disables the built-in EditorJS paragraph without causing console warnings.
    /// Use this alongside <see cref="TextElement"/> when replacing the default paragraph with a single-line text block.
    /// </summary>
    public EditorToolsBuilder DisabledParagraph()
        => AddTool("Paragraph", load_provider_class_function_default: "TextElement.DisabledParagraph");

    /// <summary>
    /// Registers a custom or external EditorJS plugin that is loaded via a <c>&lt;script&gt;</c> tag and exposes a global class.
    /// Use this for third-party plugins not bundled with the library (e.g. <c>editorjs-toggle-block</c>).
    /// The <paramref name="key"/> is transformed according to <paramref name="naming_scheme"/> for the EditorJS tool key unless <paramref name="override_options_key"/> is provided.
    /// </summary>
    /// <param name="key">The tool name used in the configuration (e.g. <c>"Toggle"</c>). Transformed according to <paramref name="naming_scheme"/> for the EditorJS tool key.</param>
    /// <param name="global_class_name">The global <c>window</c> object name the plugin registers (e.g. <c>"ToggleBlock"</c>). Supports dot-notation paths (e.g. <c>"MyLib.CustomTool"</c>).</param>
    /// <param name="override_options_key">Optional explicit EditorJS tool key, bypassing the automatic naming scheme transformation of <paramref name="key"/>.</param>
    /// <param name="naming_scheme">The naming convention used to transform <paramref name="key"/> into the EditorJS tool key. Defaults to <see cref="NamingScheme.CamelCase"/>.</param>
    /// <param name="configure">Optional configuration for tool options such as <c>inlineToolbar</c> and plugin-specific <c>config</c> properties.</param>
    public EditorToolsBuilder Tool(string key, string global_class_name, string? override_options_key = null, NamingScheme naming_scheme = NamingScheme.CamelCase, Action<JsonObject>? configure = null)
        => AddTool(key, override_options_key: override_options_key, load_provider_class_function_default: global_class_name, naming_scheme: naming_scheme, configure: configure);

    /// <summary>
    /// Builds the tool configurations into a <see cref="JsonObject"/> suitable for the <c>Tools</c> parameter of the Editor component.
    /// </summary>
    /// <returns>A <see cref="JsonObject"/> containing all registered tool configurations with their <c>LoadActions</c> metadata.</returns>
    public JsonObject Build()
    {
        JsonObject result = [];

        foreach ((string key, ToolRegistration registration) in _tools)
        {
            JsonObject load_actions = new()
            {
                ["OptionsNamingScheme"] = registration.OptionsNamingScheme
            };

            if (registration.OverrideOptionsKey is not null)
            {
                load_actions["OverrideOptionsKey"] = registration.OverrideOptionsKey;
            }

            if (registration.LoadProviderClassFunctionDefault is not null)
            {
                load_actions["LoadProviderClassFunctionDefault"] = registration.LoadProviderClassFunctionDefault;
            }

            JsonObject tool_entry = new()
            {
                ["LoadActions"] = load_actions
            };

            if (registration.Options is not null)
            {
                tool_entry["options"] = registration.Options;
            }

            result[key] = tool_entry;
        }

        return result;
    }
}
