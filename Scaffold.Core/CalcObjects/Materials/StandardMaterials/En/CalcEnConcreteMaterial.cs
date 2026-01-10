using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Serialization;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;

namespace Scaffold.Core.CalcObjects.Materials.StandardMaterials.En;
public sealed class CalcEnConcreteMaterial : EnConcreteMaterial, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcEnConcreteMaterial>
#endif
{
    public string TypeName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcEnConcreteMaterial(EnConcreteGrade grade, NationalAnnex nationalAnnex, string name, string symbol = "")
        : base(grade, nationalAnnex)
    {
        TypeName = name;
        Symbol = symbol;
    }

    public CalcEnConcreteMaterial(EnConcreteGrade grade, NationalAnnex nationalAnnex, EnConcreteExposureClass exposureClass, Length maxAggregateSize, EnCementClass cementClass, string name, string symbol = "")
        : base(grade, nationalAnnex, exposureClass, maxAggregateSize, cementClass)
    {
        TypeName = name;
        Symbol = symbol;
    }

    public CalcEnConcreteMaterial(EnConcreteGrade grade, NationalAnnex nationalAnnex, EnConcreteExposureClass exposureClass, Length maxAggregateSize, EnCementClass cementClass, Length crackWidthLimit, Length minimumCover, string name, string symbol = "")
        : base(grade, nationalAnnex, exposureClass, maxAggregateSize, cementClass, crackWidthLimit, minimumCover)
    {
        TypeName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcEnConcreteMaterial result)
    {
        try
        {
            result = s.FromJson<CalcEnConcreteMaterial>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcEnConcreteMaterial Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcEnConcreteMaterial>();
    }

    public string GetValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcEnConcreteMaterial result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
