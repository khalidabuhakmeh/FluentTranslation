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
