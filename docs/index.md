---
layout: default
title: "Home"
---

# NTokenizers.Extensions.Spectre.Console

The **NTokenizers.Extensions.Spectre.Console** library provides advanced syntax highlighting using Spectre.Console for XML, JSON, Markdown, TypeScript, JavaScript, C#, and SQL.

![Example](assets/example.png)

This library builds on:
- **[Spectre.Console](https://spectreconsole.net/)** for advanced console rendering
- **[NTokenizers](https://github.com/crwsolutions/NTokenizers)** for modular, stream‑based tokenization

Together, they enable expressive syntax highlighting directly in the console.

## Example Usage

```csharp
await AnsiConsole.Console.WriteMarkdownAsync(stream);
```

> **Especially suitable for parsing AI chat streams**, NTokenizers excels at processing real-time tokenized data from AI models, enabling efficient handling of streaming responses and chat conversations without buffering entire responses.
>
> Check out [AI Example](ai) for an HowTo.

## Token visualization demo

<iframe width="1112" height="590" src="https://www.youtube.com/embed/o4LR8MxP3rg" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

Link: [Youtube](https://www.youtube.com/watch?v=o4LR8MxP3rg)

## Ai demo

<iframe width="1112" height="590" src="https://www.youtube.com/embed/NleKdlooc_0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

Link: [Youtube](https://youtu.be/NleKdlooc_0)

## String output

```csharp
var result = await AnsiConsole.Console.WriteMarkdownAsync(stream);
```

In addition to writing to the console, the original input is returned for convenience.