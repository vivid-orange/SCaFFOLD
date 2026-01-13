using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;
using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Profiles;

public sealed class CalcRoundedRectangularProfile : RoundedRectangle, ICalcProfile<CalcRoundedRectangularProfile>, ICalcValue, IParsable<CalcRoundedRectangularProfile>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcRoundedRectangularProfile(Length width, Length height, Length flatWidth, Length flatHeight, string name, string symbol = "")
        : base(width, height, flatWidth, flatHeight)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcRoundedRectangularProfile(double width, double height, double flatWidth, double flatHeight, LengthUnit unit, string name, string symbol = "")
        : base(new Length(width, unit), new Length(height, unit), new Length(flatWidth, unit), new Length(flatHeight, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcRoundedRectangularProfile CreateFromDescription(string description)
    {
        return ProfileDescription.ProfileFromDescription<CalcRoundedRectangularProfile>(description);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcRoundedRectangularProfile result)
    {
        try
        {
            result = s.FromJson<CalcRoundedRectangularProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcRoundedRectangularProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcRoundedRectangularProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcRoundedRectangularProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
