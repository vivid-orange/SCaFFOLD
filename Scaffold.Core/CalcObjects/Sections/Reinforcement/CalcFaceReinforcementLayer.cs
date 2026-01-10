using MagmaWorks.Taxonomy.Sections;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Sections.Reinforcement;
public sealed class CalcFaceReinforcementLayer : FaceReinforcementLayer, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcFaceReinforcementLayer>
#endif
{
    public string TypeName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcFaceReinforcementLayer(SectionFace face, IRebar rebar, int numberOfRebars, string name, string symbol = "")
        : base(face, rebar, numberOfRebars)
    {
        TypeName = name;
        Symbol = symbol;
    }

    public CalcFaceReinforcementLayer(SectionFace face, IRebar rebar, Length maxSpacing, string name, string symbol = "")
        : base(face, rebar, maxSpacing)
    {
        TypeName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcFaceReinforcementLayer result)
    {
        try
        {
            result = s.FromJson<CalcFaceReinforcementLayer>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcFaceReinforcementLayer Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcFaceReinforcementLayer>();
    }

    public string GetValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcFaceReinforcementLayer result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
