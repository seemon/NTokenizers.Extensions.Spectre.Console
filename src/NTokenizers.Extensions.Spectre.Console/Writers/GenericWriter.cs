using NTokenizers.Markdown;
using Spectre.Console;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal sealed class GenericWriter(IAnsiConsole ansiConsole) : BaseInlineWriter<MarkdownToken, MarkdownTokenType>(ansiConsole);