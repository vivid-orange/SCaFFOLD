using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcZProfile : Z, ICalcProfile<CalcZProfile>, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcZProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcZProfile(Length height, Length topFlangeWidth, Length bottomFlangeWidth, Length thickness, Length topLip, Length bottomLip, string name, string symbol = "")
        : base(height, topFlangeWidth, bottomFlangeWidth, thickness, topLip, bottomLip)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcZProfile(double height, double topFlangeWidth, double bottomFlangeWidth, double thickness, double topLip, double bottomLip, LengthUnit unit, string name, string symbol = "")
        : base(new Length(height, unit), new Length(topFlangeWidth, unit), new Length(bottomFlangeWidth, unit), new Length(thickness, unit), new Length(topLip, unit), new Length(bottomLip, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcZProfile CreateFromDescription(string description)
    {
        return ProfileDescription.ProfileFromDescription<CalcZProfile>(description);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcZProfile result)
    {
        try
        {
            result = s.FromJson<CalcZProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcZProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcZProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcZProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
