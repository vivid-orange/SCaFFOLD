namespace Scaffold.Core.Abstract;

public abstract class CalcQuantity<T> : ICalcQuantity, IEquatable<CalcQuantity<T>> where T : IQuantity
{
    public virtual IQuantity Quantity
    {
        get { return _quantity; }
        set
        {
            if (_quantity != null && !value.Dimensions.Equals(_quantity.Dimensions))
            {
                throw new UnitsNotSameException(_quantity, value);
            }

            _quantity = value;
        }
    }
    public string Unit => GetUnit();
    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public CalcStatus Status { get; set; }
    public double Value => (double)Quantity.Value;
    private IQuantity _quantity;

    public CalcQuantity(T quantity, string name, string symbol)
    {
        Quantity = quantity;
        DisplayName = name;
        Symbol = symbol;
    }

    public static implicit operator T(CalcQuantity<T> value) => (T)value.Quantity;
    public static implicit operator double(CalcQuantity<T> value) => value.Value;

    public static bool GreaterThan(CalcQuantity<T> value, CalcQuantity<T> other)
    {
        return value.Value > other.Quantity.As(value.Quantity.Unit);
    }

    public static bool LessThan(CalcQuantity<T> value, CalcQuantity<T> other)
    {
        return value.Value < other.Quantity.As(value.Quantity.Unit);
    }

    public static bool GreaterOrEqualThan(CalcQuantity<T> value, CalcQuantity<T> other)
    {
        return value.Value >= other.Quantity.As(value.Quantity.Unit);
    }

    public static bool LessOrEqualThan(CalcQuantity<T> value, CalcQuantity<T> other)
    {
        return value.Value <= other.Quantity.As(value.Quantity.Unit);
    }

    public bool SetValue(string strValue)
    {
        try
        {
            IQuantity quantity = UnitsNet.Quantity.Parse(CultureInfo.InvariantCulture, _quantity.QuantityInfo.ValueType, strValue);
            _quantity = (T)quantity;
            return true;
        }
        catch { }

        if (double.TryParse(strValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
        {
            _quantity = UnitsNet.Quantity.From(val, _quantity.Unit);
            return true;
        }

        return false;
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

        if (obj is CalcQuantity<T> other)
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

    public bool Equals(CalcQuantity<T> other)
    {
        if (Value == other?.Quantity.As(Quantity.Unit))
        {
            return true;
        }

        return false;
    }

    public string GetValue() => ToString();
    public override string ToString() => Quantity.ToString(CultureInfo.InvariantCulture).Replace(" ", "\u2009");

    internal static (string name, string symbol, U unit) OperatorMetadataHelper<U>(
        CalcQuantity<T> x, CalcQuantity<T> y, char operation) where U : Enum
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009{operation}\u2009{y.DisplayName}";
        string symbol = x.Symbol == y.Symbol ? x.Symbol : string.Empty;
        U unit = (U)x.Quantity.Unit;
        return (name, symbol, unit);
    }

    private string GetUnit()
    {
        if (Quantity != null)
        {
            string[] quantity = Quantity.ToString().Split(' ');
            if (quantity.Count() > 1)
            {
                return quantity[1];
            }
        }

        return "-";
    }
}
