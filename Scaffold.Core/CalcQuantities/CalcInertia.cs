#if NET7_0_OR_GREATER
using System.Numerics;
#endif
using Scaffold.Core.CalcValues;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcInertia : CalcSIQuantity<AreaMomentOfInertia>
#if NET7_0_OR_GREATER
    , IParsable<CalcInertia>
    , IAdditionOperators<CalcInertia, CalcInertia, CalcInertia>
    , IAdditionOperators<CalcInertia, double, CalcInertia>
    , IAdditiveIdentity<CalcInertia, CalcInertia>
    , ISubtractionOperators<CalcInertia, CalcInertia, CalcInertia>
    , ISubtractionOperators<CalcInertia, double, CalcInertia>
    , IMultiplyOperators<CalcInertia, double, CalcInertia>
    , IDivisionOperators<CalcInertia, CalcLength, CalcVolume>
    , IDivisionOperators<CalcInertia, CalcArea, CalcArea>
    , IDivisionOperators<CalcInertia, CalcVolume, CalcLength>
    , IDivisionOperators<CalcInertia, double, CalcInertia>
    , IDivisionOperators<CalcInertia, CalcInertia, CalcDouble>
    , IUnaryNegationOperators<CalcInertia, CalcInertia>
    , IComparisonOperators<CalcInertia, CalcInertia, bool>
#endif
{
    public CalcInertia(AreaMomentOfInertia areaMomentOfInertia, string name, string symbol = "")
        : base(areaMomentOfInertia, name, symbol) { }

    public CalcInertia(double value, AreaMomentOfInertiaUnit unit, string name, string symbol)
        : base(new AreaMomentOfInertia(value, unit), name, symbol) { }

    public static implicit operator CalcInertia(AreaMomentOfInertia value) => new CalcInertia(value, string.Empty);

    #region AdditionOperators
    public static CalcInertia operator +(CalcInertia x, CalcInertia y)
    {
        (string name, string symbol, AreaMomentOfInertiaUnit unit)
            = OperatorMetadataHelper<AreaMomentOfInertiaUnit>(x, y, '+');
        return new CalcInertia(new AreaMomentOfInertia(x.Quantity.As(unit) + y.Quantity.As(unit),
            unit), name, symbol);
    }

    public static CalcInertia operator +(CalcInertia x, double y)
    {
        return new CalcInertia(x.Value + y, (AreaMomentOfInertiaUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }

    public static CalcInertia operator +(double x, CalcInertia y) => y + x;
    #endregion

    #region SubtractionOperators
    public static CalcInertia operator -(CalcInertia x)
    {
        return new CalcInertia(-(AreaMomentOfInertia)x.Quantity, $"-{x.TypeName}", x.Symbol);
    }

    public static CalcInertia operator -(CalcInertia x, CalcInertia y)
    {
        (string name, string symbol, AreaMomentOfInertiaUnit unit)
            = OperatorMetadataHelper<AreaMomentOfInertiaUnit>(x, y, '-');
        return new CalcInertia(new AreaMomentOfInertia(x.Quantity.As(unit) - y.Quantity.As(unit),
            unit), name, symbol);
    }
    public static CalcInertia operator -(CalcInertia x, double y)
    {
        return new CalcInertia(x.Value - y, (AreaMomentOfInertiaUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }
    #endregion

    #region MultiplicationOperators
    public static CalcInertia operator *(CalcInertia x, double y)
    {
        return new CalcInertia(x.Value * y, (AreaMomentOfInertiaUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }

    public static CalcInertia operator *(double x, CalcInertia y) => y * x;
    #endregion

    #region DivisionOperators
    public static CalcDouble operator /(CalcInertia x, CalcInertia y)
    {
        (string name, _, AreaMomentOfInertiaUnit _) = OperatorMetadataHelper<AreaMomentOfInertiaUnit>(x, y, '/');
        return new CalcDouble((AreaMomentOfInertia)x.Quantity / (AreaMomentOfInertia)y.Quantity, name, string.Empty);
    }

    public static CalcVolume operator /(CalcInertia x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.TypeName) || string.IsNullOrEmpty(y.TypeName)
            ? string.Empty : $"{x.TypeName}\u2009/\u2009{y.TypeName}";
        AreaMomentOfInertiaUnit unit = (AreaMomentOfInertiaUnit)x.Quantity.Unit;
        return new CalcVolume(new Volume(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantVolumeUnit()), name, "");
    }

    public static CalcArea operator /(CalcInertia x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.TypeName) || string.IsNullOrEmpty(y.TypeName)
            ? string.Empty : $"{x.TypeName}\u2009/\u2009{y.TypeName}";
        AreaMomentOfInertiaUnit unit = (AreaMomentOfInertiaUnit)x.Quantity.Unit;
        AreaUnit areaUnit = unit.GetEquivilantAreaUnit();
        return new CalcArea(new Area(x.Quantity.As(unit) / y.Quantity.As(areaUnit), areaUnit), name, "");
    }

    public static CalcLength operator /(CalcInertia x, CalcVolume y)
    {
        string name = string.IsNullOrEmpty(x.TypeName) || string.IsNullOrEmpty(y.TypeName)
            ? string.Empty : $"{x.TypeName}\u2009/\u2009{y.TypeName}";
        AreaMomentOfInertiaUnit unit = (AreaMomentOfInertiaUnit)x.Quantity.Unit;
        return new CalcLength(new Length(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantVolumeUnit()),
            unit.GetEquivilantLengthUnit()), name, "");
    }

    public static CalcInertia operator /(CalcInertia x, double y)
    {
        return new CalcInertia(x.Value / y, (AreaMomentOfInertiaUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }
    #endregion

    #region PowerOperators
    public static ICalcSIQuantity operator ^(CalcInertia x, double y)
    {
        if (y == 0.5)
        {
            return x.Sqrt();
        }

        throw new MathException("CalcInertia can only be raised by the power of 0.5");
    }
    #endregion

    #region ComparisonOperators
    public static bool operator >(CalcInertia left, CalcInertia right) => GreaterThan(left, right);
    public static bool operator >=(CalcInertia left, CalcInertia right) => GreaterOrEqualThan(left, right);
    public static bool operator <(CalcInertia left, CalcInertia right) => LessThan(left, right);
    public static bool operator <=(CalcInertia left, CalcInertia right) => LessOrEqualThan(left, right);
    public static bool operator ==(CalcInertia left, CalcInertia right) => left.Equals(right);
    public static bool operator !=(CalcInertia left, CalcInertia right) => !left.Equals(right);
    #endregion

    public static bool TryParse(string str, IFormatProvider provider, out CalcInertia result)
    {
        if (AreaMomentOfInertia.TryParse(str, provider, out AreaMomentOfInertia quantity))
        {
            result = new CalcInertia(quantity, string.Empty);
            return true;
        }

        result = null;
        return false;
    }

    public static CalcInertia Parse(string str, IFormatProvider provider)
    {
        return new CalcInertia(AreaMomentOfInertia.Parse(str, provider), string.Empty);
    }

    public static CalcInertia Zero => new CalcInertia(AreaMomentOfInertia.Zero, string.Empty);
    public static CalcInertia AdditiveIdentity => Zero;
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
    public CalcArea Sqrt()
    {
        AreaMomentOfInertiaUnit unit = (AreaMomentOfInertiaUnit)Quantity.Unit;
        string name = string.IsNullOrEmpty(TypeName) ? string.Empty : $"√{TypeName}";
        return new CalcArea(Math.Sqrt(Value), unit.GetEquivilantAreaUnit(), name, "");
    }
}
