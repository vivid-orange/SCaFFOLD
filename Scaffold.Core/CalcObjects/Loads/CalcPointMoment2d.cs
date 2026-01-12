using VividOrange.Taxonomy.Loads;
using VividOrange.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcPointMoment2d : PointMoment2d, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcPointMoment2d>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcPointMoment2d(Torque yy, Torque zz, string name, string symbol = "")
        : base(yy, zz)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPointMoment2d result)
    {
        try
        {
            result = s.FromJson<CalcPointMoment2d>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPointMoment2d Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPointMoment2d>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcPointMoment2d result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
