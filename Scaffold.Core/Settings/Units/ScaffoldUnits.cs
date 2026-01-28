namespace Scaffold.Settings.Units;

public class ScaffoldUnits
{
    public AccelerationUnit AccelerationUnit { get; set; } = DefaultUnits.AccelerationUnit;
    public ReciprocalLengthUnit CurvatureUnit { get; set; } = DefaultUnits.CurvatureUnit;
    public DensityUnit DensityUnit { get; set; } = DefaultUnits.DensityUnit;
    public EnergyUnit EnergyUnit { get; set; } = DefaultUnits.EnergyUnit;
    public PressureUnit ForcePerAreaUnit { get; set; } = DefaultUnits.ForcePerAreaUnit;
    public ForcePerLengthUnit ForcePerLengthUnit { get; set; } = DefaultUnits.ForcePerLengthUnit;
    public ForceUnit ForceUnit { get; set; } = DefaultUnits.ForceUnit;
    public LengthUnit GeometryLengthUnit { get; set; } = DefaultUnits.LengthUnitGeometry;
    public LengthUnit DisplacementLengthUnit { get; set; } = DefaultUnits.DisplacementLengthUnit;
    public LinearDensityUnit LinearDensityUnit { get; set; } = DefaultUnits.LinearDensityUnit;
    public MassUnit MassUnit { get; set; } = DefaultUnits.MassUnit;
    public RatioUnit MaterialStrainUnit { get; set; } = DefaultUnits.MaterialStrainUnit;
    public PressureUnit MaterialStrengthUnit { get; set; } = DefaultUnits.MaterialStrengthUnit;
    public TorqueUnit MomentUnit { get; set; } = DefaultUnits.MomentUnit;
    public AreaMomentOfInertiaUnit SectionAreaMomentOfInertiaUnit { get; set; } = DefaultUnits.SectionAreaMomentOfInertiaUnit;
    public AreaUnit SectionAreaUnit { get; set; } = DefaultUnits.SectionAreaUnit;
    public LengthUnit SectionLengthUnit { get; set; } = DefaultUnits.LengthUnitSection;
    public VolumeUnit SectionModulusUnit { get; set; } = DefaultUnits.SectionModulusUnit;
    public VolumeUnit SectionVolumeUnit { get; set; } = DefaultUnits.SectionVolumeUnit;
    public RatioUnit StrainUnit { get; set; } = DefaultUnits.StrainUnit;
    public PressureUnit StressUnit { get; set; } = DefaultUnits.StressUnit;
    public TemperatureUnit TemperatureUnit { get; set; } = DefaultUnits.TemperatureUnit;
    public SpeedUnit VelocityUnit { get; set; } = DefaultUnits.VelocityUnit;
    public VolumePerLengthUnit VolumePerLengthUnit { get; set; } = DefaultUnits.VolumePerLengthUnit;
    public PressureUnit YoungsModulusUnit { get; set; } = DefaultUnits.YoungsModulusUnit;

    public CoefficientOfThermalExpansionUnit CoefficientOfThermalExpansionUnit
    {
        get
        {
            return TemperatureUnit.GetCoefficientOfThermalExpansionUnit();
        }
    }
}
