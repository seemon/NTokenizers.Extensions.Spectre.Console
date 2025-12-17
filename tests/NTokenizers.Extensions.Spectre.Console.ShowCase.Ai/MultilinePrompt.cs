using Spectre.Console;
using Spectre.Console.Rendering;
using TextCopy;
using SystemConsole = System.Console;

namespace NTokenizers.Extensions.Spectre.Console.ShowCase.Ai;

public sealed class MultilinePrompt : IPrompt<string>
{
    private const string WRAP_STRING = " ↩ ";

    private sealed record class VisualLine
    {
        public int LogicalLine { get; init; }
        public int StartIndex { get; init; }
        public string Text { get; init; } = "";
        public bool IsWrapped { get; init; } = false;
    }

    readonly List<string> lines = [""];
    int currentLine = 0;

    readonly int startLeft = SystemConsole.CursorLeft;
    int startTop = SystemConsole.CursorTop;

    internal string Read(IAnsiConsole console)
    {
        int cursorPos = 0;
        while (true)
        {
            // Redraw
            Redraw(cursorPos);

            // Key handling
            if (console.Input.ReadKey(true) is ConsoleKeyInfo keyInfo)
            {
                var (newCursorPos, inputIsCompleted) = keyInfo.Key switch
                {
                    ConsoleKey.Enter when (keyInfo.Modifiers & ConsoleModifiers.Shift) != 0 => HandleSoftEnter(cursorPos), // Insert line break without completing input
                    ConsoleKey.Enter => (cursorPos, true), // Complete input
                    ConsoleKey.Backspace when cursorPos > 0 => RemoveCharacter(cursorPos), // Remove character before cursor
                    ConsoleKey.Backspace when currentLine > 0 => RemoveLine(cursorPos), // Merge with previous line
                    ConsoleKey.Delete when cursorPos < lines[currentLine].Length => DeleteCharacter(cursorPos), // Delete character at cursor
                    ConsoleKey.Delete when currentLine < lines.Count - 1 => MergeNextLine(cursorPos), // Merge with next line
                    ConsoleKey.LeftArrow when cursorPos > 0 => (cursorPos - 1, false), // Move cursor left
                    ConsoleKey.LeftArrow when currentLine > 0 => (lines[--currentLine].Length, false), // Move to end of previous line
                    ConsoleKey.RightArrow when cursorPos < lines[currentLine].Length => (cursorPos + 1, false), // Move cursor right
                    ConsoleKey.RightArrow when currentLine < lines.Count - 1 => MoveToNextLineStart(cursorPos), // Move to start of next line
                    ConsoleKey.UpArrow when currentLine > 0 => (Math.Min(cursorPos, lines[--currentLine].Length), false), // Move up preserving column
                    ConsoleKey.DownArrow when currentLine < lines.Count - 1 => (Math.Min(cursorPos, lines[++currentLine].Length), false), // Move down preserving column
                    ConsoleKey.Home => (0, false), // Move to start of line
                    ConsoleKey.End => (lines[currentLine].Length, false), // Move to end of line
                    ConsoleKey.B when (keyInfo.Modifiers & ConsoleModifiers.Control) != 0 => Paste(cursorPos), // Paste from clipboard
                    _ when !char.IsControl(keyInfo.KeyChar) => InsertCharacter(cursorPos, keyInfo.KeyChar), // Insert printable character
                    _ => (cursorPos, false) // No-op
                };

                cursorPos = newCursorPos;

                if (inputIsCompleted)
                {
                    break;
                }
            }
        }

        var visualLines = Redraw(cursorPos, false);
        var boxEnd = startTop + visualLines + 2;
        if (boxEnd < SystemConsole.BufferHeight)
        {
            SystemConsole.SetCursorPosition(0, boxEnd);
        }
        return string.Join(Environment.NewLine, lines);
    }

    private (int CursorPos, bool IsComplete) RemoveLine(int cursorPos)
    {
        int newCursorPos = lines[currentLine - 1].Length;
        lines[currentLine - 1] += lines[currentLine];
        lines.RemoveAt(currentLine);
        currentLine--;
        return (newCursorPos, false);
    }

    private (int CursorPos, bool IsComplete) RemoveCharacter(int cursorPos)
    {
        lines[currentLine] = lines[currentLine].Remove(cursorPos - 1, 1);
        return (cursorPos - 1, false);
    }

    private (int CursorPos, bool IsComplete) HandleSoftEnter(int cursorPos)
    {
        string current = lines[currentLine];
        string left = current[..cursorPos];
        string right = current[cursorPos..];

        lines[currentLine] = left;
        lines.Insert(currentLine + 1, right);
        currentLine++;
        return (0, false);
    }

    private (int CursorPos, bool IsComplete) DeleteCharacter(int cursorPos)
    {
        lines[currentLine] = lines[currentLine].Remove(cursorPos, 1);
        return (cursorPos, false);
    }

    private (int CursorPos, bool IsComplete) MergeNextLine(int cursorPos)
    {
        lines[currentLine] += lines[currentLine + 1];
        lines.RemoveAt(currentLine + 1);
        return (cursorPos, false);
    }

    private (int CursorPos, bool IsComplete) MoveToNextLineStart(int cursorPos)
    {
        currentLine++;
        return (0, false);
    }

    private (int CursorPos, bool IsComplete) InsertCharacter(int cursorPos, char c)
    {
        lines[currentLine] = lines[currentLine].Insert(cursorPos, c.ToString());
        return (cursorPos + 1, false);
    }

    private (int CursorPos, bool IsComplete) Paste(int cursorPos)
    {
        string pasted = ClipboardService.GetText() ?? "";
        pasted = pasted.Replace("\r\n", "\n").Replace("\r", "\n");

        var pastedLines = pasted.Split('\n');

        string current = lines[currentLine];
        string left = current[..cursorPos];
        string right = current[cursorPos..];

        lines[currentLine] = left + pastedLines[0];

        for (int i = 1; i < pastedLines.Length; i++)
        {
            lines.Insert(currentLine + i, pastedLines[i]);
        }

        currentLine += pastedLines.Length - 1;
        lines[currentLine] += right;

        return (pastedLines[^1].Length, false);
    }

    private int Redraw(int cursorPos, bool hasPrompt = true)
    {
        var boxWidth = SystemConsole.WindowWidth;
        var prefixWidth = hasPrompt ? 4 : 2;
        var contentWidth = SystemConsole.WindowWidth - startLeft - prefixWidth - 2;

        // Build visual lines
        var visualLines = BuildVisualLines(lines, contentWidth);

        // Clear previous box area
        var endBox = startTop + visualLines.Count + 2;
        if (endBox < SystemConsole.BufferHeight)
        {
            SystemConsole.SetCursorPosition(0, endBox);
            SystemConsole.Write(new string(' ', boxWidth));
        }

        // Draw top border
        if (startTop > 0)
        {
            SystemConsole.SetCursorPosition(startLeft, startTop);
        }

        var renderables = new List<IRenderable>();
        var overflow = endBox + 1 - SystemConsole.BufferHeight;
        if (overflow > 0)
        {
            startTop -= overflow;
        }

        // Draw content
        for (int i = 0; i < visualLines.Count; i++)
        {
            var start = hasPrompt ? i == 0 ? "> " : "  " : "";

            var wrap = visualLines[i].IsWrapped ? WRAP_STRING : "";
            var text = $"{start}{visualLines[i].Text}{wrap}";

            renderables.Add(new Markup(Markup.Escape(text)));
        }

        var rows = new Rows(renderables);

        // Put the rows inside a panel
        var panel = new Panel(rows)
        {
            Border = BoxBorder.Rounded,
            BorderStyle = Color.Green1,
            Expand = true
        };

        AnsiConsole.Write(panel);

        // Map cursor to visual position
        int vLine = 0, vOffset = 0;
        for (int i = 0; i < visualLines.Count; i++)
        {
            var v = visualLines[i];
            if (v.LogicalLine == currentLine &&
                cursorPos >= v.StartIndex &&
                cursorPos <= v.StartIndex + v.Text.Length)
            {
                vLine = i;
                vOffset = cursorPos - v.StartIndex;
                if (v.IsWrapped && vOffset > v.Text.Length - WRAP_STRING.Length)
                {
                    vOffset = v.Text.Length - WRAP_STRING.Length; // beperk cursor
                }

                break;
            }
        }
        var hPos = Math.Min(startTop + 1 + vLine, SystemConsole.BufferHeight - 3);
        if (hPos < SystemConsole.BufferHeight)
        {
            SystemConsole.SetCursorPosition(startLeft + prefixWidth + vOffset, hPos);
        }

        return visualLines.Count;
    }

    private List<VisualLine> BuildVisualLines(List<string> lines, int maxWidth)
    {
        var result = new List<VisualLine>();

        for (int i = 0; i < lines.Count; i++)
        {
            string line = lines[i];
            int index = 0;

            if (line.Length == 0)
            {
                result.Add(new VisualLine { LogicalLine = i, StartIndex = 0, Text = " " });
                continue;
            }

            while (index < line.Length)
            {
                int remaining = line.Length - index;
                bool wrapped = remaining > (maxWidth - WRAP_STRING.Length);

                int take = wrapped ? Math.Max(0, maxWidth - WRAP_STRING.Length) : Math.Min(maxWidth, remaining);
                string part = line.Substring(index, take);

                result.Add(new VisualLine { LogicalLine = i, StartIndex = index, Text = part, IsWrapped = wrapped });
                index += take;
            }
        }

        return result;
    }

    public string Show(IAnsiConsole console) => ShowAsync(console, CancellationToken.None).GetAwaiter().GetResult();

    public async Task<string> ShowAsync(IAnsiConsole console, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(console);

        return await console.RunExclusive<Task<string>>(async () =>
        {
            return Read(console);
        });
    }
}
