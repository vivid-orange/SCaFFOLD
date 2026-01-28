public static class DefaultUnits
{
    public static AccelerationUnit AccelerationUnit { get; set; }
        = AccelerationUnit.MeterPerSecondSquared;
    public static AngleUnit AngleUnit { get; set; } = AngleUnit.Degree;
    public static ReciprocalLengthUnit CurvatureUnit { get; set; }
        = ReciprocalLengthUnit.InverseMeter;
    public static DensityUnit DensityUnit { get; set; }
        = DensityUnit.KilogramPerCubicMeter;
    public static EnergyUnit EnergyUnit { get; set; } = EnergyUnit.Megajoule;
    public static PressureUnit ForcePerAreaUnit { get; set; }
        = PressureUnit.KilonewtonPerSquareMeter;
    public static ForcePerLengthUnit ForcePerLengthUnit { get; set; }
        = ForcePerLengthUnit.KilonewtonPerMeter;
    public static ForceUnit ForceUnit { get; set; } = ForceUnit.Kilonewton;
    public static LengthUnit LengthUnitGeometry { get; set; } = LengthUnit.Meter;
    public static LengthUnit LengthUnitSection { get; set; } = LengthUnit.Millimeter;
    public static LengthUnit DisplacementLengthUnit { get; set; } = LengthUnit.Millimeter;
    public static LinearDensityUnit LinearDensityUnit { get; set; }
        = LinearDensityUnit.KilogramPerMeter;
    public static MassUnit MassUnit { get; set; } = MassUnit.Tonne;
    public static RatioUnit MaterialStrainUnit { get; set; } = RatioUnit.DecimalFraction;
    public static PressureUnit MaterialStrengthUnit { get; set; } = PressureUnit.Megapascal;
    public static TorqueUnit MomentUnit { get; set; } = TorqueUnit.KilonewtonMeter;
    public static RatioUnit RatioUnit { get; set; } = RatioUnit.DecimalFraction;
    public static RotationalStiffnessUnit RotationalStiffnessUnit { get; set; }
        = RotationalStiffnessUnit.NewtonMeterPerRadian;
    public static AreaMomentOfInertiaUnit SectionAreaMomentOfInertiaUnit { get; set; }
        = AreaMomentOfInertiaUnit.CentimeterToTheFourth;
    public static AreaUnit SectionAreaUnit { get; set; } = AreaUnit.SquareMillimeter;
    public static VolumeUnit SectionModulusUnit { get; set; } = VolumeUnit.CubicMeter;
    public static VolumeUnit SectionVolumeUnit { get; set; } = VolumeUnit.CubicMeter;
    public static RatioUnit StrainUnit { get; set; } = RatioUnit.DecimalFraction;
    public static PressureUnit StressUnit { get; set; } = PressureUnit.Megapascal;
    public static TemperatureUnit TemperatureUnit { get; set; } = TemperatureUnit.DegreeCelsius;
    public static DurationUnit TimeLongUnit { get; set; } = DurationUnit.Day;
    public static DurationUnit TimeMediumUnit { get; set; } = DurationUnit.Minute;
    public static DurationUnit TimeShortUnit { get; set; } = DurationUnit.Second;
    public static SpeedUnit VelocityUnit { get; set; } = SpeedUnit.MeterPerSecond;
    public static VolumePerLengthUnit VolumePerLengthUnit { get; set; }
        = VolumePerLengthUnit.CubicMeterPerMeter;
    public static PressureUnit YoungsModulusUnit { get; set; } = PressureUnit.Gigapascal;
}
