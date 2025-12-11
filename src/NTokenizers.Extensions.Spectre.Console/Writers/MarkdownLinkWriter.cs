using NTokenizers.Markdown.Metadata;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal class MarkdownLinkWriter(IAnsiConsole ansiConsole)
{
    internal void Write(LinkMetadata linkMeta)
    {
        ansiConsole.Write($"[{linkMeta.Title}]({linkMeta.Url})");
    }
}