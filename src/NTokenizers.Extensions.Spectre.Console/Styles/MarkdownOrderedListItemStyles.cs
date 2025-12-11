using Spectre.Console;
using System.ComponentModel;

namespace NTokenizers.Extensions.Spectre.Console.Styles;

/// <summary>
/// Represents the styling options for ordered list items in markdown rendering.
/// This class defines the visual appearance of numbered list items when rendering
/// markdown content with Spectre.Console extensions for NTokenizers.
/// </summary>
/// <remarks>
/// The <see cref="MarkdownOrderedListItemStyles"/> class provides styling capabilities
/// for ordered list items, allowing customization of the appearance of numbers
/// in markdown lists. This is particularly useful when rendering structured
/// markdown content such as documentation or formatted text with numbered lists.
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public class MarkdownOrderedListItemStyles
{
    /// <summary>
    /// Gets the default styling options for ordered list items.
    /// </summary>
    /// <value>
    /// A <see cref="MarkdownOrderedListItemStyles"/> instance with default settings,
    /// currently configured with an Aqua color for the number styling.
    /// </value>
    public static MarkdownOrderedListItemStyles Default => new();

    /// <summary>
    /// Gets or sets the style applied to the number portion of ordered list items.
    /// </summary>
    /// <value>
    /// A <see cref="Style"/> object that defines the visual appearance of the
    /// numbering in ordered lists, including color, font weight, and other
    /// text formatting options.
    /// </value>
    public Style Number { get; set; } = new Style(Color.Aqua);
}