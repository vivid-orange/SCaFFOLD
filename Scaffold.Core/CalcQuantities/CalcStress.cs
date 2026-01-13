using System.Numerics;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcStress : CalcQuantity<Stress>,
    IParsable<CalcStress>,
    IAdditionOperators<CalcStress, CalcStress, CalcStress>,
    IAdditionOperators<CalcStress, double, CalcStress>,
    IAdditiveIdentity<CalcStress, CalcStress>,
    ISubtractionOperators<CalcStress, CalcStress, CalcStress>,
    ISubtractionOperators<CalcStress, double, CalcStress>,
    IMultiplyOperators<CalcStress, CalcLength, CalcLinearForce>,
    IMultiplyOperators<CalcStress, CalcArea, CalcForce>,
    IMultiplyOperators<CalcStress, double, CalcStress>,
    IDivisionOperators<CalcStress, double, CalcStress>,
    IDivisionOperators<CalcStress, CalcStress, CalcDouble>,
    IUnaryNegationOperators<CalcStress, CalcStress>,
    IComparisonOperators<CalcStress, CalcStress, bool>
{
    public CalcStress(Stress stress, string name, string symbol = "")
        : base(stress, name, symbol) { }

    public CalcStress(double value, StressUnit unit, string name, string symbol)
        : base(new Stress(value, unit), name, symbol) { }

    public static implicit operator CalcStress(Stress value) => new CalcStress(value, string.Empty);

    #region AdditionOperators
    public static CalcStress operator +(CalcStress x, CalcStress y)
    {
        (string name, string symbol, StressUnit unit) = OperatorMetadataHelper<StressUnit>(x, y, '+');
        return new CalcStress(new Stress(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcStress operator +(CalcStress x, double y)
    {
        return new CalcStress(x.Value + y, (StressUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcStress operator +(double x, CalcStress y) => y + x;
    #endregion

    #region SubtractionOperators
    public static CalcStress operator -(CalcStress x)
    {
        return new CalcStress(-(Stress)x.Quantity, $"-{x.DisplayName}", x.Symbol);
    }

    public static CalcStress operator -(CalcStress x, CalcStress y)
    {
        (string name, string symbol, StressUnit unit) = OperatorMetadataHelper<StressUnit>(x, y, '-');
        return new CalcStress(new Stress(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcStress operator -(CalcStress x, double y)
    {
        return new CalcStress(x.Value - y, (StressUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
    #endregion

    #region MultiplicationOperators
    public static CalcLinearForce operator *(CalcStress x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
           ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        StressUnit unit = ((StressUnit)x.Quantity.Unit).GetKnownUnit();
        return new CalcLinearForce(new ForcePerLength(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantForcePerLengthUnit()), name, "");
    }

    public static CalcLinearForce operator *(CalcLength x, CalcStress y) => y * x;

    public static CalcForce operator *(CalcStress x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
           ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        StressUnit unit = ((StressUnit)x.Quantity.Unit).GetKnownUnit();
        return new CalcForce(new Force(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantAreaUnit()),
            unit.GetEquivilantForceUnit()), name, "");
    }

    public static CalcForce operator *(CalcArea x, CalcStress y) => y * x;

    public static CalcStress operator *(CalcStress x, double y)
    {
        return new CalcStress(x.Value * y, (StressUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcStress operator *(double x, CalcStress y) => y * x;
    #endregion

    #region DivisionOperators
    public static CalcDouble operator /(CalcStress x, CalcStress y)
    {
        (string name, string _, StressUnit _) = OperatorMetadataHelper<StressUnit>(x, y, '/');
        return new CalcDouble((Stress)x.Quantity / (Stress)y.Quantity, name, string.Empty);
    }

    public static CalcStress operator /(CalcStress x, double y)
    {
        return new CalcStress(x.Value / y, (StressUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcStress operator /(CalcStress x, int y)
    {
        return new CalcStress(x.Value / y, (StressUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcStress operator /(CalcStress x, CalcDouble y)
    {
        return new CalcStress(x.Value / y, (StressUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
    #endregion

    #region ComparisonOperators
    public static bool operator >(CalcStress left, CalcStress right) => GreaterThan(left, right);
    public static bool operator >=(CalcStress left, CalcStress right) => GreaterOrEqualThan(left, right);
    public static bool operator <(CalcStress left, CalcStress right) => LessThan(left, right);
    public static bool operator <=(CalcStress left, CalcStress right) => LessOrEqualThan(left, right);
    public static bool operator ==(CalcStress left, CalcStress right) => left.Equals(right);
    public static bool operator !=(CalcStress left, CalcStress right) => !left.Equals(right);
    #endregion

    public static bool TryParse(string str, IFormatProvider provider, out CalcStress result)
    {
        if (Stress.TryParse(str, provider, out Stress quantity))
        {
            result = new CalcStress(quantity, string.Empty);
            return true;
        }

        result = null;
        return false;
    }

    public static CalcStress Parse(string str, IFormatProvider provider)
    {
        return new CalcStress(Stress.Parse(str, provider), string.Empty);
    }

    public static CalcStress Zero => new CalcStress(Stress.Zero, string.Empty);
    public static CalcStress AdditiveIdentity => Zero;
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
}
