using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using VividOrange.Taxonomy.Loads;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Loads;

public sealed class CalcPointDisplacement : PointDisplacement, ICalcValue, IParsable<CalcPointDisplacement>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcPointDisplacement(Length x, Length y, Length z, string name, string symbol = "")
        : base(x, y, z)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPointDisplacement result)
    {
        try
        {
            result = s.FromJson<CalcPointDisplacement>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPointDisplacement Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPointDisplacement>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcPointDisplacement result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
