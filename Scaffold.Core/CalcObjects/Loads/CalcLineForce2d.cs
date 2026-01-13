using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using VividOrange.Taxonomy.Loads;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Loads;

public sealed class CalcLineForce2d : LineForce2d, ICalcValue, IParsable<CalcLineForce2d>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcLineForce2d(ForcePerLength z, string name, string symbol = "")
        : base(z)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcLineForce2d(ForcePerLength x, ForcePerLength z, LoadApplication application, string name, string symbol = "")
        : base(x, z, application)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcLineForce2d result)
    {
        try
        {
            result = s.FromJson<CalcLineForce2d>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcLineForce2d Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcLineForce2d>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcLineForce2d result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
