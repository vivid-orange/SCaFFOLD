using VividOrange.Standards.Eurocode;

namespace Scaffold.Settings;

public class ProjectSettings
{
    public NationalAnnex NationalAnnex { get; set; } = NationalAnnex.UnitedKingdom;
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectNumber { get; set; } = string.Empty;
}
