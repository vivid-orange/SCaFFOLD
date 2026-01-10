#if NET7_0_OR_GREATER
using System.Numerics;
#endif
using Scaffold.Core.CalcValues;
using Scaffold.Core.Utility;

namespace Scaffold.Core.CalcQuantities;

public sealed class CalcVolume : CalcSIQuantity<Volume>
#if NET7_0_OR_GREATER
    , IParsable<CalcVolume>
    , IAdditionOperators<CalcVolume, CalcVolume, CalcVolume>
    , IAdditionOperators<CalcVolume, double, CalcVolume>
    , IAdditiveIdentity<CalcVolume, CalcVolume>
    , ISubtractionOperators<CalcVolume, CalcVolume, CalcVolume>
    , ISubtractionOperators<CalcVolume, double, CalcVolume>
    , IMultiplyOperators<CalcVolume, CalcLength, CalcInertia>
    , IMultiplyOperators<CalcVolume, double, CalcVolume>
    , IDivisionOperators<CalcVolume, CalcLength, CalcArea>
    , IDivisionOperators<CalcVolume, CalcArea, CalcLength>
    , IDivisionOperators<CalcVolume, double, CalcVolume>
    , IDivisionOperators<CalcVolume, CalcVolume, CalcDouble>
    , IUnaryNegationOperators<CalcVolume, CalcVolume>
    , IComparisonOperators<CalcVolume, CalcVolume, bool>
#endif
{
    public CalcVolume(Volume volume, string name, string symbol = "")
        : base(volume, name, symbol) { }

    public CalcVolume(double value, VolumeUnit unit, string name, string symbol)
        : base(new Volume(value, unit), name, symbol) { }

    public static implicit operator CalcVolume(Volume value) => new CalcVolume(value, string.Empty);

    #region AdditionOperators
    public static CalcVolume operator +(CalcVolume x, CalcVolume y)
    {
        (string name, string symbol, VolumeUnit unit) = OperatorMetadataHelper<VolumeUnit>(x, y, '+');
        return new CalcVolume(new Volume(x.Quantity.As(unit) + y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcVolume operator +(CalcVolume x, double y)
    {
        return new CalcVolume(x.Value + y, (VolumeUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }

    public static CalcVolume operator +(double x, CalcVolume y) => y + x;
    #endregion

    #region SubtractionOperators
    public static CalcVolume operator -(CalcVolume x)
    {
        return new CalcVolume(-(Volume)x.Quantity, $"-{x.TypeName}", x.Symbol);
    }
    public static CalcVolume operator -(CalcVolume x, CalcVolume y)
    {
        (string name, string symbol, VolumeUnit unit) = OperatorMetadataHelper<VolumeUnit>(x, y, '-');
        return new CalcVolume(new Volume(x.Quantity.As(unit) - y.Quantity.As(unit), unit), name, symbol);
    }

    public static CalcVolume operator -(CalcVolume x, double y)
    {
        return new CalcVolume(x.Value - y, (VolumeUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }
    #endregion

    #region MultiplicationOperators
    public static CalcInertia operator *(CalcVolume x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.TypeName) || string.IsNullOrEmpty(y.TypeName)
            ? string.Empty : $"{x.TypeName}\u2009·\u2009{y.TypeName}";
        VolumeUnit unit = (VolumeUnit)x.Quantity.Unit;
        return new CalcInertia(new AreaMomentOfInertia(x.Quantity.As(unit) * y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantInertiaUnit()), name, "");
    }

    public static CalcVolume operator *(CalcVolume x, double y)
    {
        return new CalcVolume(x.Value * y, (VolumeUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }

    public static CalcVolume operator *(double x, CalcVolume y) => y * x;
    #endregion

    #region DivisionOperators
    public static CalcDouble operator /(CalcVolume x, CalcVolume y)
    {
        (string name, string _, VolumeUnit unit) = OperatorMetadataHelper<VolumeUnit>(x, y, '/');
        return new CalcDouble((Volume)x.Quantity / (Volume)y.Quantity, name, string.Empty);
    }

    public static CalcLength operator /(CalcVolume x, CalcArea y)
    {
        string name = string.IsNullOrEmpty(x.TypeName) || string.IsNullOrEmpty(y.TypeName)
            ? string.Empty : $"{x.TypeName}\u2009/\u2009{y.TypeName}";
        VolumeUnit unit = (VolumeUnit)x.Quantity.Unit;
        return new CalcLength(new Length(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantAreaUnit()),
            unit.GetEquivilantLengthUnit()), name, "");
    }

    public static CalcArea operator /(CalcVolume x, CalcLength y)
    {
        string name = string.IsNullOrEmpty(x.TypeName) || string.IsNullOrEmpty(y.TypeName)
            ? string.Empty : $"{x.TypeName}\u2009/\u2009{y.TypeName}";
        VolumeUnit unit = (VolumeUnit)x.Quantity.Unit;
        return new CalcArea(new Area(x.Quantity.As(unit) / y.Quantity.As(unit.GetEquivilantLengthUnit()),
            unit.GetEquivilantAreaUnit()), name, "");
    }

    public static CalcVolume operator /(CalcVolume x, double y)
    {
        return new CalcVolume(x.Value / y, (VolumeUnit)x.Quantity.Unit, x.TypeName, x.Symbol);
    }
    #endregion

    #region ComparisonOperators
    public static bool operator >(CalcVolume left, CalcVolume right) => GreaterThan(left, right);
    public static bool operator >=(CalcVolume left, CalcVolume right) => GreaterOrEqualThan(left, right);
    public static bool operator <(CalcVolume left, CalcVolume right) => LessThan(left, right);
    public static bool operator <=(CalcVolume left, CalcVolume right) => LessOrEqualThan(left, right);
    public static bool operator ==(CalcVolume left, CalcVolume right) => left.Equals(right);
    public static bool operator !=(CalcVolume left, CalcVolume right) => !left.Equals(right);
    #endregion

    public static bool TryParse(string str, IFormatProvider provider, out CalcVolume result)
    {
        if (Volume.TryParse(str, provider, out Volume quantity))
        {
            result = new CalcVolume(quantity, string.Empty);
            return true;
        }

        result = null;
        return false;
    }

    public static CalcVolume Parse(string str, IFormatProvider provider)
    {
        return new CalcVolume(Volume.Parse(str, provider), string.Empty);
    }

    public static CalcVolume Zero => new CalcVolume(Volume.Zero, string.Empty);
    public static CalcVolume AdditiveIdentity => Zero;
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
}
