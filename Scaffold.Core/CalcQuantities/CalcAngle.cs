#if NET7_0_OR_GREATER
using System.Numerics;
#endif
using Scaffold.Core.CalcValues;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcAngle : CalcSIQuantity<Angle>
#if NET7_0_OR_GREATER
    , IParsable<CalcAngle>
    , IAdditionOperators<CalcAngle, CalcAngle, CalcAngle>
    , IAdditionOperators<CalcAngle, double, CalcAngle>
    , IAdditiveIdentity<CalcAngle, CalcAngle>
    , ISubtractionOperators<CalcAngle, CalcAngle, CalcAngle>
    , ISubtractionOperators<CalcAngle, double, CalcAngle>
    , IMultiplyOperators<CalcAngle, double, CalcAngle>
    , IDivisionOperators<CalcAngle, double, CalcAngle>
    , IDivisionOperators<CalcAngle, CalcAngle, CalcDouble>
    , IUnaryNegationOperators<CalcAngle, CalcAngle>
    , IComparisonOperators<CalcAngle, CalcAngle, bool>
#endif
{
    public CalcAngle(Angle length, string name, string symbol = "")
        : base(length, name, symbol) { }

    public CalcAngle(double value, AngleUnit unit, string name, string symbol)
        : base(new Angle(value, unit), name, symbol) { }

    public static implicit operator CalcAngle(Angle value) => new CalcAngle(value, string.Empty);

    #region AdditionOperators
    public static CalcAngle operator +(CalcAngle x, CalcAngle y)
    {
        (string name, string symbol, AngleUnit unit) = OperatorMetadataHelper<AngleUnit>(x, y, '+');
        return new CalcAngle(new Angle(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcAngle operator +(CalcAngle x, double y)
    {
        return new CalcAngle(x.Value + y, (AngleUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }

    public static CalcAngle operator +(double x, CalcAngle y) => y + x;
    #endregion

    #region SubtractionOperators
    public static CalcAngle operator -(CalcAngle x)
    {
        return new CalcAngle(-(Angle)x.Quantity, $"-{x.TypeName}", x.Symbol);
    }

    public static CalcAngle operator -(CalcAngle x, CalcAngle y)
    {
        (string name, string symbol, AngleUnit unit) = OperatorMetadataHelper<AngleUnit>(x, y, '-');
        return new CalcAngle(new Angle(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcAngle operator -(CalcAngle x, double y)
    {
        return new CalcAngle(x.Value - y, (AngleUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }
    #endregion

    #region MultiplicationOperators
    public static CalcAngle operator *(CalcAngle x, double y)
    {
        return new CalcAngle(x.Value * y, (AngleUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }

    public static CalcAngle operator *(double x, CalcAngle y) => y * x;
    #endregion

    #region DivisionOperators
    public static CalcDouble operator /(CalcAngle x, CalcAngle y)
    {
        (string name, string _, AngleUnit _) = OperatorMetadataHelper<AngleUnit>(x, y, '/');
        return new CalcDouble((Angle)x.Quantity / (Angle)y.Quantity, name, string.Empty, "-");
    }

    public static CalcAngle operator /(CalcAngle x, double y)
    {
        return new CalcAngle(x.Value / y, (AngleUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }
    #endregion

    #region ComparisonOperators
    public static bool operator >(CalcAngle left, CalcAngle right) => GreaterThan(left, right);
    public static bool operator >=(CalcAngle left, CalcAngle right) => GreaterOrEqualThan(left, right);
    public static bool operator <(CalcAngle left, CalcAngle right) => LessThan(left, right);
    public static bool operator <=(CalcAngle left, CalcAngle right) => LessOrEqualThan(left, right);
    public static bool operator ==(CalcAngle left, CalcAngle right) => left.Equals(right);
    public static bool operator !=(CalcAngle left, CalcAngle right) => !left.Equals(right);
    #endregion

    public static bool TryParse(string str, IFormatProvider provider, out CalcAngle result)
    {
        if (Angle.TryParse(str, provider, out Angle quantity))
        {
            result = new CalcAngle(quantity, string.Empty);
            return true;
        }

        result = null;
        return false;
    }

    public static CalcAngle Parse(string str, IFormatProvider provider)
    {
        return new CalcAngle(Angle.Parse(str, provider), string.Empty);
    }

    public static CalcAngle Zero => new CalcAngle(Angle.Zero, string.Empty);
    public static CalcAngle AdditiveIdentity => Zero;
    public static CalcAngle FromDegrees(double degrees, string name, string symbol = "")
    {
        return new CalcAngle(Angle.FromDegrees(degrees), name, symbol);
    }

    public static CalcAngle FromRadians(double radians, string name, string symbol = "")
    {
        return new CalcAngle(Angle.FromRadians(radians), name, symbol);
    }

    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();

}
