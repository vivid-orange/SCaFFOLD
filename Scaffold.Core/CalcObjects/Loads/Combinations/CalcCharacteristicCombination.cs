using Newtonsoft.Json;
using Scaffold.Core.Extensions;
using VividOrange.Taxonomy.Loads.Combinations;
using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects.Loads.Combinations;

public sealed class CalcCharacteristicCombination : CharacteristicCombination, ICalcValue, IParsable<CalcCharacteristicCombination>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [JsonConstructor]
    public CalcCharacteristicCombination(string name, string symbol = "")
        : base()
    {
        DisplayName = name;
        Symbol = symbol;
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcCharacteristicCombination result)
    {
        try
        {
            result = s.FromJson<CalcCharacteristicCombination>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public static CalcCharacteristicCombination Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcCharacteristicCombination>();
    }

    public string ValueAsString() => this.ToJson();

    public bool TryParse(string strValue)
    {
        CalcCharacteristicCombination result = null;
        if (TryParse(strValue, null, out result))
        {
            result.CopyTo(this);
            return true;
        }

        return false;
    }
}
