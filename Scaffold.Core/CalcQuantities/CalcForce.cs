using System.Numerics;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcForce : CalcQuantity<Force>,
    IParsable<CalcForce>,
    IAdditionOperators<CalcForce, CalcForce, CalcForce>,
    IAdditionOperators<CalcForce, double, CalcForce>,
    IAdditiveIdentity<CalcForce, CalcForce>,
    ISubtractionOperators<CalcForce, CalcForce, CalcForce>,
    ISubtractionOperators<CalcForce, double, CalcForce>,
    IMultiplyOperators<CalcForce, CalcLength, CalcMoment>,
    IMultiplyOperators<CalcForce, double, CalcForce>,
    IDivisionOperators<CalcForce, CalcLength, CalcLinearForce>,
    IDivisionOperators<CalcForce, CalcArea, CalcStress>,
    IDivisionOperators<CalcForce, double, CalcForce>,
    IDivisionOperators<CalcForce, CalcForce, CalcDouble>,
    IUnaryNegationOperators<CalcForce, CalcForce>,
    IComparisonOperators<CalcForce, CalcForce, bool>
{
    public CalcForce(Force force, string name, string symbol = "")
        : base(force, name, symbol) { }

    public CalcForce(double value, ForceUnit unit, string name, string symbol)
        : base(new Force(value, unit), name, symbol) { }

    public CalcForce(double value, string name, string symbol)
        : base(new Force(value, ForceUnit.Kilonewton), name, symbol) { }

    public static implicit operator CalcForce(Force value) => new CalcForce(value, string.Empty);

    #region AdditionOperators
    public static CalcForce operator +(CalcForce x, CalcForce y)
    {
        (string name, string symbol, ForceUnit unit) = OperatorMetadataHelper<ForceUnit>(x, y, '+');
        return new CalcForce(new Force(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcForce operator +(CalcForce x, double y)
    {
        return new CalcForce(x.Value + y, (ForceUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcForce operator +(double x, CalcForce y) => y + x;
    #endregion

    #region SubtractionOperators
    public static CalcForce operator -(CalcForce x)
    {
        return new CalcForce(-(Force)x.Quantity, $"-{x.DisplayName}", x.Symbol);
    }

    public static CalcForce operator -(CalcForce x, double y)
    {
        return new CalcForce(x.Value - y, (ForceUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcForce operator -(CalcForce x, CalcForce y)
    {
        (string name, string symbol, ForceUnit unit) = OperatorMetadataHelper<ForceUnit>(x, y, '-');
        return new CalcForce(new Force(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }
    #endregion

    #region MultiplicationOperators
    public static CalcMoment operator *(CalcForce x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        ForceUnit unit = (ForceUnit)x.Quantity.Unit;
        return new CalcMoment(new Moment(x.Quantity.As(unit) * y.Value,
            unit.GetEquivilantMomentUnit((LengthUnit)y.Quantity.Unit)), name, "");
    }

    public static CalcMoment operator *(CalcLength x, CalcForce y) => y * x;

    public static CalcForce operator *(CalcForce x, double y)
    {
        return new CalcForce(x.Value * y, (ForceUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcForce operator *(double x, CalcForce y) => y * x;
    #endregion

    #region DivisionOperators
    public static CalcLinearForce operator /(CalcForce x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009/\u2009{y.DisplayName}";
        ForceUnit unit = (ForceUnit)x.Quantity.Unit;
        return new CalcLinearForce(new ForcePerLength(x.Quantity.As(unit) / y.Value,
            unit.GetEquivilantForcePerLengthUnit((LengthUnit)y.Quantity.Unit)), name, "");
    }

    public static CalcStress operator /(CalcForce x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
            ? string.Empty : $"{x.DisplayName}\u2009/\u2009{y.DisplayName}";
        ForceUnit unit = (ForceUnit)x.Quantity.Unit;
        return new CalcStress(new Pressure(x.Quantity.As(unit) / y.Value,
            unit.GetEquivilantPressureUnit((AreaUnit)y.Quantity.Unit)), name, "");
    }

    public static CalcDouble operator /(CalcForce x, CalcForce y)
    {
        (string name, string _, ForceUnit _) = OperatorMetadataHelper<ForceUnit>(x, y, '/');
        return new CalcDouble((Force)x.Quantity / (Force)y.Quantity, name, string.Empty);
    }

    public static CalcForce operator /(CalcForce x, double y)
    {
        return new CalcForce(x.Value / y, (ForceUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
    #endregion

    #region ComparisonOperators
    public static bool operator >(CalcForce left, CalcForce right) => GreaterThan(left, right);
    public static bool operator >=(CalcForce left, CalcForce right) => GreaterOrEqualThan(left, right);
    public static bool operator <(CalcForce left, CalcForce right) => LessThan(left, right);
    public static bool operator <=(CalcForce left, CalcForce right) => LessOrEqualThan(left, right);
    public static bool operator ==(CalcForce left, CalcForce right) => left.Equals(right);
    public static bool operator !=(CalcForce left, CalcForce right) => !left.Equals(right);
    #endregion

    public static bool TryParse(string str, IFormatProvider provider, out CalcForce result)
    {
        if (Force.TryParse(str, provider, out Force quantity))
        {
            result = new CalcForce(quantity, string.Empty);
            return true;
        }

        result = null;
        return false;
    }

    public static CalcForce Parse(string str, IFormatProvider provider)
    {
        return new CalcForce(Force.Parse(str, provider), string.Empty);
    }

    public static CalcForce Zero => new CalcForce(Force.Zero, string.Empty);
    public static CalcForce AdditiveIdentity => Zero;
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
}
