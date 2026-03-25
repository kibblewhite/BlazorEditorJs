namespace EditorJs.Builders;

/// <summary>
/// Specifies the naming convention used to transform a tool key into the EditorJS tool registration key.
/// </summary>
public enum NamingScheme
{
    /// <summary>
    /// Transforms the key to camelCase (e.g. <c>"NestedList"</c> becomes <c>"nestedList"</c>).
    /// </summary>
    CamelCase,

    /// <summary>
    /// Preserves the key as PascalCase (e.g. <c>"NestedList"</c> stays <c>"NestedList"</c>).
    /// </summary>
    PascalCase,

    /// <summary>
    /// Transforms the key to snake_case (e.g. <c>"NestedList"</c> becomes <c>"nested_list"</c>).
    /// </summary>
    SnakeCase,

    /// <summary>
    /// Transforms the key to kebab-case (e.g. <c>"NestedList"</c> becomes <c>"nested-list"</c>).
    /// </summary>
    KebabCase
}
