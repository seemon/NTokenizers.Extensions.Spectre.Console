using NTokenizers.Core;
using NTokenizers.Markdown.Metadata;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Diagnostics;

namespace NTokenizers.Extensions.Spectre.Console.Writers;

internal abstract class BaseInlineWriter<TToken, TTokentype>(IAnsiConsole ansiConsole) where TToken : IToken<TTokentype> where TTokentype : Enum
{
    protected virtual Style GetStyle(TTokentype token) => Style.Plain;

    protected readonly Paragraph _liveParagraph = new("");

    internal void WriteToken(TToken token)
    {
        ansiConsole.Write(new Markup(Markup.Escape(token.Value), GetStyle(token.TokenType)));
    }

    internal async Task WriteAsync(InlineMarkdownMetadata<TToken> metadata)
    {
        var liveDisplay = new LiveDisplay(ansiConsole, GetIRendable());
        await liveDisplay
        .StartAsync(async ctx =>
        {
            await StartedAsync(metadata);
            await metadata.RegisterInlineTokenHandler(async inlineToken =>
            {
                await WriteTokenAsync(_liveParagraph, inlineToken);
                ctx.Refresh();
            });

            await FinalizeAsync(metadata);
            ctx.Refresh();
        });
    }

    protected virtual IRenderable GetIRendable()
    {
        return new Panel(_liveParagraph)
            .Border(new LeftBoxBorder())
            .BorderStyle(new Style(Color.Green))
            ;
    }

    protected virtual Task StartedAsync(InlineMarkdownMetadata<TToken> metadata) => Task.CompletedTask;

    protected virtual Task FinalizeAsync(InlineMarkdownMetadata<TToken> metadata) => Task.CompletedTask;

    protected virtual Task WriteTokenAsync(Paragraph liveParagraph, TToken token)
    {
        if (!string.IsNullOrEmpty(token.Value))
        {
            Debug.WriteLine($"Writing token: `{token.Value}` of type `{token.TokenType}`");
            liveParagraph.Append(token.Value, GetStyle(token.TokenType));
        }
        return Task.CompletedTask;
    }
}
