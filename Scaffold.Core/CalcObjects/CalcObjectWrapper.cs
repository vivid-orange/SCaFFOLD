using VividOrange.Taxonomy.Serialization;

namespace Scaffold.Core.CalcObjects;

public class CalcObjectWrapper<T> : ICalcValue, ITaxonomySerializable,
    IParsable<CalcObjectWrapper<T>>
{
    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public CalcStatus Status { get; set; } = CalcStatus.None;
    public T Value { get; set; }
    public Type Type { get; set; } = typeof(T);

    public CalcObjectWrapper(T typeValue, string name, string symbol)
    {
        Value = typeValue;
        DisplayName = name;
        Symbol = symbol;
    }

    public static implicit operator T(CalcObjectWrapper<T> value) => value.Value;

    public static CalcObjectWrapper<T> Parse(string s, IFormatProvider provider)
    {
        return s.FromJson<CalcObjectWrapper<T>>();
    }

    public static bool TryParse(string s, IFormatProvider provider, out CalcObjectWrapper<T> result)
    {
        try
        {
            result = s.FromJson<CalcObjectWrapper<T>>();
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public bool TryParse(string strValue)
    {
        try
        {
            var obj = strValue.FromJson<CalcObjectWrapper<T>>();
            Value = obj.Value;
            Symbol = obj.Symbol;
            DisplayName = obj.DisplayName;
            Status = obj.Status;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public string ValueAsString() => this.ToJson();
}
