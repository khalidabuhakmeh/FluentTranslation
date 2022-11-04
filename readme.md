## Fluent Localization in .NET

This sample uses [Fluent.NET](https://github.com/blushingpenguin/Fluent.Net) a [fluent syntax](https://projectfluent.org/) developed by Mozilla.

The goal of fluent is to move beyond spreadsheet-like formats that don't offer the flexibility on natural languages. Languages have attributes that spreadsheets could adapt to, but that typically leads to resource bloat and difficulty managing a growing set of scenarios and languages.

With Fluent, you can have state-machine-like statements that can account for plurality and other variables. In the following sample, you can use a `$tabCount` argument to determine the result.

```
tabs-close-tooltip = {$tabCount ->
    [one] Close {$tabCount} tab
   *[other] Close {$tabCount} tabs
}
```

That's pretty cool!

## Fluent.NET

Fluent.NET is a lower-level library that wraps the Fluent primitives for you, but that doesn't offer any particullary opinionated approach to consuming the localization files. You need to figure that out. In this sample, I've written a [`Translator`](./FluentTranslation/Translator.cs) that reads the files from embedded resources.

```csharp
using FluentTranslation;

var translator = new Translator<Program>();

var cultures = translator
    .SupportedCultures
    .Select(ci => $"{ci.DisplayName} ({ci.TwoLetterISOLanguageName})");

Console.WriteLine($"Supported cultures of: {string.Join(", ", cultures)}");

var tabClose =
    translator.GetString("tabs-close-button");
var tabCloseWarning =
    translator.GetString("tabs-close-warning", new { tabCount = 4 });
var tabsCloseTooltipItalian =
    translator.GetString("tabs-close-tooltip", new { tabCount = 1 }, new("it"));

Console.WriteLine(tabClose);
Console.WriteLine(tabCloseWarning);
Console.WriteLine(tabsCloseTooltipItalian);
```

In my class I use the `CultureInfo` types found in in .NET, and also convert anonymous objects into the necessary arguments. I also give you a mechanism to retrieve all the supported cultures for UI reasons. I feel they improve the experience but you might like a different approach. Try it out for yourself! 

Here's the results from the code seen previously.

```text
Supported cultures of: English (en), Italian (it)
Close
You are about to close 4 tabs.
Are you sure you want to continue?
Chiudi 1 scheda
```

## Thoughts

- Fluent.NET could have an accompanying package that works with ASP.NET Core and include some tag helpers. That would be cool.
- Syntax highlighting support for the Fluent syntax would be nice.
- Tooling support would be even better :)