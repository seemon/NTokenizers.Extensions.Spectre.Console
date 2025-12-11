using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Styles;
/// <summary>
/// Represents the styling options for markdown list items in the NTokenizers console extensions.
/// This class defines the visual appearance of list markers used when rendering markdown content
/// with syntax highlighting support for various token types including XML, JSON, TypeScript, C#, and SQL.
/// </summary>
public class MarkdownListItemStyles
{
    /// <summary>
    /// Gets the default styling configuration for markdown list items.
    /// </summary>
    public static MarkdownListItemStyles Default => new();

    /// <summary>
    /// Gets or sets the style applied to the marker character of markdown list items.
    /// This style controls the visual appearance of the list marker (e.g., bullet points)
    /// when rendering markdown content with syntax highlighting.
    /// </summary>
    public Style Marker { get; set; } = new Style(Color.Turquoise2);
}
