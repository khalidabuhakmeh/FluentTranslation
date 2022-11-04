using System.Collections.Immutable;
using System.Dynamic;
using System.Globalization;
using Fluent.Net;

namespace FluentTranslation;

public class Translator<T>
{
    private readonly Dictionary<string, MessageContext> _cultures
        = new();

    public IReadOnlyList<CultureInfo> SupportedCultures
        => _cultures
            .Select(key => new CultureInfo(key.Key))
            .ToImmutableList();

    public Translator()
    {
        // let's load all these message contexts
        var asm = typeof(T).Assembly;
        var locales =
            asm
                .GetManifestResourceNames()
                .Where(n => n.StartsWith("locale."))
                .ToList();

        foreach (var locale in locales)
        {
            using var stream = asm.GetManifestResourceStream(locale)!;
            using var streamReader = new StreamReader(stream);

            var cultureInfo = new CultureInfo(locale[(locale.IndexOf('.') + 1)..]);
            var mc = new MessageContext(cultureInfo.TwoLetterISOLanguageName, new() { UseIsolating = false });
            mc.AddMessages(streamReader);

            _cultures.Add(cultureInfo.TwoLetterISOLanguageName, mc);
        }
    }

    public string GetString(string key, object? args = default, CultureInfo? cultureInfo = default)
    {
        cultureInfo ??= CultureInfo.CurrentUICulture;
        var culture = _cultures[cultureInfo.TwoLetterISOLanguageName];
        var errors = new List<FluentError>();

        var message = culture.GetMessage(key);
        var result = culture.Format(message, Convert(args), errors);

        // do something with errors eventually
        return errors.Any() ? key : result;
    }

    private static Dictionary<string, object?>? Convert(object? content)
    {
        if (content is null)
            return null;

        var props = content.GetType().GetProperties();
        var pairDictionary = props
            .ToDictionary(
                x => x.Name,
                x => x.GetValue(content, null)
            );

        return pairDictionary;
    }
}