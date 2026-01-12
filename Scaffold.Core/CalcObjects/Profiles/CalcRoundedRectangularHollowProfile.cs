using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Serialization;
using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcObjects.Profiles;
public sealed class CalcRoundedRectangularHollowProfile : RoundedRectangularHollow, ICalcProfile<CalcRoundedRectangularHollowProfile>, ICalcValue
#if NET7_0_OR_GREATER
    , IParsable<CalcRoundedRectangularHollowProfile>
#endif
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcRoundedRectangularHollowProfile(Length width, Length height, Length flatWidth, Length flatHeight, Length thickness, string name, string symbol = "")
        : base(width, height, flatWidth, flatHeight, thickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcRoundedRectangularHollowProfile(double width, double height, double flatWidth, double flatHeight, double thickness, LengthUnit unit, string name, string symbol = "")
        : base(new Length(width, unit), new Length(height, unit), new Length(flatWidth, unit), new Length(flatHeight, unit), new Length(thickness, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcRoundedRectangularHollowProfile CreateFromDescription(string description)
    {
        return ProfileDescription.ProfileFromDescription<CalcRoundedRectangularHollowProfile>(description);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcRoundedRectangularHollowProfile result)
    {
        try
        {
            result = s.FromJson<CalcRoundedRectangularHollowProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcRoundedRectangularHollowProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcRoundedRectangularHollowProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcRoundedRectangularHollowProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
