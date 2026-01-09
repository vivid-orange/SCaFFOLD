using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcPointMoment : PointMoment, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcPointMoment>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcPointMoment(Torque xx, Torque yy, Torque zz, string name, string symbol = "")
        : base(xx, yy, zz)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPointMoment result)
    {
        try
        {
            result = s.FromJson<CalcPointMoment>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPointMoment Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPointMoment>();
    }

    public string GetValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcPointMoment result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
