public class ScaffoldSettings
{
    public bool ProjectSettings { get; } = true;
    public string UnitSettings { get; } = "";

    public ScaffoldSettings() { }
}

internal sealed class SettingsSingleton
{
    public static ScaffoldSettings Instance => lazy.Value;

    private static readonly Lazy<ScaffoldSettings> lazy =
            new Lazy<ScaffoldSettings>(() => new ScaffoldSettings());
}
