using MagmaWorks.Taxonomy.Materials;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Materials;
public sealed class CalcBiLinearMaterial : BiLinearMaterial, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcBiLinearMaterial>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcBiLinearMaterial(MaterialType type, Pressure elasticModulus, Pressure yieldStrength, Pressure ultimateStrength, Ratio failureStrain, string name, string symbol = "")
        : base(type, elasticModulus, yieldStrength, ultimateStrength, failureStrain)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcBiLinearMaterial(ILinearElasticMaterial linearElasticMaterial, Pressure ultimateStrength, Ratio failureStrain, string name, string symbol = "")
        : base(linearElasticMaterial, ultimateStrength, failureStrain)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcBiLinearMaterial result)
    {
        try
        {
            result = s.FromJson<CalcBiLinearMaterial>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcBiLinearMaterial Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcBiLinearMaterial>();
    }

    public string GetValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcBiLinearMaterial result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
