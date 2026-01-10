#if NET7_0_OR_GREATER
using System.Numerics;
#endif
using Scaffold.Core.CalcValues;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcMoment : CalcSIQuantity<Moment>
#if NET7_0_OR_GREATER
    , IParsable<CalcMoment>
    , IAdditionOperators<CalcMoment, CalcMoment, CalcMoment>
    , IAdditionOperators<CalcMoment, double, CalcMoment>
    , IAdditiveIdentity<CalcMoment, CalcMoment>
    , ISubtractionOperators<CalcMoment, CalcMoment, CalcMoment>
    , ISubtractionOperators<CalcMoment, double, CalcMoment>
    , IMultiplyOperators<CalcMoment, double, CalcMoment>
    , IDivisionOperators<CalcMoment, CalcLength, CalcForce>
    , IDivisionOperators<CalcMoment, CalcForce, CalcLength>
    , IDivisionOperators<CalcMoment, double, CalcMoment>
    , IDivisionOperators<CalcMoment, CalcMoment, CalcDouble>
    , IUnaryNegationOperators<CalcMoment, CalcMoment>
    , IComparisonOperators<CalcMoment, CalcMoment, bool>
#endif
{
    public CalcMoment(Moment moment, string name, string symbol = "")
        : base(moment, name, symbol) { }

    public CalcMoment(double value, MomentUnit unit, string name, string symbol)
        : base(new Moment(value, unit), name, symbol) { }

    public CalcMoment(double value, string name, string symbol)
        : base(new Moment(value, MomentUnit.KilonewtonMeter), name, symbol) { }

    public static implicit operator CalcMoment(Moment value) => new CalcMoment(value, string.Empty);

    #region AdditionOperators
    public static CalcMoment operator +(CalcMoment x, CalcMoment y)
    {
        (string name, string symbol, MomentUnit unit) = OperatorMetadataHelper<MomentUnit>(x, y, '+');
        return new CalcMoment(new Moment(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcMoment operator +(CalcMoment x, double y)
    {
        return new CalcMoment(x.Value + y, (MomentUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }

    public static CalcMoment operator +(double x, CalcMoment y) => y + x;
    #endregion

    #region SubtractionOperators
    public static CalcMoment operator -(CalcMoment x)
    {
        return new CalcMoment(-(Moment)x.Quantity, $"-{x.TypeName}", x.Symbol);
    }

    public static CalcMoment operator -(CalcMoment x, CalcMoment y)
    {
        (string name, string symbol, MomentUnit unit) = OperatorMetadataHelper<MomentUnit>(x, y, '-');
        return new CalcMoment(new Moment(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcMoment operator -(CalcMoment x, double y)
    {
        return new CalcMoment(x.Value - y, (MomentUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }
    #endregion

    #region MultiplicationOperators
    public static CalcMoment operator *(CalcMoment x, double y)
    {
        return new CalcMoment(x.Value * y, (MomentUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }

    public static CalcMoment operator *(double x, CalcMoment y) => y * x;
    #endregion

    #region DivisionOperators
    public static CalcForce operator /(CalcMoment x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.TypeName) || string.IsNullOrEmpty(y.TypeName)
           ? string.Empty : $"{x.TypeName}\u2009/\u2009{y.TypeName}";
        MomentUnit unit = (MomentUnit)x.Quantity.Unit;
        return new CalcForce(new Force(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantForceUnit()), name);
    }

    public static CalcLength operator /(CalcMoment x, CalcForce y)
    {
        string name = string.IsNullOrEmpty(x.TypeName) || string.IsNullOrEmpty(y.TypeName)
           ? string.Empty : $"{x.TypeName}\u2009/\u2009{y.TypeName}";
        MomentUnit unit = (MomentUnit)x.Quantity.Unit;
        return new CalcLength(new Length(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantForceUnit()),
            unit.GetEquivilantLengthUnit()), name);
    }

    public static CalcDouble operator /(CalcMoment x, CalcMoment y)
    {
        (string name, string _, MomentUnit _) = OperatorMetadataHelper<MomentUnit>(x, y, '/');
        return new CalcDouble((Moment)x.Quantity / (Moment)y.Quantity, name, string.Empty);
    }

    public static CalcMoment operator /(CalcMoment x, double y)
    {
        return new CalcMoment(x.Value / y, (MomentUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }
    #endregion

    #region ComparisonOperators
    public static bool operator >(CalcMoment left, CalcMoment right) => GreaterThan(left, right);
    public static bool operator >=(CalcMoment left, CalcMoment right) => GreaterOrEqualThan(left, right);
    public static bool operator <(CalcMoment left, CalcMoment right) => LessThan(left, right);
    public static bool operator <=(CalcMoment left, CalcMoment right) => LessOrEqualThan(left, right);
    public static bool operator ==(CalcMoment left, CalcMoment right) => left.Equals(right);
    public static bool operator !=(CalcMoment left, CalcMoment right) => !left.Equals(right);
    #endregion

    public static bool TryParse(string str, IFormatProvider provider, out CalcMoment result)
    {
        if (Moment.TryParse(str, provider, out Moment quantity))
        {
            result = new CalcMoment(quantity, string.Empty);
            return true;
        }

        result = null;
        return false;
    }

    public static CalcMoment Parse(string str, IFormatProvider provider)
    {
        return new CalcMoment(Moment.Parse(str, provider), string.Empty);
    }

    public static CalcMoment Zero => new CalcMoment(Moment.Zero, string.Empty);
    public static CalcMoment AdditiveIdentity => Zero;
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();

    public CalcLinearMoment MakeLinear(CalcLength length)
    {
        MomentUnit unit = (MomentUnit)Quantity.Unit;
        return new CalcLinearMoment(new MomentPerLength(Quantity.As(unit) / length.Value,
            unit.GetEquivilantMomentPerLengthUnit((LengthUnit)length.Quantity.Unit)), TypeName);
    }

    public CalcLinearMoment MakeLinear() => MakeLinear(CalcLength.OneMeter);
}
