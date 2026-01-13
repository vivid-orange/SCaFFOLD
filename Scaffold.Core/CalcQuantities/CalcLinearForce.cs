using System.Numerics;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcLinearForce : CalcQuantity<ForcePerLength>,
    IParsable<CalcLinearForce>,
    IAdditionOperators<CalcLinearForce, CalcLinearForce, CalcLinearForce>,
    IAdditionOperators<CalcLinearForce, double, CalcLinearForce>,
    IAdditiveIdentity<CalcLinearForce, CalcLinearForce>,
    ISubtractionOperators<CalcLinearForce, CalcLinearForce, CalcLinearForce>,
    ISubtractionOperators<CalcLinearForce, double, CalcLinearForce>,
    IMultiplyOperators<CalcLinearForce, CalcLength, CalcForce>,
    IMultiplyOperators<CalcLinearForce, double, CalcLinearForce>,
    IDivisionOperators<CalcLinearForce, CalcLength, CalcStress>,
    IDivisionOperators<CalcLinearForce, double, CalcLinearForce>,
    IDivisionOperators<CalcLinearForce, CalcLinearForce, CalcDouble>,
    IUnaryNegationOperators<CalcLinearForce, CalcLinearForce>,
    IComparisonOperators<CalcLinearForce, CalcLinearForce, bool>
{
    public CalcLinearForce(ForcePerLength linearForce, string name, string symbol = "")
        : base(linearForce, name, symbol) { }

    public CalcLinearForce(double value, ForcePerLengthUnit unit, string name, string symbol)
        : base(new ForcePerLength(value, unit), name, symbol) { }

    public CalcLinearForce(double value, string name, string symbol)
        : base(new ForcePerLength(value, ForcePerLengthUnit.KilonewtonPerMeter), name, symbol) { }

    public static implicit operator CalcLinearForce(ForcePerLength value) => new CalcLinearForce(value, string.Empty);

    #region AdditionOperators
    public static CalcLinearForce operator +(CalcLinearForce x, CalcLinearForce y)
    {
        (string name, string symbol, ForcePerLengthUnit unit) = OperatorMetadataHelper<ForcePerLengthUnit>(x, y, '+');
        return new CalcLinearForce(new ForcePerLength(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }
    public static CalcLinearForce operator +(CalcLinearForce x, double y)
    {
        return new CalcLinearForce(x.Value + y, (ForcePerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLinearForce operator +(double x, CalcLinearForce y) => y + x;
    #endregion

    #region SubtractionOperators
    public static CalcLinearForce operator -(CalcLinearForce x)
    {
        return new CalcLinearForce(-(ForcePerLength)x.Quantity, $"-{x.DisplayName}", x.Symbol);
    }
    public static CalcLinearForce operator -(CalcLinearForce x, CalcLinearForce y)
    {
        (string name, string symbol, ForcePerLengthUnit unit) = OperatorMetadataHelper<ForcePerLengthUnit>(x, y, '-');
        return new CalcLinearForce(new ForcePerLength(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcLinearForce operator -(CalcLinearForce x, double y)
    {
        return new CalcLinearForce(x.Value - y, (ForcePerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
    #endregion

    #region MultiplicationOperators
    public static CalcForce operator *(CalcLinearForce x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
           ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        ForcePerLengthUnit unit = (ForcePerLengthUnit)x.Quantity.Unit;
        return new CalcForce(new Force(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantForceUnit()), name, "");
    }

    public static CalcForce operator *(CalcLength x, CalcLinearForce y) => y * x;

    public static CalcLinearForce operator *(CalcLinearForce x, double y)
    {
        return new CalcLinearForce(x.Value * y, (ForcePerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLinearForce operator *(double x, CalcLinearForce y) => y * x;
    #endregion

    #region DivisionOperators
    public static CalcStress operator /(CalcLinearForce x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
           ? string.Empty : $"{x.DisplayName}\u2009/\u2009{y.DisplayName}";
        ForcePerLengthUnit unit = (ForcePerLengthUnit)x.Quantity.Unit;
        return new CalcStress(new Pressure(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantPressureUnit()), name, "");
    }

    public static CalcDouble operator /(CalcLinearForce x, CalcLinearForce y)
    {
        (string name, string _, ForcePerLengthUnit _) = OperatorMetadataHelper<ForcePerLengthUnit>(x, y, '/');
        return new CalcDouble((ForcePerLength)x.Quantity / (ForcePerLength)y.Quantity, name, string.Empty);
    }

    public static CalcLinearForce operator /(CalcLinearForce x, double y)
    {
        return new CalcLinearForce(x.Value / y, (ForcePerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
    #endregion

    #region ComparisonOperators
    public static bool operator >(CalcLinearForce left, CalcLinearForce right) => GreaterThan(left, right);
    public static bool operator >=(CalcLinearForce left, CalcLinearForce right) => GreaterOrEqualThan(left, right);
    public static bool operator <(CalcLinearForce left, CalcLinearForce right) => LessThan(left, right);
    public static bool operator <=(CalcLinearForce left, CalcLinearForce right) => LessOrEqualThan(left, right);
    public static bool operator ==(CalcLinearForce left, CalcLinearForce right) => left.Equals(right);
    public static bool operator !=(CalcLinearForce left, CalcLinearForce right) => !left.Equals(right);
    #endregion

    public static bool TryParse(string str, IFormatProvider provider, out CalcLinearForce result)
    {
        if (ForcePerLength.TryParse(str, provider, out ForcePerLength quantity))
        {
            result = new CalcLinearForce(quantity, string.Empty);
            return true;
        }

        result = null;
        return false;
    }

    public static CalcLinearForce Parse(string str, IFormatProvider provider)
    {
        return new CalcLinearForce(ForcePerLength.Parse(str, provider), string.Empty);
    }

    public static CalcLinearForce Zero => new CalcLinearForce(ForcePerLength.Zero, string.Empty);
    public static CalcLinearForce AdditiveIdentity => Zero;
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
}
