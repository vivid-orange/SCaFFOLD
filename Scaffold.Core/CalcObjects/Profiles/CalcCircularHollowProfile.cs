using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;
using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Profiles;

public sealed class CalcCircularHollowProfile : CircularHollow, ICalcProfile<CalcCircularHollowProfile>, ICalcValue, IParsable<CalcCircularHollowProfile>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcCircularHollowProfile(Length diameter, Length thickness, string name, string symbol = "")
        : base(diameter, thickness)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcCircularHollowProfile(double diameter, double thickness, LengthUnit unit, string name, string symbol = "")
        : base(new Length(diameter, unit), new Length(thickness, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcCircularHollowProfile CreateFromDescription(string description)
    {
        return ProfileDescription.ProfileFromDescription<CalcCircularHollowProfile>(description);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCircularHollowProfile result)
    {
        try
        {
            result = s.FromJson<CalcCircularHollowProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCircularHollowProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCircularHollowProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcCircularHollowProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
