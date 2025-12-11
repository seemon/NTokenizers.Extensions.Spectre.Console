using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Styles;

/// <summary>
/// Represents the style configuration for markdown headings in the Spectre.Console rendering extensions for NTokenizers.
/// This class defines the visual styling for different heading levels (H1-H6) when rendering markdown content
/// with syntax highlighting in the console.
/// </summary>
public class MarkdownHeadingStyles
{
    /// <summary>
    /// Gets the default markdown heading styles.
    /// </summary>
    public static MarkdownHeadingStyles Default => new();

    /// <summary>
    /// Gets or sets the style for level 1 headings (H1).
    /// </summary>
    public Style Level1 { get; set; } = Color.Yellow2;

    /// <summary>
    /// Gets or sets the style for levels 2 through 4 headings (H2-H4).
    /// </summary>
    public Style Level2To4 { get; set; } = Color.DarkOliveGreen1_1;

    /// <summary>
    /// Gets or sets the style for levels 5 and above headings (H5-H6 and beyond).
    /// </summary>
    public Style Level5AndAbove { get; set; } = Color.DarkSeaGreen1_1;
}