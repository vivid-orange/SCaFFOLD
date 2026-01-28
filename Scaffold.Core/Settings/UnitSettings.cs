using Scaffold.Settings.Units;

namespace Scaffold.Settings;

public class UnitSettings
{
    public int SignificantDigits { get; set; } = 4;

    public ScaffoldUnits Units { get; set; } = new ScaffoldUnits();

    public UnitSettings() { }
}
