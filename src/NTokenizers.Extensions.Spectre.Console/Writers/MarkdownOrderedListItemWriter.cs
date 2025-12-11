using NTokenizers.Extensions.Spectre.Console.Styles;
using NTokenizers.Markdown.Metadata;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class MarkdownOrderedListItemWriter(IAnsiConsole ansiConsole, MarkdownOrderedListItemStyles styles)
{
    internal async Task WriteAsync(OrderedListItemMetadata metadata)
    {
        ansiConsole.Write(new Markup($"{metadata.Number}. ", styles.Number));
        await metadata.RegisterInlineTokenHandler(
            async token => await MarkdownWriter.Create(ansiConsole).WriteAsync(token));
        ansiConsole.Write(new Text("\n")); //HACK
    }
}
