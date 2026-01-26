public class ScaffoldSettings
{
    public ProjectSettings ProjectSettings { get; set; } = new();
    public UnitSettings UnitSettings { get; set; } = new();

    public ScaffoldSettings() { }
}

internal sealed class SettingsSingleton
{
    public static ScaffoldSettings Instance => lazy.Value;

    private static readonly Lazy<ScaffoldSettings> lazy =
            new Lazy<ScaffoldSettings>(() => new ScaffoldSettings());
}
