using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using VividOrange.Taxonomy.Loads;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcAreaForce : AreaForce, ICalcValue, IParsable<CalcAreaForce>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcAreaForce(Pressure z, string name, string symbol = "")
        : base(z)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcAreaForce(Pressure x, Pressure y, Pressure z, LoadApplication application, string name, string symbol = "")
        : base(x, y, z, application)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcAreaForce result)
    {
        try
        {
            result = s.FromJson<CalcAreaForce>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcAreaForce Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcAreaForce>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcAreaForce result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
