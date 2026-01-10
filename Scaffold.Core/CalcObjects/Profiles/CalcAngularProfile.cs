using MagmaWorks.Taxonomy.Profiles;
using MagmaWorks.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;
using Angle = MagmaWorks.Taxonomy.Profiles.Angle;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcAngularProfile : Angle, ICalcProfile<CalcAngularProfile>, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcAngularProfile>
#endif
{
    public string TypeName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcAngularProfile(Length height, Length width, Length webThickness, Length flangeThickness, string name, string symbol = "")
        : base(height, width, webThickness, flangeThickness)
    {
        TypeName = name;
        Symbol = symbol;
    }

    public CalcAngularProfile(double height, double width, double webThickness, double flangeThickness, LengthUnit unit, string name, string symbol = "")
        : base(new Length(height, unit), new Length(width, unit), new Length(webThickness, unit), new Length(flangeThickness, unit))
    {
        TypeName = name;
        Symbol = symbol;
    }

    public static CalcAngularProfile CreateFromDescription(string description)
    {
        return ProfileDescription.ProfileFromDescription<CalcAngularProfile>(description);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcAngularProfile result)
    {
        try
        {
            result = s.FromJson<CalcAngularProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcAngularProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcAngularProfile>();
    }

    public string GetValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcAngularProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
