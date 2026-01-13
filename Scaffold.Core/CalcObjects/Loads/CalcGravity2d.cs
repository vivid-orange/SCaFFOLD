using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using VividOrange.Taxonomy.Loads;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Loads;

public sealed class CalcGravity2d : Gravity2d, ICalcValue, IParsable<CalcGravity2d>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcGravity2d(string name, string symbol = "")
        : base()
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcGravity2d(Ratio z, string name, string symbol = "")
        : base(z)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcGravity2d result)
    {
        try
        {
            result = s.FromJson<CalcGravity2d>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcGravity2d Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcGravity2d>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcGravity2d result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
