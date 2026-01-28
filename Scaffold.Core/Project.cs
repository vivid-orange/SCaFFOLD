using VividOrange.Standards.Eurocode;

namespace Scaffold;

public static class Project
{
    public static NationalAnnex NationalAnnex => SettingsSingleton.Instance.ProjectSettings.NationalAnnex;
    public static string ProjectName => SettingsSingleton.Instance.ProjectSettings.ProjectName;
    public static string JobNumber => SettingsSingleton.Instance.ProjectSettings.ProjectNumber;
    public static UnitSystem UnitSystem => SettingsSingleton.Instance.UnitSettings.UnitSystem;
    public static int SignificantDigits => SettingsSingleton.Instance.UnitSettings.SignificantDigits;
}
