using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Materials;
public sealed class CalcParabolaRectangleMaterial : ParabolaRectangleMaterial, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcParabolaRectangleMaterial>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcParabolaRectangleMaterial(MaterialType type, Pressure yieldStrength, Ratio yieldStrain, Ratio failureStrain, double exponent, string name, string symbol = "")
        : base(type, yieldStrength, yieldStrain, failureStrain, exponent)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcParabolaRectangleMaterial result)
    {
        try
        {
            result = s.FromJson<CalcParabolaRectangleMaterial>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcParabolaRectangleMaterial Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcParabolaRectangleMaterial>();
    }

    public string GetValue() => this.ToJson();

    public bool SetValue(string strValue)
    {
        CalcParabolaRectangleMaterial result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
