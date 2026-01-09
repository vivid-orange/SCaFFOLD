using System.ComponentModel;
using Scaffold.Core.CalcValues;

namespace Scaffold.Core.Abstract;

public abstract class CalcValue<T> : ICalcValue, IEquatable<CalcValue<T>>
{
    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public CalcStatus Status { get; set; }
    public T Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string GetValueAsString() => ToString();

    protected CalcValue(T value, string name, string symbol, string unit = "")
    {
        Value = value;
        DisplayName = name;
        Symbol = symbol?.Trim();
        Unit = unit?.Trim();
    }

    public static implicit operator T(CalcValue<T> value) => value.Value;

    public static bool operator ==(CalcValue<T> value, CalcValue<T> other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Equals(other);
    }

    public static bool operator !=(CalcValue<T> value, CalcValue<T> other)
    {
        return !value.Equals((object)other);
    }

    public virtual bool TryParse(string input)
    {
        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        if (converter != null)
        {
            try
            {
                Value = (T)converter.ConvertFromString(null, CultureInfo.InvariantCulture, input);
                return true;
            }
            catch (ArgumentException) { }
        }

        return false;
    }

    internal static (string name, string symbol, string unit) OperatorMetadataHelper<U1, U2>(
        CalcValue<U1> x, CalcValue<U2> y, char operation)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009{operation}\u2009{y.DisplayName}";
        string symbol = x.Symbol == y.Symbol ? x.Symbol : string.Empty;
        string unit = x.Unit == y.Unit ? x.Unit : string.Empty;
        return (name, symbol, unit);
    }

    internal static void CheckUnitsAreTheSame<U1, U2>(CalcValue<U1> x, CalcValue<U2> y)
    {
        if (x.Unit != y.Unit)
        {
            throw new UnitsNotSameException(x.DisplayName, y.DisplayName, x.Unit, y.Unit);
        }
    }

    public override string ToString()
    {
        string number;
        switch (Value)
        {
            case double d:
                number = d.ToString(CultureInfo.InvariantCulture);
                break;

            default:
                number = Value.ToString();
                break;
        }

        return $"{number}\u2009{Unit}";
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        if (obj is CalcValue<T> other)
        {
            return Equals(other);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return DisplayName.GetHashCode() ^ Symbol.GetHashCode() ^ Status.GetHashCode()
            ^ Value.GetHashCode() ^ Unit.GetHashCode();
    }

    public bool Equals(CalcValue<T> other)
    {

        if (object.ReferenceEquals(other, null))
        {
            return false;
        }

        return Value.Equals(other.Value) && Unit == other.Unit;
    }
}
