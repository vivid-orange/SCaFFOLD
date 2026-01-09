using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcPointDisplacement2d : PointDisplacement2d, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcPointDisplacement2d>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcPointDisplacement2d(Length x, Length z, string name, string symbol = "")
        : base(x, z)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPointDisplacement2d result)
    {
        try
        {
            result = s.FromJson<CalcPointDisplacement2d>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPointDisplacement2d Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPointDisplacement2d>();
    }

    public string GetValue() => this.ToJson();

    public bool SetValue(string strValue)
    {
        CalcPointDisplacement2d result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
