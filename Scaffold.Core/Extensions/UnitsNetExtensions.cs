using System.Globalization;
using System.Threading;

public static class UnitsNetExtensions
{
    private static readonly BaseUnits sI = UnitsNet.UnitSystem.SI.BaseUnits;

    public static AreaMomentOfInertiaUnit GetAreaMomentOfInertiaUnit(this LengthUnit unit)
    {
        switch (unit)
        {
            case LengthUnit.Millimeter:
                return AreaMomentOfInertiaUnit.MillimeterToTheFourth;

            case LengthUnit.Centimeter:
                return AreaMomentOfInertiaUnit.CentimeterToTheFourth;

            case LengthUnit.Meter:
                return AreaMomentOfInertiaUnit.MeterToTheFourth;

            case LengthUnit.Foot:
                return AreaMomentOfInertiaUnit.FootToTheFourth;

            case LengthUnit.Inch:
                return AreaMomentOfInertiaUnit.InchToTheFourth;
        }

        throw new UnitException("Unable to convert " + unit + " to a known type of AreaMomentOfInertia");
    }

    public static AreaUnit GetAreaUnit(this LengthUnit unit)
    {
        switch (unit)
        {
            case LengthUnit.Millimeter:
                return AreaUnit.SquareMillimeter;

            case LengthUnit.Centimeter:
                return AreaUnit.SquareCentimeter;

            case LengthUnit.Meter:
                return AreaUnit.SquareMeter;

            case LengthUnit.Foot:
                return AreaUnit.SquareFoot;

            case LengthUnit.Inch:
                return AreaUnit.SquareInch;
        }

        throw new UnitException("Unable to convert " + unit + " to a known type of Area");
    }

    public static CoefficientOfThermalExpansionUnit GetCoefficientOfThermalExpansionUnit(
      this TemperatureUnit temperatureUnit)
    {
        switch (temperatureUnit)
        {
            case TemperatureUnit.Kelvin:
                return CoefficientOfThermalExpansionUnit.PerKelvin;

            case TemperatureUnit.DegreeFahrenheit:
                return CoefficientOfThermalExpansionUnit.PerDegreeFahrenheit;

            case TemperatureUnit.DegreeCelsius:
            default:
                return CoefficientOfThermalExpansionUnit.PerDegreeCelsius;
        }
    }

    public static DensityUnit GetDensityUnit(this MassUnit massUnit, LengthUnit lengthUnit)
    {
        string mass = massUnit.ToString();
        string length = lengthUnit.ToString();
        return (DensityUnit)Enum.Parse(typeof(DensityUnit), mass + "PerCubic" + length);
    }

    public static PressureUnit GetForcePerAreaUnit(this ForceUnit forceUnit, LengthUnit lengthUnit)
    {
        switch (forceUnit)
        {
            case ForceUnit.Newton:
                switch (lengthUnit)
                {
                    case LengthUnit.Millimeter:
                        return PressureUnit.NewtonPerSquareMillimeter;

                    case LengthUnit.Centimeter:
                        return PressureUnit.NewtonPerSquareCentimeter;

                    case LengthUnit.Meter:
                        return PressureUnit.NewtonPerSquareMeter;
                }

                break;

            case ForceUnit.Kilonewton:
                switch (lengthUnit)
                {
                    case LengthUnit.Millimeter:
                        return PressureUnit.KilonewtonPerSquareMillimeter;

                    case LengthUnit.Centimeter:
                        return PressureUnit.KilonewtonPerSquareCentimeter;

                    case LengthUnit.Meter:
                        return PressureUnit.KilonewtonPerSquareMeter;
                }

                break;

            case ForceUnit.Meganewton:
                switch (lengthUnit)
                {
                    case LengthUnit.Meter:
                        return PressureUnit.MeganewtonPerSquareMeter;
                }

                break;

            case ForceUnit.KilopoundForce:
                switch (lengthUnit)
                {
                    case LengthUnit.Inch:
                        return PressureUnit.KilopoundForcePerSquareInch;

                    case LengthUnit.Foot:
                        return PressureUnit.KilopoundForcePerSquareFoot;
                }

                break;

            case ForceUnit.PoundForce:
                switch (lengthUnit)
                {
                    case LengthUnit.Inch:
                        return PressureUnit.PoundForcePerSquareInch;

                    case LengthUnit.Foot:
                        return PressureUnit.PoundForcePerSquareFoot;
                }

                break;
        }

        throw new UnitsNetException("Unable to convert " + forceUnit.ToString() + " combined with " +
                                      lengthUnit.ToString() + " to force per area");
    }

    public static ForcePerLengthUnit GetForcePerLengthUnit(this ForceUnit forceUnit, LengthUnit lengthUnit)
    {
        switch (forceUnit)
        {
            case ForceUnit.Newton:
                switch (lengthUnit)
                {
                    case LengthUnit.Millimeter:
                        return ForcePerLengthUnit.NewtonPerMillimeter;

                    case LengthUnit.Centimeter:
                        return ForcePerLengthUnit.NewtonPerCentimeter;

                    case LengthUnit.Meter:
                        return ForcePerLengthUnit.NewtonPerMeter;
                }

                break;

            case ForceUnit.Kilonewton:
                switch (lengthUnit)
                {
                    case LengthUnit.Millimeter:
                        return ForcePerLengthUnit.KilonewtonPerMillimeter;

                    case LengthUnit.Centimeter:
                        return ForcePerLengthUnit.KilonewtonPerCentimeter;

                    case LengthUnit.Meter:
                        return ForcePerLengthUnit.KilonewtonPerMeter;
                }

                break;

            case ForceUnit.Meganewton:
                switch (lengthUnit)
                {
                    case LengthUnit.Millimeter:
                        return ForcePerLengthUnit.MeganewtonPerMillimeter;

                    case LengthUnit.Centimeter:
                        return ForcePerLengthUnit.MeganewtonPerCentimeter;

                    case LengthUnit.Meter:
                        return ForcePerLengthUnit.MeganewtonPerMeter;
                }

                break;

            case ForceUnit.KilopoundForce:
                switch (lengthUnit)
                {
                    case LengthUnit.Inch:
                        return ForcePerLengthUnit.KilopoundForcePerInch;

                    case LengthUnit.Foot:
                        return ForcePerLengthUnit.KilopoundForcePerFoot;
                }

                break;

            case ForceUnit.PoundForce:
                switch (lengthUnit)
                {
                    case LengthUnit.Inch:
                        return ForcePerLengthUnit.PoundForcePerInch;

                    case LengthUnit.Foot:
                        return ForcePerLengthUnit.PoundForcePerFoot;
                }

                break;
        }

        throw new UnitsNetException("Unable to convert " + forceUnit + " x " + lengthUnit +
                                      " to a known type of VolumePerLengthUnit");
    }

    public static LinearDensityUnit GetLinearDensityUnit(this MassUnit massUnit, LengthUnit lengthUnit)
    {
        switch (massUnit)
        {
            case MassUnit.Kilogram:
                switch (lengthUnit)
                {
                    case LengthUnit.Millimeter:
                        return LinearDensityUnit.KilogramPerMillimeter;

                    case LengthUnit.Centimeter:
                        return LinearDensityUnit.KilogramPerCentimeter;

                    case LengthUnit.Meter:
                        return LinearDensityUnit.KilogramPerMeter;
                }

                break;

            case MassUnit.Pound:
                switch (lengthUnit)
                {
                    case LengthUnit.Foot:
                        return LinearDensityUnit.PoundPerFoot;

                    case LengthUnit.Inch:
                        return LinearDensityUnit.PoundPerInch;
                }

                break;
        }

        throw new UnitsNetException("Unable to convert " + massUnit.ToString() + " combined with " +
                                      lengthUnit.ToString() + " to Linear Density");
    }

    public static TorqueUnit GetMomentUnit(this ForceUnit forceUnit, LengthUnit lengthUnit)
    {
        switch (forceUnit)
        {
            case ForceUnit.Newton:
                switch (lengthUnit)
                {
                    case LengthUnit.Millimeter:
                        return TorqueUnit.NewtonMillimeter;

                    case LengthUnit.Centimeter:
                        return TorqueUnit.NewtonCentimeter;

                    case LengthUnit.Meter:
                        return TorqueUnit.NewtonMeter;
                }

                break;

            case ForceUnit.Kilonewton:
                switch (lengthUnit)
                {
                    case LengthUnit.Millimeter:
                        return TorqueUnit.KilonewtonMillimeter;

                    case LengthUnit.Centimeter:
                        return TorqueUnit.KilonewtonCentimeter;

                    case LengthUnit.Meter:
                        return TorqueUnit.KilonewtonMeter;
                }

                break;

            case ForceUnit.Meganewton:
                switch (lengthUnit)
                {
                    case LengthUnit.Millimeter:
                        return TorqueUnit.MeganewtonMillimeter;

                    case LengthUnit.Centimeter:
                        return TorqueUnit.MeganewtonCentimeter;

                    case LengthUnit.Meter:
                        return TorqueUnit.MeganewtonMeter;
                }

                break;

            case ForceUnit.KilopoundForce:
                switch (lengthUnit)
                {
                    case LengthUnit.Inch:
                        return TorqueUnit.KilopoundForceInch;

                    case LengthUnit.Foot:
                        return TorqueUnit.KilopoundForceFoot;
                }

                break;

            case ForceUnit.PoundForce:
                switch (lengthUnit)
                {
                    case LengthUnit.Inch:
                        return TorqueUnit.PoundForceInch;

                    case LengthUnit.Foot:
                        return TorqueUnit.PoundForceFoot;
                }

                break;
        }

        throw new UnitsNetException("Unable to convert " + forceUnit.ToString() + " combined with " +
                                      lengthUnit.ToString() + " to moment");
    }

    public static VolumeUnit GetVolumeUnit(this LengthUnit unit)
    {
        switch (unit)
        {
            case LengthUnit.Millimeter:
                return VolumeUnit.CubicMillimeter;

            case LengthUnit.Centimeter:
                return VolumeUnit.CubicCentimeter;

            case LengthUnit.Meter:
                return VolumeUnit.CubicMeter;

            case LengthUnit.Foot:
                return VolumeUnit.CubicFoot;

            case LengthUnit.Inch:
                return VolumeUnit.CubicInch;
        }

        throw new UnitException("Unable to convert " + unit + " to a known type of Volume");
    }

    public static VolumePerLengthUnit GetVolumePerLengthUnit(this LengthUnit unit)
    {
        switch (unit)
        {
            case LengthUnit.Foot:
            case LengthUnit.Inch:
                return VolumePerLengthUnit.CubicYardPerFoot;

            case LengthUnit.Millimeter:
            case LengthUnit.Centimeter:
            case LengthUnit.Meter:
                return VolumePerLengthUnit.CubicMeterPerMeter;

            default:
                throw new UnitsNetException("Unable to convert " + unit + " to a known type of VolumePerLengthUnit");
        }
    }


    /// <summary>
    /// Tries to parse a units abbreviation or string representation.
    /// </summary>
    /// <param name="unitType"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Enum Parse(Type unitType, string value)
    {
        CultureInfo culture = Thread.CurrentThread.CurrentUICulture;
        return Parse(unitType, value, culture);
    }

    /// <summary>
    /// Tries to parse a unit´s abbreviation or string representation.
    /// </summary>
    /// <param name="unitType"></param>
    /// <param name="value"></param>
    /// <param name="currentUiCulture"></param>
    /// <returns></returns>
    public static Enum Parse(Type unitType, string value, CultureInfo currentUiCulture)
    {
        if (UnitsNetSetup.Default.UnitParser.TryParse(value, unitType, out Enum unit))
        {
            return unit;
        }

        try
        {
            return (Enum)Enum.Parse(unitType, value, true);
        }
        catch (ArgumentException)
        {
            // try to use current culture to parse unit abbreviation
            switch (unitType)
            {
                case Type _ when unitType == typeof(AccelerationUnit):
                    return Acceleration.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(AngleUnit):
                    return Angle.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(AreaMomentOfInertiaUnit):
                    return AreaMomentOfInertia.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(AreaUnit):
                    return Area.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(CoefficientOfThermalExpansionUnit):
                    return CoefficientOfThermalExpansion.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(ReciprocalLengthUnit):
                    return ReciprocalLength.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(DensityUnit):
                    return Density.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(DurationUnit):
                    return Duration.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(EnergyUnit):
                    return Energy.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(ForcePerLengthUnit):
                    return ForcePerLength.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(ForceUnit):
                    return Force.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(MassUnit):
                    return Mass.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(TorqueUnit):
                    return Torque.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(LengthUnit):
                    return Length.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(LinearDensityUnit):
                    return LinearDensity.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(PressureUnit):
                    return Pressure.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(RatioUnit):
                    return Ratio.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(SpeedUnit):
                    return Speed.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(RatioUnit):
                    return Ratio.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(TemperatureUnit):
                    return Temperature.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(VolumePerLengthUnit):
                    return VolumePerLength.ParseUnit(value, currentUiCulture);

                case Type _ when unitType == typeof(VolumeUnit):
                    return Volume.ParseUnit(value, currentUiCulture);

                default:
                    throw new ArgumentException();
            }
        }
    }
}
