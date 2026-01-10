#if NET7_0_OR_GREATER
using System.Numerics;
#endif
using Scaffold.Core.CalcValues;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcCurvature : CalcSIQuantity<ReciprocalLength>
#if NET7_0_OR_GREATER
    , IParsable<CalcCurvature>
    , IAdditionOperators<CalcCurvature, CalcCurvature, CalcCurvature>
    , IAdditionOperators<CalcCurvature, double, CalcCurvature>
    , IAdditiveIdentity<CalcCurvature, CalcCurvature>
    , ISubtractionOperators<CalcCurvature, CalcCurvature, CalcCurvature>
    , ISubtractionOperators<CalcCurvature, double, CalcCurvature>
    , IMultiplyOperators<CalcCurvature, CalcLength, CalcDouble>
    , IMultiplyOperators<CalcCurvature, double, CalcCurvature>
    , IDivisionOperators<CalcCurvature, double, CalcCurvature>
    , IDivisionOperators<CalcCurvature, CalcCurvature, CalcDouble>
    , IUnaryNegationOperators<CalcCurvature, CalcCurvature>
    , IComparisonOperators<CalcCurvature, CalcCurvature, bool>
#endif
{
    public CalcCurvature(ReciprocalLength length, string name, string symbol = "")
        : base(length, name, symbol) { }

    public CalcCurvature(double value, ReciprocalLengthUnit unit, string name, string symbol)
        : base(new ReciprocalLength(value, unit), name, symbol) { }

    public static implicit operator CalcCurvature(ReciprocalLength value) => new CalcCurvature(value, string.Empty);

    #region AdditionOperators
    public static CalcCurvature operator +(CalcCurvature x, CalcCurvature y)
    {
        (string name, string symbol, ReciprocalLengthUnit unit) = OperatorMetadataHelper<ReciprocalLengthUnit>(x, y, '+');
        return new CalcCurvature(new ReciprocalLength(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcCurvature operator +(CalcCurvature x, double y)
    {
        return new CalcCurvature(x.Value + y, (ReciprocalLengthUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }

    public static CalcCurvature operator +(double x, CalcCurvature y) => y + x;
    #endregion

    #region SubtractionOperators
    public static CalcCurvature operator -(CalcCurvature x)
    {
        return new CalcCurvature(-(ReciprocalLength)x.Quantity, $"-{x.TypeName}", x.Symbol);
    }

    public static CalcCurvature operator -(CalcCurvature x, CalcCurvature y)
    {
        (string name, string symbol, ReciprocalLengthUnit unit) = OperatorMetadataHelper<ReciprocalLengthUnit>(x, y, '-');
        return new CalcCurvature(new ReciprocalLength(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcCurvature operator -(CalcCurvature x, double y)
    {
        return new CalcCurvature(x.Value - y, (ReciprocalLengthUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }
    #endregion

    #region MultiplicationOperators
    public static CalcDouble operator *(CalcCurvature x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.TypeName) || string.IsNullOrEmpty(y.TypeName)
            ? string.Empty : $"{x.TypeName}\u2009·\u2009{y.TypeName}";
        LengthUnit unit = ((ReciprocalLengthUnit)x.Quantity.Unit).GetEquivilantLengthUnit();
        return new CalcDouble(x.Value * y.Quantity.As(unit), name, string.Empty, "-");
    }
    public static CalcCurvature operator *(CalcCurvature x, double y)
    {
        return new CalcCurvature(x.Value * y, (ReciprocalLengthUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }

    public static CalcCurvature operator *(double x, CalcCurvature y) => y * x;
    #endregion

    #region DivisionOperators
    public static CalcDouble operator /(CalcCurvature x, CalcCurvature y)
    {
        (string name, string _, ReciprocalLengthUnit _) = OperatorMetadataHelper<ReciprocalLengthUnit>(x, y, '/');
        return new CalcDouble((ReciprocalLength)x.Quantity / (ReciprocalLength)y.Quantity, name, string.Empty, "-");
    }

    public static CalcCurvature operator /(CalcCurvature x, double y)
    {
        return new CalcCurvature(x.Value / y, (ReciprocalLengthUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }
    #endregion

    #region ComparisonOperators
    public static bool operator >(CalcCurvature left, CalcCurvature right) => GreaterThan(left, right);
    public static bool operator >=(CalcCurvature left, CalcCurvature right) => GreaterOrEqualThan(left, right);
    public static bool operator <(CalcCurvature left, CalcCurvature right) => LessThan(left, right);
    public static bool operator <=(CalcCurvature left, CalcCurvature right) => LessOrEqualThan(left, right);
    public static bool operator ==(CalcCurvature left, CalcCurvature right) => left.Equals(right);
    public static bool operator !=(CalcCurvature left, CalcCurvature right) => !left.Equals(right);
    #endregion

    public static bool TryParse(string str, IFormatProvider provider, out CalcCurvature result)
    {
        if (ReciprocalLength.TryParse(str, provider, out ReciprocalLength quantity))
        {
            result = new CalcCurvature(quantity, string.Empty);
            return true;
        }

        result = null;
        return false;
    }

    public static CalcCurvature Parse(string str, IFormatProvider provider)
    {
        return new CalcCurvature(ReciprocalLength.Parse(str, provider), string.Empty);
    }

    public static CalcCurvature Zero => new CalcCurvature(ReciprocalLength.Zero, string.Empty);
    public static CalcCurvature AdditiveIdentity => Zero;
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
}
