using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcPerimeterReinforcementLayer : PerimeterReinforcementLayer, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcPerimeterReinforcementLayer>
#endif
{
    public string TypeName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcPerimeterReinforcementLayer(IRebar rebar, int numberOfRebars, string name, string symbol = "")
        : base(rebar, numberOfRebars)
    {
        TypeName = name;
        Symbol = symbol;
    }

    public CalcPerimeterReinforcementLayer(IRebar rebar, Length maxSpacing, string name, string symbol = "")
        : base(rebar, maxSpacing)
    {
        TypeName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcPerimeterReinforcementLayer result)
    {
        try
        {
            result = s.FromJson<CalcPerimeterReinforcementLayer>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcPerimeterReinforcementLayer Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcPerimeterReinforcementLayer>();
    }

    public string GetValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcPerimeterReinforcementLayer result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
