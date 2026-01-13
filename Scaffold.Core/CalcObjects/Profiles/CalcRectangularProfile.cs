using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using Scaffold.Core.Utility;
using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Profiles;

public sealed class CalcRectangularProfile : Rectangle, ICalcProfile<CalcRectangularProfile>, ICalcValue, IParsable<CalcRectangularProfile>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcRectangularProfile(Length width, Length height, string name, string symbol = "")
        : base(width, height)
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public CalcRectangularProfile(double width, double height, LengthUnit unit, string name, string symbol = "")
        : base(new Length(width, unit), new Length(height, unit))
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static CalcRectangularProfile CreateFromDescription(string description)
    {
        return ProfileDescription.ProfileFromDescription<CalcRectangularProfile>(description);
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcRectangularProfile result)
    {
        try
        {
            result = s.FromJson<CalcRectangularProfile>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcRectangularProfile Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcRectangularProfile>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcRectangularProfile result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
