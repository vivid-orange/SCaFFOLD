using System.Numerics;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcArea : CalcQuantity<Area>,
    IParsable<CalcArea>,
    IAdditionOperators<CalcArea, CalcArea, CalcArea>,
    IAdditionOperators<CalcArea, double, CalcArea>,
    IAdditiveIdentity<CalcArea, CalcArea>,
    ISubtractionOperators<CalcArea, CalcArea, CalcArea>,
    ISubtractionOperators<CalcArea, double, CalcArea>,
    IMultiplyOperators<CalcArea, CalcLength, CalcVolume>,
    IMultiplyOperators<CalcArea, CalcArea, CalcInertia>,
    IMultiplyOperators<CalcArea, double, CalcArea>,
    IDivisionOperators<CalcArea, CalcLength, CalcLength>,
    IDivisionOperators<CalcArea, double, CalcArea>,
    IDivisionOperators<CalcArea, CalcArea, CalcDouble>,
    IUnaryNegationOperators<CalcArea, CalcArea>,
    IComparisonOperators<CalcArea, CalcArea, bool>
{
    public CalcArea(Area area, string name, string symbol = "")
        : base(area, name, symbol) { }

    public CalcArea(double value, AreaUnit unit, string name, string symbol)
        : base(new Area(value, unit), name, symbol) { }

    public static implicit operator CalcArea(Area value) => new CalcArea(value, string.Empty);

    #region AdditionOperators
    public static CalcArea operator +(CalcArea x, CalcArea y)
    {
        (string name, string symbol, AreaUnit unit) = OperatorMetadataHelper<AreaUnit>(x, y, '+');
        return new CalcArea(new Area(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcArea operator +(CalcArea x, double y)
    {
        return new CalcArea(x.Value + y, (AreaUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcArea operator +(double x, CalcArea y) => y + x;
    #endregion

    #region SubtractionOperators
    public static CalcArea operator -(CalcArea x)
    {
        return new CalcArea(-(Area)x.Quantity, $"-{x.DisplayName}", x.Symbol);
    }

    public static CalcArea operator -(CalcArea x, double y)
    {
        return new CalcArea(x.Value - y, (AreaUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcArea operator -(CalcArea x, CalcArea y)
    {
        (string name, string symbol, AreaUnit unit) = OperatorMetadataHelper<AreaUnit>(x, y, '-');
        return new CalcArea(new Area(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }
    #endregion

    #region MultiplicationOperators
    public static CalcArea operator *(CalcArea x, double y)
    {
        return new CalcArea(x.Value * y, (AreaUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcArea operator *(double x, CalcArea y) => y * x;

    public static CalcVolume operator *(CalcArea x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        AreaUnit unit = (AreaUnit)x.Quantity.Unit;
        return new CalcVolume(new Volume(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantVolumeUnit()), name, "");
    }

    public static CalcInertia operator *(CalcArea x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        AreaUnit unit = (AreaUnit)x.Quantity.Unit;
        return new CalcInertia(new AreaMomentOfInertia(x.Quantity.As(unit) * y.Quantity.As(unit),
            unit.GetEquivilantInertiaUnit()), name, "");
    }
    #endregion

    #region DivisionOperators
    public static CalcDouble operator /(CalcArea x, CalcArea y)
    {
        (string name, string _, AreaUnit _) = OperatorMetadataHelper<AreaUnit>(x, y, '/');
        return new CalcDouble((Area)x.Quantity / (Area)y.Quantity, name, string.Empty);
    }

    public static CalcArea operator /(CalcArea x, double y)
    {
        return new CalcArea(x.Value / y, (AreaUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLength operator /(CalcArea x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009/\u2009{y.DisplayName}";
        AreaUnit unit = (AreaUnit)x.Quantity.Unit;
        LengthUnit lengthUnit = unit.GetEquivilantLengthUnit();
        return new CalcLength(new Length(x.Quantity.As(unit) / y.Quantity.As(lengthUnit), lengthUnit), name, "");
    }
    #endregion

    #region PowerOperators
    public static ICalcQuantity operator ^(CalcArea x, int y)
    {
        if (y == 2)
        {
            AreaUnit unit = (AreaUnit)x.Quantity.Unit;
            string name = string.IsNullOrEmpty(x.DisplayName) ? string.Empty : $"{x.DisplayName}²";
            return new CalcInertia(Math.Pow(x.Value, y), unit.GetEquivilantInertiaUnit(), name, "");
        }

        throw new MathException("CalcArea can only be raised by the power of 2");
    }

    public static ICalcQuantity operator ^(CalcArea x, double y)
    {
        if (y == 2)
        {
            return x ^ 2;
        }
        else if (y == 0.5)
        {
            return x.Sqrt();
        }

        throw new MathException("CalcArea can only be raised by the power of 2 or 0.5");
    }
    #endregion

    #region ComparisonOperators
    public static bool operator >(CalcArea left, CalcArea right) => GreaterThan(left, right);
    public static bool operator >=(CalcArea left, CalcArea right) => GreaterOrEqualThan(left, right);
    public static bool operator <(CalcArea left, CalcArea right) => LessThan(left, right);
    public static bool operator <=(CalcArea left, CalcArea right) => LessOrEqualThan(left, right);
    public static bool operator ==(CalcArea left, CalcArea right) => left.Equals(right);
    public static bool operator !=(CalcArea left, CalcArea right) => !left.Equals(right);
    #endregion

    public static bool TryParse(string str, IFormatProvider provider, out CalcArea result)
    {
        if (Area.TryParse(str, provider, out Area quantity))
        {
            result = new CalcArea(quantity, string.Empty);
            return true;
        }

        result = null;
        return false;
    }

    public static CalcArea Parse(string str, IFormatProvider provider)
    {
        return new CalcArea(Area.Parse(str, provider), string.Empty);
    }

    public static CalcArea Zero => new CalcArea(Area.Zero, string.Empty);
    public static CalcArea AdditiveIdentity => Zero;
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
    public CalcLength Sqrt()
    {
        AreaUnit unit = (AreaUnit)Quantity.Unit;
        string name = string.IsNullOrEmpty(DisplayName) ? string.Empty : $"√{DisplayName}";
        return new CalcLength(Math.Sqrt(Value), unit.GetEquivilantLengthUnit(), name, "");
    }
}
