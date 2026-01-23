internal class UnitSystem
{
    internal AccelerationUnit AccelerationUnit { get; }
    internal ReciprocalLengthUnit CurvatureUnit { get; }
    internal DensityUnit DensityUnit { get; }
    internal EnergyUnit EnergyUnit { get; }
    internal PressureUnit ForcePerAreaUnit { get; }
    internal ForcePerLengthUnit ForcePerLengthUnit { get; }
    internal ForceUnit ForceUnit { get; }
    internal LengthUnit GeometryLengthUnit { get; }
    internal LengthUnit DisplacementLengthUnit { get; }
    internal LinearDensityUnit LinearDensityUnit { get; }
    internal MassUnit MassUnit { get; }
    internal RatioUnit MaterialStrainUnit { get; }
    internal PressureUnit MaterialStrengthUnit { get; }
    internal TorqueUnit MomentUnit { get; }
    internal AreaMomentOfInertiaUnit SectionAreaMomentOfInertiaUnit { get; }
    internal AreaUnit SectionAreaUnit { get; }
    internal LengthUnit SectionLengthUnit { get; }
    internal VolumeUnit SectionModulusUnit { get; }
    internal VolumeUnit SectionVolumeUnit { get; }
    internal RatioUnit StrainUnit { get; }
    internal PressureUnit StressUnit { get; }
    internal TemperatureUnit TemperatureUnit { get; }
    internal SpeedUnit VelocityUnit { get; }
    internal VolumePerLengthUnit VolumePerLengthUnit { get; }
    internal PressureUnit YoungsModulusUnit { get; }

    internal UnitSystem()
    {
        SectionLengthUnit = DefaultUnits.LengthUnitSection;
        SectionAreaUnit = DefaultUnits.SectionAreaUnit;
        SectionVolumeUnit = DefaultUnits.SectionVolumeUnit;
        SectionAreaMomentOfInertiaUnit = DefaultUnits.SectionAreaMomentOfInertiaUnit;
        MassUnit = DefaultUnits.MassUnit;
        DensityUnit = DefaultUnits.DensityUnit;
        LinearDensityUnit = DefaultUnits.LinearDensityUnit;
        VolumePerLengthUnit = DefaultUnits.VolumePerLengthUnit;
        MaterialStrengthUnit = DefaultUnits.MaterialStrengthUnit;
        MaterialStrainUnit = DefaultUnits.MaterialStrainUnit;
        YoungsModulusUnit = DefaultUnits.YoungsModulusUnit;
        GeometryLengthUnit = DefaultUnits.LengthUnitGeometry;
        ForceUnit = DefaultUnits.ForceUnit;
        ForcePerLengthUnit = DefaultUnits.ForcePerLengthUnit;
        ForcePerAreaUnit = DefaultUnits.ForcePerAreaUnit;
        MomentUnit = DefaultUnits.MomentUnit;
        TemperatureUnit = DefaultUnits.TemperatureUnit;
        DisplacementLengthUnit = DefaultUnits.DisplacementLengthUnit;
        StressUnit = DefaultUnits.StressUnit;
        StrainUnit = DefaultUnits.StrainUnit;
        VelocityUnit = DefaultUnits.VelocityUnit;
        AccelerationUnit = DefaultUnits.AccelerationUnit;
        EnergyUnit = DefaultUnits.EnergyUnit;
        CurvatureUnit = DefaultUnits.CurvatureUnit;
        SectionModulusUnit = DefaultUnits.SectionModulusUnit;
    }

    internal UnitSystem(LengthUnit sectionLengthUnit, AreaUnit areaUnit, VolumeUnit volumeUnit,
        AreaMomentOfInertiaUnit areaMomentOfInertiaUnit, MassUnit massUnit,
        DensityUnit densityUnit, LinearDensityUnit linearDensityUnit,
        VolumePerLengthUnit volumePerLengthUnit, PressureUnit materialStrengthUnit,
        RatioUnit materialStrainUnit, PressureUnit youngsModulusUnit, LengthUnit lengthUnit,
        ForceUnit forceUnit, ForcePerLengthUnit forcePerLengthUnit,
        PressureUnit forcePerAreaUnit, TorqueUnit momentUnit, TemperatureUnit temperatureUnit,
        LengthUnit displacementUuit, PressureUnit stressUnit, RatioUnit strainUnit,
        SpeedUnit velocityUnit, AccelerationUnit accelerationUnit, EnergyUnit energyUnit,
        ReciprocalLengthUnit curvatureUnit, VolumeUnit sectionModulusUnit)
    {
        SectionLengthUnit = sectionLengthUnit;
        SectionAreaUnit = areaUnit;
        SectionVolumeUnit = volumeUnit;
        SectionAreaMomentOfInertiaUnit = areaMomentOfInertiaUnit;
        MassUnit = massUnit;
        DensityUnit = densityUnit;
        LinearDensityUnit = linearDensityUnit;
        VolumePerLengthUnit = volumePerLengthUnit;
        MaterialStrengthUnit = materialStrengthUnit;
        MaterialStrainUnit = materialStrainUnit;
        YoungsModulusUnit = youngsModulusUnit;
        GeometryLengthUnit = lengthUnit;
        ForceUnit = forceUnit;
        ForcePerLengthUnit = forcePerLengthUnit;
        ForcePerAreaUnit = forcePerAreaUnit;
        MomentUnit = momentUnit;
        TemperatureUnit = temperatureUnit;
        DisplacementLengthUnit = displacementUuit;
        StressUnit = stressUnit;
        StrainUnit = strainUnit;
        VelocityUnit = velocityUnit;
        AccelerationUnit = accelerationUnit;
        EnergyUnit = energyUnit;
        CurvatureUnit = curvatureUnit;
        SectionModulusUnit = sectionModulusUnit;
    }
}
