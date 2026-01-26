public class UnitSystem
{
    public AccelerationUnit AccelerationUnit { get; set; }
    public ReciprocalLengthUnit CurvatureUnit { get; }
    public DensityUnit DensityUnit { get; }
    public EnergyUnit EnergyUnit { get; }
    public PressureUnit ForcePerAreaUnit { get; }
    public ForcePerLengthUnit ForcePerLengthUnit { get; }
    public ForceUnit ForceUnit { get; }
    public LengthUnit GeometryLengthUnit { get; }
    public LengthUnit DisplacementLengthUnit { get; }
    public LinearDensityUnit LinearDensityUnit { get; }
    public MassUnit MassUnit { get; }
    public RatioUnit MaterialStrainUnit { get; }
    public PressureUnit MaterialStrengthUnit { get; }
    public TorqueUnit MomentUnit { get; }
    public AreaMomentOfInertiaUnit SectionAreaMomentOfInertiaUnit { get; }
    public AreaUnit SectionAreaUnit { get; }
    public LengthUnit SectionLengthUnit { get; }
    public VolumeUnit SectionModulusUnit { get; }
    public VolumeUnit SectionVolumeUnit { get; }
    public RatioUnit StrainUnit { get; }
    public PressureUnit StressUnit { get; }
    public TemperatureUnit TemperatureUnit { get; }
    public SpeedUnit VelocityUnit { get; }
    public VolumePerLengthUnit VolumePerLengthUnit { get; }
    public PressureUnit YoungsModulusUnit { get; }

    public UnitSystem()
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
}
