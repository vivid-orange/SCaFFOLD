using System.Numerics;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcLinearMoment : CalcQuantity<MomentPerLength>,
    IParsable<CalcLinearMoment>,
    IAdditionOperators<CalcLinearMoment, CalcLinearMoment, CalcLinearMoment>,
    IAdditionOperators<CalcLinearMoment, double, CalcLinearMoment>,
    IAdditiveIdentity<CalcLinearMoment, CalcLinearMoment>,
    ISubtractionOperators<CalcLinearMoment, CalcLinearMoment, CalcLinearMoment>,
    ISubtractionOperators<CalcLinearMoment, double, CalcLinearMoment>,
    IMultiplyOperators<CalcLinearMoment, CalcLength, CalcMoment>,
    IMultiplyOperators<CalcLinearMoment, double, CalcLinearMoment>,
    IDivisionOperators<CalcLinearMoment, double, CalcLinearMoment>,
    IDivisionOperators<CalcLinearMoment, CalcLinearMoment, CalcDouble>,
    IUnaryNegationOperators<CalcLinearMoment, CalcLinearMoment>,
    IComparisonOperators<CalcLinearMoment, CalcLinearMoment, bool>
{
    public CalcLinearMoment(MomentPerLength linearMoment, string name, string symbol = "")
        : base(linearMoment, name, symbol) { }

    public CalcLinearMoment(double value, MomentPerLengthUnit unit, string name, string symbol)
        : base(new MomentPerLength(value, unit), name, symbol) { }

    public CalcLinearMoment(double value, string name, string symbol)
        : base(new MomentPerLength(value, MomentPerLengthUnit.KilonewtonMeterPerMeter), name, symbol) { }

    public static implicit operator CalcLinearMoment(MomentPerLength value) => new CalcLinearMoment(value, string.Empty);

    #region AdditionOperators
    public static CalcLinearMoment operator +(CalcLinearMoment x, CalcLinearMoment y)
    {
        (string name, string symbol, MomentPerLengthUnit unit) = OperatorMetadataHelper<MomentPerLengthUnit>(x, y, '+');
        return new CalcLinearMoment(new MomentPerLength(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }
    public static CalcLinearMoment operator +(CalcLinearMoment x, double y)
    {
        return new CalcLinearMoment(x.Value + y, (MomentPerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLinearMoment operator +(double x, CalcLinearMoment y) => y + x;
    #endregion

    #region SubtractionOperators
    public static CalcLinearMoment operator -(CalcLinearMoment x)
    {
        return new CalcLinearMoment(-(MomentPerLength)x.Quantity, $"-{x.DisplayName}", x.Symbol);
    }

    public static CalcLinearMoment operator -(CalcLinearMoment x, CalcLinearMoment y)
    {
        (string name, string symbol, MomentPerLengthUnit unit) = OperatorMetadataHelper<MomentPerLengthUnit>(x, y, '-');
        return new CalcLinearMoment(new MomentPerLength(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcLinearMoment operator -(CalcLinearMoment x, double y)
    {
        return new CalcLinearMoment(x.Value - y, (MomentPerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
    #endregion

    #region MultiplicationOperators
    public static CalcMoment operator *(CalcLinearMoment x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.DisplayName) || string.IsNullOrEmpty(y.DisplayName)
           ? string.Empty : $"{x.DisplayName}\u2009·\u2009{y.DisplayName}";
        MomentPerLengthUnit unit = (MomentPerLengthUnit)x.Quantity.Unit;
        return new CalcMoment(new Moment(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantMomentUnit()), name, "");
    }

    public static CalcMoment operator *(CalcLength x, CalcLinearMoment y) => y * x;

    public static CalcLinearMoment operator *(CalcLinearMoment x, double y)
    {
        return new CalcLinearMoment(x.Value * y, (MomentPerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcLinearMoment operator *(double x, CalcLinearMoment y) => y * x;
    #endregion

    #region DivisionOperators
    public static CalcDouble operator /(CalcLinearMoment x, CalcLinearMoment y)
    {
        (string name, string _, MomentPerLengthUnit _) = OperatorMetadataHelper<MomentPerLengthUnit>(x, y, '/');
        return new CalcDouble((MomentPerLength)x.Quantity / (MomentPerLength)y.Quantity, name, string.Empty);
    }

    public static CalcLinearMoment operator /(CalcLinearMoment x, double y)
    {
        return new CalcLinearMoment(x.Value / y, (MomentPerLengthUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
    #endregion

    #region ComparisonOperators
    public static bool operator >(CalcLinearMoment left, CalcLinearMoment right) => GreaterThan(left, right);
    public static bool operator >=(CalcLinearMoment left, CalcLinearMoment right) => GreaterOrEqualThan(left, right);
    public static bool operator <(CalcLinearMoment left, CalcLinearMoment right) => LessThan(left, right);
    public static bool operator <=(CalcLinearMoment left, CalcLinearMoment right) => LessOrEqualThan(left, right);
    public static bool operator ==(CalcLinearMoment left, CalcLinearMoment right) => left.Equals(right);
    public static bool operator !=(CalcLinearMoment left, CalcLinearMoment right) => !left.Equals(right);
    #endregion

    public static bool TryParse(string str, IFormatProvider provider, out CalcLinearMoment result)
    {
        if (MomentPerLength.TryParse(str, provider, out MomentPerLength quantity))
        {
            result = new CalcLinearMoment(quantity, string.Empty);
            return true;
        }

        result = null;
        return false;
    }

    public static CalcLinearMoment Parse(string str, IFormatProvider provider)
    {
        return new CalcLinearMoment(MomentPerLength.Parse(str, provider), string.Empty);
    }

    public static CalcLinearMoment Zero => new CalcLinearMoment(MomentPerLength.Zero, string.Empty);
    public static CalcLinearMoment AdditiveIdentity => Zero;
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
}
