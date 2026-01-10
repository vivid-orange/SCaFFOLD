using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public sealed class CalcInt : CalcValue<int>
{
    public CalcInt(int value)
        : base(value, string.Empty, string.Empty, string.Empty) { }

    public CalcInt(int value, string name)
        : base(value, name, string.Empty, string.Empty) { }

    public CalcInt(int value, string name, string symbol, string unit = "")
        : base(value, name, symbol, unit) { }

    public static implicit operator CalcInt(int value) => new CalcInt(value, string.Empty);

    public static bool operator >(CalcInt value, CalcInt other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value > other.Value;
    }

    public static bool operator <(CalcInt value, CalcInt other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value < other.Value;
    }

    public static bool operator >(CalcInt value, CalcDouble other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value > other.Value;
    }

    public static bool operator >(CalcInt value, double other)
    {
        return value.Value > other;
    }

    public static bool operator >(CalcInt value, int other)
    {
        return value.Value > other;
    }

    public static bool operator <(CalcInt value, int other)
    {
        return value.Value < other;
    }

    public static bool operator <(CalcInt value, CalcDouble other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value < other.Value;
    }

    public static bool operator <(CalcInt value, double other)
    {
        return value.Value < other;
    }

    public static bool operator >=(CalcInt value, CalcInt other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value >= other.Value;
    }

    public static bool operator >=(CalcInt value, int other)
    {
        return value.Value >= other;
    }

    public static bool operator <=(CalcInt value, CalcInt other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value <= other.Value;
    }

    public static bool operator <=(CalcInt value, int other)
    {
        return value.Value <= other;
    }

    public static bool operator >=(CalcInt value, CalcDouble other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value >= other.Value;
    }

    public static bool operator >=(CalcInt value, double other)
    {
        return value.Value >= other;
    }

    public static bool operator <=(CalcInt value, CalcDouble other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value <= other.Value;
    }

    public static bool operator <=(CalcInt value, double other)
    {
        return value.Value <= other;
    }

    public static CalcInt operator +(CalcInt x, CalcInt y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '+');
        return new CalcInt(x.Value + y.Value, name, symbol, unit);
    }

    public static CalcInt operator -(CalcInt x, CalcInt y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '-');
        return new CalcInt(x.Value - y.Value, name, symbol, unit);
    }

    public static CalcInt operator *(CalcInt x, CalcInt y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '·');
        if (!string.IsNullOrEmpty(unit))
        {
            unit = $"{unit}²";
        }

        return new CalcInt(x.Value * y.Value, name, symbol, unit);
    }

    public static CalcInt operator /(CalcInt x, CalcInt y)
    {
        (string name, string symbol, string _) = OperatorMetadataHelper(x, y, '/');
        return new CalcInt(x.Value / y.Value, name, symbol);
    }

    public static CalcInt operator *(CalcInt x, int y)
    {
        return new CalcInt(x.Value * y, x.TypeName, x.Symbol, x.Unit);
    }

    public static CalcInt operator *(int x, CalcInt y)
    {
        return y * x;
    }

    public static CalcInt operator /(CalcInt x, int y)
    {
        return new CalcInt(x.Value / y, x.TypeName, x.Symbol, x.Unit);
    }
}
