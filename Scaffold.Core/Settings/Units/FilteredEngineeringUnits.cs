public class FilteredEngineeringUnits
{
    public static List<string> FilteredAccelerationUnits =
    [
        AccelerationUnit.MillimeterPerSecondSquared.ToString(),
        AccelerationUnit.CentimeterPerSecondSquared.ToString(),
        AccelerationUnit.MeterPerSecondSquared.ToString(),
        AccelerationUnit.KilometerPerSecondSquared.ToString(),
        AccelerationUnit.FootPerSecondSquared.ToString(),
        AccelerationUnit.InchPerSecondSquared.ToString(),
    ];

    public static List<string> FilteredAngleUnits =
    [
        AngleUnit.Radian.ToString(),
        AngleUnit.Degree.ToString()
    ];

    public static List<string> FilteredAreaMomentOfInertiaUnits =
    [
        AreaMomentOfInertiaUnit.MillimeterToTheFourth.ToString(),
        AreaMomentOfInertiaUnit.CentimeterToTheFourth.ToString(),
        AreaMomentOfInertiaUnit.MeterToTheFourth.ToString(),
        AreaMomentOfInertiaUnit.InchToTheFourth.ToString(),
        AreaMomentOfInertiaUnit.FootToTheFourth.ToString()
    ];

    public static List<string> FilteredAreaUnits =
    [
        AreaUnit.SquareMillimeter.ToString(),
        AreaUnit.SquareCentimeter.ToString(),
        AreaUnit.SquareMeter.ToString(),
        AreaUnit.SquareInch.ToString(),
        AreaUnit.SquareFoot.ToString()
    ];


    public static List<string> FilteredCurvatureUnits =
        [.. Enum.GetNames(typeof(ReciprocalLengthUnit)).ToList()];

    public static List<string> FilteredDensityUnits =
    [
        DensityUnit.GramPerCubicMillimeter.ToString(),
        DensityUnit.GramPerCubicCentimeter.ToString(),
        DensityUnit.GramPerCubicMeter.ToString(),
        DensityUnit.KilogramPerCubicMillimeter.ToString(),
        DensityUnit.KilogramPerCubicCentimeter.ToString(),
        DensityUnit.KilogramPerCubicMeter.ToString(),
        DensityUnit.TonnePerCubicMillimeter.ToString(),
        DensityUnit.TonnePerCubicCentimeter.ToString(),
        DensityUnit.TonnePerCubicMeter.ToString(),
        DensityUnit.PoundPerCubicFoot.ToString(),
        DensityUnit.PoundPerCubicInch.ToString(),
        DensityUnit.KilopoundPerCubicFoot.ToString(),
        DensityUnit.KilopoundPerCubicInch.ToString(),
        DensityUnit.SlugPerCubicFoot.ToString(),
    ];

    public static List<string> FilteredEnergyUnits =
    [
        EnergyUnit.Joule.ToString(),
        EnergyUnit.Kilojoule.ToString(),
        EnergyUnit.Megajoule.ToString(),
        EnergyUnit.Gigajoule.ToString(),
        EnergyUnit.KilowattHour.ToString(),
        EnergyUnit.FootPound.ToString(),
        EnergyUnit.Calorie.ToString(),
        EnergyUnit.BritishThermalUnit.ToString(),
    ];

    public static List<string> FilteredForcePerAreaUnits =
    [
        PressureUnit.NewtonPerSquareMillimeter.ToString(),
        PressureUnit.NewtonPerSquareCentimeter.ToString(),
        PressureUnit.NewtonPerSquareMeter.ToString(),
        PressureUnit.KilonewtonPerSquareCentimeter.ToString(),
        PressureUnit.KilonewtonPerSquareMillimeter.ToString(),
        PressureUnit.KilonewtonPerSquareMeter.ToString(),
        PressureUnit.PoundForcePerSquareInch.ToString(),
        PressureUnit.PoundForcePerSquareFoot.ToString(),
        PressureUnit.KilopoundForcePerSquareInch.ToString(),
        PressureUnit.KilopoundForcePerSquareFoot.ToString(),
    ];

    public static List<string> FilteredForcePerLengthUnits =
    [
        ForcePerLengthUnit.NewtonPerMillimeter.ToString(),
        ForcePerLengthUnit.NewtonPerCentimeter.ToString(),
        ForcePerLengthUnit.NewtonPerMeter.ToString(),
        ForcePerLengthUnit.KilonewtonPerMillimeter.ToString(),
        ForcePerLengthUnit.KilonewtonPerCentimeter.ToString(),
        ForcePerLengthUnit.KilonewtonPerMeter.ToString(),
        ForcePerLengthUnit.TonneForcePerCentimeter.ToString(),
        ForcePerLengthUnit.TonneForcePerMeter.ToString(),
        ForcePerLengthUnit.TonneForcePerMillimeter.ToString(),
        ForcePerLengthUnit.MeganewtonPerMeter.ToString(),
        ForcePerLengthUnit.PoundForcePerInch.ToString(),
        ForcePerLengthUnit.PoundForcePerFoot.ToString(),
        ForcePerLengthUnit.PoundForcePerYard.ToString(),
        ForcePerLengthUnit.KilopoundForcePerInch.ToString(),
        ForcePerLengthUnit.KilopoundForcePerFoot.ToString()
    ];

    public static List<string> FilteredRotationalStiffnessUnits =
    [
        RotationalStiffnessUnit.NewtonMeterPerDegree.ToString(),
        RotationalStiffnessUnit.NewtonMeterPerRadian.ToString(),
        RotationalStiffnessUnit.NewtonMillimeterPerDegree.ToString(),
        RotationalStiffnessUnit.NewtonMillimeterPerRadian.ToString(),
        RotationalStiffnessUnit.KilonewtonMeterPerDegree.ToString(),
        RotationalStiffnessUnit.KilonewtonMeterPerRadian.ToString(),
        RotationalStiffnessUnit.KilonewtonMillimeterPerDegree.ToString(),
        RotationalStiffnessUnit.KilonewtonMillimeterPerRadian.ToString(),
        RotationalStiffnessUnit.MeganewtonMeterPerDegree.ToString(),
        RotationalStiffnessUnit.MeganewtonMeterPerRadian.ToString(),
        RotationalStiffnessUnit.MeganewtonMillimeterPerDegree.ToString(),
        RotationalStiffnessUnit.MeganewtonMillimeterPerRadian.ToString(),
        RotationalStiffnessUnit.PoundForceFeetPerRadian.ToString(),
        RotationalStiffnessUnit.PoundForceFootPerDegrees.ToString(),
        RotationalStiffnessUnit.KilopoundForceFootPerDegrees.ToString()
    ];

    public static List<string> FilteredForceUnits =
    [
        ForceUnit.Newton.ToString(),
        ForceUnit.Kilonewton.ToString(),
        ForceUnit.Meganewton.ToString(),
        ForceUnit.PoundForce.ToString(),
        ForceUnit.KilopoundForce.ToString(),
        ForceUnit.TonneForce.ToString()
    ];

    public static List<string> FilteredLengthUnits =
    [
        LengthUnit.Millimeter.ToString(),
        LengthUnit.Centimeter.ToString(),
        LengthUnit.Meter.ToString(),
        LengthUnit.Inch.ToString(),
        LengthUnit.Foot.ToString()
    ];

    public static List<string> FilteredLinearDensityUnits =
    [
        LinearDensityUnit.GramPerMillimeter.ToString(),
        LinearDensityUnit.GramPerCentimeter.ToString(),
        LinearDensityUnit.GramPerMeter.ToString(),
        LinearDensityUnit.KilogramPerMillimeter.ToString(),
        LinearDensityUnit.KilogramPerCentimeter.ToString(),
        LinearDensityUnit.KilogramPerMeter.ToString(),
        LinearDensityUnit.PoundPerInch.ToString(),
        LinearDensityUnit.PoundPerFoot.ToString(),
    ];

    public static List<string> FilteredMassUnits =
    [
        MassUnit.Gram.ToString(),
        MassUnit.Kilogram.ToString(),
        MassUnit.Tonne.ToString(),
        MassUnit.Kilotonne.ToString(),
        MassUnit.Pound.ToString(),
        MassUnit.Kilopound.ToString(),
        MassUnit.Slug.ToString()
    ];

    public static List<string> FilteredMomentUnits =
        [.. Enum.GetNames(typeof(TorqueUnit)).ToList()];

    public static List<string> FilteredRatioUnits = [.. Enum.GetNames(typeof(RatioUnit)).ToList()];

    public static List<string> FilteredSectionModulusUnits =
    [
        VolumeUnit.CubicMillimeter.ToString(),
        VolumeUnit.CubicCentimeter.ToString(),
        VolumeUnit.CubicMeter.ToString(),
        VolumeUnit.CubicInch.ToString(),
        VolumeUnit.CubicFoot.ToString()
,
    ];
    public static List<string> FilteredStrainUnits =
        [.. Enum.GetNames(typeof(RatioUnit)).ToList()];

    public static List<string> FilteredStressUnits =
    [
        PressureUnit.Pascal.ToString(),
        PressureUnit.Kilopascal.ToString(),
        PressureUnit.Megapascal.ToString(),
        PressureUnit.Gigapascal.ToString(),
        PressureUnit.NewtonPerSquareMillimeter.ToString(),
        PressureUnit.NewtonPerSquareMeter.ToString(),
        PressureUnit.PoundForcePerSquareInch.ToString(),
        PressureUnit.PoundForcePerSquareFoot.ToString(),
        PressureUnit.KilopoundForcePerSquareInch.ToString(),
        PressureUnit.KilopoundForcePerSquareFoot.ToString()
    ];

    public static List<string> FilteredTemperatureUnits =
    [
        TemperatureUnit.DegreeCelsius.ToString(),
        TemperatureUnit.Kelvin.ToString(),
        TemperatureUnit.DegreeFahrenheit.ToString()
    ];

    public static List<string> FilteredTimeUnits =
    [
        DurationUnit.Millisecond.ToString(),
        DurationUnit.Second.ToString(),
        DurationUnit.Minute.ToString(),
        DurationUnit.Hour.ToString(),
        DurationUnit.Day.ToString(),
    ];

    public static List<string> FilteredVelocityUnits =
    [
        SpeedUnit.MillimeterPerSecond.ToString(),
        SpeedUnit.CentimeterPerSecond.ToString(),
        SpeedUnit.MeterPerSecond.ToString(),
        SpeedUnit.FootPerSecond.ToString(),
        SpeedUnit.InchPerSecond.ToString(),
        SpeedUnit.KilometerPerHour.ToString(),
        SpeedUnit.MilePerHour.ToString(),
    ];

    public static List<string> FilteredVolumePerLengthUnits =
    [
        VolumePerLengthUnit.CubicMeterPerMeter.ToString(),
        VolumePerLengthUnit.CubicYardPerFoot.ToString()
    ];

    public static List<string> FilteredVolumeUnits =
    [
        VolumeUnit.CubicMillimeter.ToString(),
        VolumeUnit.CubicCentimeter.ToString(),
        VolumeUnit.CubicMeter.ToString(),
        VolumeUnit.CubicInch.ToString(),
        VolumeUnit.CubicFoot.ToString()
    ];
}
