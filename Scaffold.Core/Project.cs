using Scaffold.Settings;
using Scaffold.Settings.Units;
using VividOrange.Standards.Eurocode;

namespace Scaffold;

public static class Project
{
    public static NationalAnnex NationalAnnex => SettingsSingleton.Instance.Project.NationalAnnex;
    public static string ProjectName => SettingsSingleton.Instance.Project.ProjectName;
    public static string JobNumber => SettingsSingleton.Instance.Project.ProjectNumber;
    public static ScaffoldUnits Units => SettingsSingleton.Instance.Units.Units;
    public static int SignificantDigits => SettingsSingleton.Instance.Units.SignificantDigits;
}
