using NTokenizers.Markdown.Metadata;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal class MarkdownFootnoteWriter(IAnsiConsole ansiConsole)
{
    internal void Write(FootnoteMetadata footnoteMeta)
    {
        ansiConsole.Write($"[^{footnoteMeta.Id}]");
    }
}