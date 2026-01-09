using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcPointForce2d : PointForce2d, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcPointForce2d>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcPointForce2d(Force z, string name, string symbol = "")
        : base(z)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcPointForce2d(Force x, Force z, string name, string symbol = "")
        : base(x, z)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPointForce2d result)
    {
        try
        {
            result = s.FromJson<CalcPointForce2d>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPointForce2d Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPointForce2d>();
    }

    public string GetValue() => this.ToJson();

    public bool SetValue(string strValue)
    {
        CalcPointForce2d result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
