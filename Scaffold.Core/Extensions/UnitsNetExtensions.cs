public static class UnitsNetExtensions
{
    private static readonly BaseUnits sI = UnitsNet.UnitSystem.SI.BaseUnits;

    public static AreaMomentOfInertiaUnit GetAreaMomentOfInertiaUnit(this LengthUnit unit)
    {
        var baseUnits = new BaseUnits(unit, sI.Mass, sI.Time, sI.Current, sI.Temperature, sI.Amount,
          sI.LuminousIntensity);
        return new AreaMomentOfInertia(1, new UnitsNet.UnitSystem(baseUnits)).Unit;
    }

    public static AreaUnit GetAreaUnit(this LengthUnit unit)
    {
        var baseUnits = new BaseUnits(unit, sI.Mass, sI.Time, sI.Current, sI.Temperature, sI.Amount,
          sI.LuminousIntensity);
        return new Area(1, new UnitsNet.UnitSystem(baseUnits)).Unit;
    }

    public static CoefficientOfThermalExpansionUnit GetCoefficientOfThermalExpansionUnit(
      this TemperatureUnit unit)
    {
        switch (unit)
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
        try
        {
            return (DensityUnit)Enum.Parse(typeof(DensityUnit), $"{massUnit}PerCubic{lengthUnit}");
        }
        catch (Exception)
        {
            throw new ArgumentException(
                $"Unable to convert {massUnit} combined with {lengthUnit} to Density");
        }
    }

    public static PressureUnit GetForcePerAreaUnit(this ForceUnit forceUnit, LengthUnit lengthUnit)
    {
        try
        {
            return (PressureUnit)Enum.Parse(typeof(PressureUnit), $"{forceUnit}PerSquare{lengthUnit}");
        }
        catch (Exception)
        {
            throw new ArgumentException(
                $"Unable to convert {forceUnit} combined with {lengthUnit} to Pressure");
        }
    }

    public static ForcePerLengthUnit GetForcePerLengthUnit(this ForceUnit forceUnit, LengthUnit lengthUnit)
    {
        try
        {
            return (ForcePerLengthUnit)Enum.Parse(typeof(ForcePerLengthUnit), $"{forceUnit}Per{lengthUnit}");
        }
        catch (Exception)
        {
            throw new ArgumentException(
                $"Unable to convert {forceUnit} combined with {lengthUnit} to Force per Length");
        }
    }

    public static LinearDensityUnit GetLinearDensityUnit(this MassUnit massUnit, LengthUnit lengthUnit)
    {
        try
        {
            return (LinearDensityUnit)Enum.Parse(typeof(LinearDensityUnit), $"{massUnit}Per{lengthUnit}");
        }
        catch (Exception)
        {
            throw new ArgumentException(
                $"Unable to convert {massUnit} combined with {lengthUnit} to Linear Density");
        }
    }

    public static TorqueUnit GetMomentUnit(this ForceUnit forceUnit, LengthUnit lengthUnit)
    {
        try
        {
            return (TorqueUnit)Enum.Parse(typeof(TorqueUnit), $"{forceUnit}{lengthUnit}");
        }
        catch (Exception)
        {
            throw new ArgumentException(
                $"Unable to convert {forceUnit} combined with {lengthUnit} to Moment");
        }
    }

    public static VolumeUnit GetVolumeUnit(this LengthUnit lengthUnit)
    {
        try
        {
            return (VolumeUnit)Enum.Parse(typeof(VolumeUnit), $"Cubic{lengthUnit}");
        }
        catch (Exception)
        {
            var baseUnits = new BaseUnits(lengthUnit, sI.Mass, sI.Time, sI.Current, sI.Temperature,
                sI.Amount, sI.LuminousIntensity);
            try
            {
                return new Volume(1, new UnitsNet.UnitSystem(baseUnits)).Unit;
            }
            catch (Exception)
            {
                throw new ArgumentException(
                $"Unable to convert {lengthUnit} to Moment");
            }
        }
    }
}
