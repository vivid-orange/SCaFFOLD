using System.Numerics;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcQuantityWrapper<T> : ICalcQuantity, IEquatable<CalcQuantityWrapper<T>>
#if NET7_0_OR_GREATER
    , IParsable<CalcQuantityWrapper<T>>
    , IAdditionOperators<CalcQuantityWrapper<T>, CalcQuantityWrapper<T>, CalcQuantityWrapper<T>>
    , IAdditionOperators<CalcQuantityWrapper<T>, double, CalcQuantityWrapper<T>>
    //, IAdditiveIdentity<CalcQuantityWrapper<T>, CalcQuantityWrapper<T>, CalcQuantityWrapper<T>>
    , ISubtractionOperators<CalcQuantityWrapper<T>, CalcQuantityWrapper<T>, CalcQuantityWrapper<T>>
    , ISubtractionOperators<CalcQuantityWrapper<T>, double, CalcQuantityWrapper<T>>
    //, IMultiplyOperators<CalcQuantityWrapper<T>, double, CalcQuantityWrapper<T>>
    //, IDivisionOperators<CalcQuantityWrapper<T>, double, CalcQuantityWrapper<T>>
    , IUnaryNegationOperators<CalcQuantityWrapper<T>, CalcQuantityWrapper<T>>
    , IComparisonOperators<CalcQuantityWrapper<T>, CalcQuantityWrapper<T>, bool>
#endif
    where T : IQuantity
{
    public IQuantity Quantity
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

    public CalcQuantityWrapper(T quantity, string name, string symbol)
    {
        Quantity = quantity;
        DisplayName = name;
        Symbol = symbol;
    }

    public static implicit operator T(CalcQuantityWrapper<T> value) => (T)value.Quantity;
    public static implicit operator double(CalcQuantityWrapper<T> value) => value.Value;
    public static implicit operator CalcQuantityWrapper<T>(T value) => new CalcQuantityWrapper<T>(value, string.Empty, string.Empty);

    public static CalcQuantityWrapper<T> operator +(CalcQuantityWrapper<T> x, CalcQuantityWrapper<T> y)
    {
        UnitsNet.Quantity.TryFrom(x.Value + y.Quantity.As(x.Quantity.Unit), x.Quantity.Unit, out IQuantity quantity);
        return new CalcQuantityWrapper<T>((T)quantity, string.Empty, string.Empty);
    }

    public static CalcQuantityWrapper<T> operator +(CalcQuantityWrapper<T> x, double y)
    {
        UnitsNet.Quantity.TryFrom(x.Value + y, x.Quantity.Unit, out IQuantity quantity);
        return new CalcQuantityWrapper<T>((T)quantity, string.Empty, string.Empty);
    }

    public static CalcQuantityWrapper<T> operator +(double x, CalcQuantityWrapper<T> y) => y + x;

    public static CalcQuantityWrapper<T> operator -(CalcQuantityWrapper<T> x)
    {
        UnitsNet.Quantity.TryFrom(-x.Value, x.Quantity.Unit, out IQuantity quantity);
        return new CalcQuantityWrapper<T>((T)quantity, $"-{x.DisplayName}", x.Symbol);
    }

    public static CalcQuantityWrapper<T> operator -(CalcQuantityWrapper<T> x, CalcQuantityWrapper<T> y)
    {
        UnitsNet.Quantity.TryFrom(x.Value - y.Quantity.As(x.Quantity.Unit), x.Quantity.Unit, out IQuantity quantity);
        return new CalcQuantityWrapper<T>((T)quantity, string.Empty, string.Empty);
    }

    public static CalcQuantityWrapper<T> operator -(CalcQuantityWrapper<T> x, double y)
    {
        UnitsNet.Quantity.TryFrom(x.Value - y, x.Quantity.Unit, out IQuantity quantity);
        return new CalcQuantityWrapper<T>((T)quantity, string.Empty, string.Empty);
    }

    public static bool operator >(CalcQuantityWrapper<T> value, CalcQuantityWrapper<T> other)
    {
        return value.Value > other.Quantity.As(value.Quantity.Unit);
    }

    public static bool operator <(CalcQuantityWrapper<T> value, CalcQuantityWrapper<T> other)
    {
        return value.Value < other.Quantity.As(value.Quantity.Unit);
    }

    public static bool operator >=(CalcQuantityWrapper<T> value, CalcQuantityWrapper<T> other)
    {
        return value.Value >= other.Quantity.As(value.Quantity.Unit);
    }

    public static bool operator <=(CalcQuantityWrapper<T> value, CalcQuantityWrapper<T> other)
    {
        return value.Value <= other.Quantity.As(value.Quantity.Unit);
    }
    public static bool operator ==(CalcQuantityWrapper<T> left, CalcQuantityWrapper<T> right) => left.Equals(right);
    public static bool operator !=(CalcQuantityWrapper<T> left, CalcQuantityWrapper<T> right) => !left.Equals(right);

    public static bool TryParse(string str, IFormatProvider provider, out CalcQuantityWrapper<T> result)
    {
        if (UnitsNet.Quantity.TryParse(provider, typeof(T), str, out IQuantity quantity))
        {
            result = new CalcQuantityWrapper<T>((T)quantity, string.Empty, string.Empty);
            return true;
        }

        result = null;
        return false;
    }

    public static CalcQuantityWrapper<T> Parse(string str, IFormatProvider provider)
    {
        IQuantity quantity = UnitsNet.Quantity.Parse(provider, typeof(T), str);
        return new CalcQuantityWrapper<T>((T)quantity, string.Empty, string.Empty);
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

        if (obj is CalcQuantityWrapper<T> other)
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

    public bool Equals(CalcQuantityWrapper<T> other)
    {
        if (Value == other?.Quantity.As(Quantity.Unit))
        {
            return true;
        }

        return false;
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

    public string GetValue() => ToString();
    public override string ToString() => Quantity.ToString(CultureInfo.InvariantCulture).Replace(" ", "\u2009");

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
