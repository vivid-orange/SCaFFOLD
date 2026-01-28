namespace Scaffold.Settings;

public class ScaffoldSettings : IFormattable, IParsable<ScaffoldSettings>
{
    public ProjectSettings Project { get; set; } = new();
    public UnitSettings Units { get; set; } = new();

    public ScaffoldSettings() { }

    public static ScaffoldSettings Parse(string s, IFormatProvider? provider)
    {
        ScaffoldSettings result = s.FromJson<ScaffoldSettings>();
        return result ?? throw new FormatException("Failed to deserialize ScaffoldSettings from JSON");
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out ScaffoldSettings result)
    {
        ScaffoldSettings parsed = s?.FromJson<ScaffoldSettings>();
        result = parsed;
        return parsed != null;
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return this.ToJson();
    }
}

public sealed class SettingsSingleton
{
    public static ScaffoldSettings Instance => lazy.Value;

    private static readonly Lazy<ScaffoldSettings> lazy = new(() => new ScaffoldSettings());
}
