namespace Scaffold.Core.CalcValues;

public class CalcSIQuantity<T> : ICalcSIQuantity, IEquatable<CalcSIQuantity<T>> where T : IQuantity
{
    public virtual T Quantity
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
    public IQuantity GenericQuantity
    {
        get => _quantity;
    }

    public string Unit => GetUnit();
    public string TypeName { get; set; }
    public string Symbol { get; }
    public CalcStatus Status { get; }
    public double Value
    {
        get => (double)_quantity.Value;
        set => _quantity = (T)UnitsNet.Quantity.From(value, _quantity.Unit);
    }
    private T _quantity;

    public CalcSIQuantity(T quantity, string name, string symbol)
    {
        Quantity = quantity;
        TypeName = name;
        Symbol = symbol;
    }

    public CalcSIQuantity(string name, string symbol, T quantity)
    {
        Quantity = quantity;
        TypeName= name;
        Symbol = symbol;
    }

    public static implicit operator T(CalcSIQuantity<T> value) => value.Quantity;
    public static implicit operator double(CalcSIQuantity<T> value) => value.Value;

    public static bool GreaterThan(CalcSIQuantity<T> value, CalcSIQuantity<T> other)
    {
        return value.Value > other.Quantity.As(value.Quantity.Unit);
    }

    public static bool LessThan(CalcSIQuantity<T> value, CalcSIQuantity<T> other)
    {
        return value.Value < other.Quantity.As(value.Quantity.Unit);
    }

    public static bool GreaterOrEqualThan(CalcSIQuantity<T> value, CalcSIQuantity<T> other)
    {
        return value.Value >= other.Quantity.As(value.Quantity.Unit);
    }

    public static bool LessOrEqualThan(CalcSIQuantity<T> value, CalcSIQuantity<T> other)
    {
        return value.Value <= other.Quantity.As(value.Quantity.Unit);
    }

    public bool TryParse(string strValue)
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
            _quantity = (T)UnitsNet.Quantity.From(val, _quantity.Unit);
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

        if (obj is CalcSIQuantity<T> other)
        {
            return Equals(other);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return TypeName.GetHashCode() ^ Symbol.GetHashCode() ^ Status.GetHashCode()
            ^ Value.GetHashCode() ^ Unit.GetHashCode();
    }

    public bool Equals(CalcSIQuantity<T> other)
    {
        if (Value == other?.Quantity.As(Quantity.Unit))
        {
            return true;
        }

        return false;
    }

    public string GetValueAsString() => ToString();
    public override string ToString() => Quantity.Value.ToString(CultureInfo.InvariantCulture).Replace(" ", "\u2009");

    internal static (string name, string symbol, U unit) OperatorMetadataHelper<U>(
        CalcSIQuantity<T> x, CalcSIQuantity<T> y, char operation) where U : Enum
    {
        string name = string.IsNullOrEmpty(x.TypeName) || string.IsNullOrEmpty(y.TypeName)
            ? string.Empty : $"{x.TypeName}\u2009{operation}\u2009{y.TypeName}";
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
