using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public sealed class CalcDouble : CalcValue<double>
{
    public CalcDouble(double value)
        : base(value, string.Empty, string.Empty, string.Empty) { }

    public CalcDouble(double value, string name)
        : base(value, name, string.Empty, string.Empty) { }

    public CalcDouble(double value, string name, string symbol, string unit = "")
        : base(value, name, symbol, unit) { }

    public static implicit operator CalcDouble(double value) => new CalcDouble(value, string.Empty);
    public static bool operator >(CalcDouble value, CalcDouble other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value > other.Value;
    }

    public static bool operator <(CalcDouble value, CalcDouble other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value < other.Value;
    }

    public static bool operator >=(CalcDouble value, CalcDouble other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value >= other.Value;
    }

    public static bool operator <=(CalcDouble value, CalcDouble other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value <= other.Value;
    }

    public static bool operator >(CalcDouble value, CalcInt other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value > other.Value;
    }

    public static bool operator >(CalcDouble value, int other)
    {
        return value.Value > other;
    }

    public static bool operator <(CalcDouble value, CalcInt other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value < other.Value;
    }

    public static bool operator <(CalcDouble value, int other)
    {
        return value.Value < other;
    }

    public static bool operator >=(CalcDouble value, CalcInt other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value >= other.Value;
    }

    public static bool operator >=(CalcDouble value, int other)
    {
        return value.Value >= other;
    }

    public static bool operator <=(CalcDouble value, CalcInt other)
    {
        CheckUnitsAreTheSame(value, other);
        return value.Value <= other.Value;
    }

    public static bool operator <=(CalcDouble value, int other)
    {
        return value.Value <= other;
    }

    public static CalcDouble operator +(CalcDouble x, CalcDouble y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '+');
        return new CalcDouble(x.Value + y.Value, name, symbol, unit);
    }

    public static CalcDouble operator +(CalcDouble x, double y)
    {
        return new CalcDouble(x.Value + y, x.TypeName, x.Symbol, x.Unit);
    }

    public static CalcDouble operator +(double x, CalcDouble y)
    {
        return y + x;
    }

    public static CalcDouble operator +(int x, CalcDouble y)
    {
        return y + (double)x;
    }

    public static CalcDouble operator -(CalcDouble x, CalcDouble y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '-');
        return new CalcDouble(x.Value - y.Value, name, symbol, unit);
    }

    public static CalcDouble operator -(CalcDouble x, double y)
    {
        return new CalcDouble(x.Value - y, x.TypeName, x.Symbol, x.Unit);
    }

    public static CalcDouble operator -(double x, CalcDouble y)
    {
        return new CalcDouble(x - y.Value, y.TypeName, y.Symbol, y.Unit);
    }

    public static CalcDouble operator +(CalcDouble x, CalcInt y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '+');
        return new CalcDouble(x.Value + y.Value, name, symbol, unit);
    }

    public static CalcDouble operator -(CalcDouble x, CalcInt y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '-');
        return new CalcDouble(x.Value - y.Value, name, symbol, unit);
    }

    public static CalcDouble operator *(CalcDouble x, CalcDouble y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '·');
        if (!string.IsNullOrEmpty(unit))
        {
            unit = $"{unit}²";
        }

        return new CalcDouble(x.Value * y.Value, name, symbol, unit);
    }

    public static CalcDouble operator /(CalcDouble x, CalcDouble y)
    {
        (string name, string symbol, string _) = OperatorMetadataHelper(x, y, '/');
        return new CalcDouble(x.Value / y.Value, name, symbol);
    }

    public static CalcDouble operator *(CalcDouble x, CalcInt y)
    {
        (string name, string symbol, string unit) = OperatorMetadataHelper(x, y, '·');
        if (!string.IsNullOrEmpty(unit))
        {
            unit = $"{unit}²";
        }

        return new CalcDouble(x.Value * y.Value, name, symbol, unit);
    }

    public static CalcDouble operator *(CalcInt x, CalcDouble y)
    {
        return y * x;
    }

    public static CalcDouble operator /(CalcDouble x, CalcInt y)
    {
        (string name, string symbol, string _) = OperatorMetadataHelper(x, y, '/');
        return new CalcDouble(x.Value / y.Value, name, symbol);
    }

    public static CalcDouble operator /(CalcInt x, CalcDouble y)
    {
        (string name, string symbol, string _) = OperatorMetadataHelper(x, y, '/');
        return new CalcDouble(x.Value / y.Value, name, symbol);
    }

    public static CalcDouble operator *(CalcDouble x, double y)
    {
        return new CalcDouble(x.Value * y, x.TypeName, x.Symbol, x.Unit);
    }

    public static CalcDouble operator *(double x, CalcDouble y)
    {
        return y * x;
    }

    public static CalcDouble operator /(CalcDouble x, double y)
    {
        return new CalcDouble(x.Value / y, x.TypeName, x.Symbol, x.Unit);
    }

    public static CalcDouble operator /(double x, CalcDouble y)
    {
        return new CalcDouble(x / y.Value, y.TypeName, y.Symbol, y.Unit);
    }

    public static CalcDouble operator *(CalcDouble x, int y)
    {
        return new CalcDouble(x.Value * y, x.TypeName, x.Symbol, x.Unit);
    }

    public static CalcDouble operator *(int x, CalcDouble y)
    {
        return y * x;
    }

    public static CalcDouble operator /(CalcDouble x, int y)
    {
        return new CalcDouble(x.Value / y, x.TypeName, x.Symbol, x.Unit);
    }

    public static CalcDouble operator /(int x, CalcDouble y)
    {
        return new CalcDouble(x / y.Value, y.TypeName, y.Symbol, y.Unit);
    }
}
