using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcLineForce : LineForce, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcLineForce>
#endif
{
    public string TypeName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcLineForce(ForcePerLength z, string name, string symbol = "")
        : base(z)
    {
        TypeName = name;
        Symbol = symbol;
    }

    public CalcLineForce(ForcePerLength x, ForcePerLength y, ForcePerLength z, LoadApplication application, string name, string symbol = "")
        : base(x, y, z, application)
    {
        TypeName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcLineForce result)
    {
        try
        {
            result = s.FromJson<CalcLineForce>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcLineForce Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcLineForce>();
    }

    public string GetValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcLineForce result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
