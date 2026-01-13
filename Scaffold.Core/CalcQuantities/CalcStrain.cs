using System.Numerics;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcStrain : CalcQuantity<Strain>,
    IParsable<CalcStrain>,
    IAdditionOperators<CalcStrain, CalcStrain, CalcStrain>,
    IAdditionOperators<CalcStrain, double, CalcStrain>,
    IAdditiveIdentity<CalcStrain, CalcStrain>,
    ISubtractionOperators<CalcStrain, CalcStrain, CalcStrain>,
    ISubtractionOperators<CalcStrain, double, CalcStrain>,
    IMultiplyOperators<CalcStrain, double, CalcStrain>,
    IDivisionOperators<CalcStrain, double, CalcStrain>,
    IDivisionOperators<CalcStrain, CalcStrain, CalcDouble>,
    IUnaryNegationOperators<CalcStrain, CalcStrain>,
    IComparisonOperators<CalcStrain, CalcStrain, bool>
{
    public CalcStrain(Strain strain, string name, string symbol = "")
        : base(strain, name, symbol) { }

    public CalcStrain(double value, StrainUnit unit, string name, string symbol)
        : base(new Strain(value, unit), name, symbol) { }

    public static implicit operator CalcStrain(Strain value) => new CalcStrain(value, string.Empty);

    #region AdditionOperators
    public static CalcStrain operator +(CalcStrain x, CalcStrain y)
    {
        (string name, string symbol, StrainUnit unit) = OperatorMetadataHelper<StrainUnit>(x, y, '+');
        return new CalcStrain(new Strain(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcStrain operator +(CalcStrain x, double y)
    {
        return new CalcStrain(x.Value + y, (StrainUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcStrain operator +(double x, CalcStrain y) => y + x;
    #endregion

    #region SubtractionOperators
    public static CalcStrain operator -(CalcStrain x)
    {
        return new CalcStrain(-(Strain)x.Quantity, $"-{x.DisplayName}", x.Symbol);
    }
    public static CalcStrain operator -(CalcStrain x, CalcStrain y)
    {
        (string name, string symbol, StrainUnit unit) = OperatorMetadataHelper<StrainUnit>(x, y, '-');
        return new CalcStrain(new Strain(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcStrain operator -(CalcStrain x, double y)
    {
        return new CalcStrain(x.Value - y, (StrainUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
    #endregion

    #region MultiplicationOperators
    public static CalcStrain operator *(CalcStrain x, double y)
    {
        return new CalcStrain(x.Value * y, (StrainUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }

    public static CalcStrain operator *(double x, CalcStrain y) => y * x;
    #endregion

    #region DivisionOperators
    public static CalcDouble operator /(CalcStrain x, CalcStrain y)
    {
        (string name, string _, StrainUnit unit) = OperatorMetadataHelper<StrainUnit>(x, y, '/');
        return new CalcDouble((Strain)x.Quantity / (Strain)y.Quantity, name, string.Empty);
    }

    public static CalcStrain operator /(CalcStrain x, double y)
    {
        return new CalcStrain(x.Value / y, (StrainUnit)x.Quantity.Unit, x.DisplayName, x.Symbol);
    }
    #endregion

    #region ComparisonOperators
    public static bool operator >(CalcStrain left, CalcStrain right) => GreaterThan(left, right);
    public static bool operator >=(CalcStrain left, CalcStrain right) => GreaterOrEqualThan(left, right);
    public static bool operator <(CalcStrain left, CalcStrain right) => LessThan(left, right);
    public static bool operator <=(CalcStrain left, CalcStrain right) => LessOrEqualThan(left, right);
    public static bool operator ==(CalcStrain left, CalcStrain right) => left.Equals(right);
    public static bool operator !=(CalcStrain left, CalcStrain right) => !left.Equals(right);
    #endregion

    public static bool TryParse(string str, IFormatProvider provider, out CalcStrain result)
    {
        if (Strain.TryParse(str, provider, out Strain quantity))
        {
            result = new CalcStrain(quantity, string.Empty);
            return true;
        }

        result = null;
        return false;
    }

    public static CalcStrain Parse(string str, IFormatProvider provider)
    {
        return new CalcStrain(Strain.Parse(str, provider), string.Empty);
    }

    public static CalcStrain Zero => new CalcStrain(Strain.Zero, string.Empty);
    public static CalcStrain AdditiveIdentity => Zero;
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
}
