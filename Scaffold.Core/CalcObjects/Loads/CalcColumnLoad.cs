using MagmaWorks.Taxonomy.Loads;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Loads;
public sealed class CalcColumnLoad : ColumnLoad, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcColumnLoad>
#endif
{
    public string TypeName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcColumnLoad(Force force, string name, string symbol = "")
        : base(force)
    {
        TypeName = name;
        Symbol = symbol;
    }

    public CalcColumnLoad(Force force, IPointMoment2d topMoment, IPointMoment2d bottomMoment, string name, string symbol = "")
        : base(force, topMoment, bottomMoment)
    {
        TypeName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcColumnLoad result)
    {
        try
        {
            result = s.FromJson<CalcColumnLoad>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcColumnLoad Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcColumnLoad>();
    }

    public string GetValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcColumnLoad result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
