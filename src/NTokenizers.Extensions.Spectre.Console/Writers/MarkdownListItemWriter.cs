using NTokenizers.Markdown.Metadata;
using NTokenizers.Extensions.Spectre.Console.Styles;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class MarkdownListItemWriter(IAnsiConsole ansiConsole, MarkdownListItemStyles styles)
{
    internal async Task WriteAsync(ListItemMetadata metadata)
    {
        ansiConsole.Write(new Markup($"{metadata.Marker} ", styles.Marker));
        await metadata.RegisterInlineTokenHandler(
            async token => await MarkdownWriter.Create(ansiConsole).WriteAsync(token));
        ansiConsole.Write(new Text("\n")); //HACK
    }
}