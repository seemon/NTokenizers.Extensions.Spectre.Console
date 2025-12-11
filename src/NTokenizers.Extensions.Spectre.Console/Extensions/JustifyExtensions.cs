using Spectre.Console;
using NTokenizersMarkdown = NTokenizers.Markdown.Metadata;

namespace NTokenizers.Extensions.Spectre.Console.Extensions;
internal static class JustifyExtensions
{
    internal static Justify ToSpectreJustify(this NTokenizersMarkdown.Justify justify) => justify switch
    {
        NTokenizersMarkdown.Justify.Left => Justify.Left,
        NTokenizersMarkdown.Justify.Center => Justify.Center,
        NTokenizersMarkdown.Justify.Right => Justify.Right,
        _ => Justify.Left // fallback
    };
}
