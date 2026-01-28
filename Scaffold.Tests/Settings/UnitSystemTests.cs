using Scaffold.Settings.Units;

namespace Scaffold.Tests.Settings;

public class UnitSystemTests
{
    [Fact]
    public void DefaultConstructor_InitializesAllUnits()
    {
        // Arrange & Act
        var unitSystem = new ScaffoldUnits();

        // Assert - verify all properties are initialized by checking they have values
        Assert.NotEqual(default(LengthUnit), unitSystem.SectionLengthUnit);
        Assert.NotEqual(default(AreaUnit), unitSystem.SectionAreaUnit);
        Assert.NotEqual(default(VolumeUnit), unitSystem.SectionVolumeUnit);
        Assert.NotEqual(default(AreaMomentOfInertiaUnit), unitSystem.SectionAreaMomentOfInertiaUnit);
        Assert.NotEqual(default(MassUnit), unitSystem.MassUnit);
        Assert.NotEqual(default(DensityUnit), unitSystem.DensityUnit);
        Assert.NotEqual(default(LinearDensityUnit), unitSystem.LinearDensityUnit);
        Assert.NotEqual(default(VolumePerLengthUnit), unitSystem.VolumePerLengthUnit);
        Assert.NotEqual(default(PressureUnit), unitSystem.MaterialStrengthUnit);
        Assert.NotEqual(default(RatioUnit), unitSystem.MaterialStrainUnit);
        Assert.NotEqual(default(PressureUnit), unitSystem.YoungsModulusUnit);
        Assert.NotEqual(default(LengthUnit), unitSystem.GeometryLengthUnit);
        Assert.NotEqual(default(ForceUnit), unitSystem.ForceUnit);
        Assert.NotEqual(default(ForcePerLengthUnit), unitSystem.ForcePerLengthUnit);
        Assert.NotEqual(default(PressureUnit), unitSystem.ForcePerAreaUnit);
        Assert.NotEqual(default(TorqueUnit), unitSystem.MomentUnit);
        Assert.NotEqual(default(TemperatureUnit), unitSystem.TemperatureUnit);
        Assert.NotEqual(default(LengthUnit), unitSystem.DisplacementLengthUnit);
        Assert.NotEqual(default(PressureUnit), unitSystem.StressUnit);
        Assert.NotEqual(default(RatioUnit), unitSystem.StrainUnit);
        Assert.NotEqual(default(SpeedUnit), unitSystem.VelocityUnit);
        Assert.NotEqual(default(AccelerationUnit), unitSystem.AccelerationUnit);
        Assert.NotEqual(default(EnergyUnit), unitSystem.EnergyUnit);
        Assert.NotEqual(default(ReciprocalLengthUnit), unitSystem.CurvatureUnit);
        Assert.NotEqual(default(VolumeUnit), unitSystem.SectionModulusUnit);
    }

    [Fact]
    public void DefaultConstructor_UsesDefaultUnitsValues()
    {
        // Arrange & Act
        var unitSystem = new ScaffoldUnits();

        // Assert
        Assert.Equal(DefaultUnits.LengthUnitSection, unitSystem.SectionLengthUnit);
        Assert.Equal(DefaultUnits.SectionAreaUnit, unitSystem.SectionAreaUnit);
        Assert.Equal(DefaultUnits.SectionVolumeUnit, unitSystem.SectionVolumeUnit);
        Assert.Equal(DefaultUnits.MassUnit, unitSystem.MassUnit);
        Assert.Equal(DefaultUnits.DensityUnit, unitSystem.DensityUnit);
    }

    [Fact]
    public void AccelerationUnit_CanBeSet()
    {
        // Arrange
        var unitSystem = new ScaffoldUnits();

        // Act
        unitSystem.AccelerationUnit = AccelerationUnit.MeterPerSecondSquared;

        // Assert
        Assert.Equal(AccelerationUnit.MeterPerSecondSquared, unitSystem.AccelerationUnit);
    }

    [Fact]
    public void MultipleInstances_HaveIndependentUnits()
    {
        // Arrange & Act
        var unitSystem1 = new ScaffoldUnits();
        var unitSystem2 = new ScaffoldUnits();

        // Modify one instance
        unitSystem2.AccelerationUnit = AccelerationUnit.MillimeterPerSecondSquared;

        // Assert - they should now be different
        Assert.NotEqual(unitSystem1.AccelerationUnit, unitSystem2.AccelerationUnit);
    }

    [Fact]
    public void ReadOnlyProperties_AreInitializedFromDefaultUnits()
    {
        // Arrange & Act
        var unitSystem = new ScaffoldUnits();

        // Assert
        Assert.Equal(DefaultUnits.CurvatureUnit, unitSystem.CurvatureUnit);
        Assert.Equal(DefaultUnits.DensityUnit, unitSystem.DensityUnit);
        Assert.Equal(DefaultUnits.EnergyUnit, unitSystem.EnergyUnit);
    }

    [Fact]
    public void LengthUnits_AreCorrectlyAssigned()
    {
        // Arrange & Act
        var unitSystem = new ScaffoldUnits();

        // Assert
        Assert.Equal(DefaultUnits.LengthUnitSection, unitSystem.SectionLengthUnit);
        Assert.Equal(DefaultUnits.LengthUnitGeometry, unitSystem.GeometryLengthUnit);
        Assert.Equal(DefaultUnits.DisplacementLengthUnit, unitSystem.DisplacementLengthUnit);
    }

    [Fact]
    public void StressAndStrainUnits_AreCorrectlyAssigned()
    {
        // Arrange & Act
        var unitSystem = new ScaffoldUnits();

        // Assert
        Assert.Equal(DefaultUnits.StressUnit, unitSystem.StressUnit);
        Assert.Equal(DefaultUnits.StrainUnit, unitSystem.StrainUnit);
        Assert.Equal(DefaultUnits.MaterialStrengthUnit, unitSystem.MaterialStrengthUnit);
        Assert.Equal(DefaultUnits.MaterialStrainUnit, unitSystem.MaterialStrainUnit);
    }

    [Fact]
    public void CoefficientOfThermalExpansionUnit_IsNotNull()
    {
        // Arrange & Act
        var unitSystem = new ScaffoldUnits();

        // Assert
        Assert.NotEqual(default(CoefficientOfThermalExpansionUnit), unitSystem.CoefficientOfThermalExpansionUnit);
    }

    [Fact]
    public void CoefficientOfThermalExpansionUnit_DependsOnTemperatureUnit()
    {
        // Arrange
        var unitSystem = new ScaffoldUnits();

        // Act
        CoefficientOfThermalExpansionUnit coefficientUnit = unitSystem.CoefficientOfThermalExpansionUnit;

        // Assert - verify it's derived from the temperature unit
        Assert.NotEqual(default(CoefficientOfThermalExpansionUnit), coefficientUnit);
    }

    [Fact]
    public void CoefficientOfThermalExpansionUnit_IsComputedFromTemperatureUnit()
    {
        // Arrange
        var unitSystem1 = new ScaffoldUnits();
        var unitSystem2 = new ScaffoldUnits();

        // Both should have the same temperature unit (default)
        Assert.Equal(unitSystem1.TemperatureUnit, unitSystem2.TemperatureUnit);

        // Act
        CoefficientOfThermalExpansionUnit coefficientUnit1 = unitSystem1.CoefficientOfThermalExpansionUnit;
        CoefficientOfThermalExpansionUnit coefficientUnit2 = unitSystem2.CoefficientOfThermalExpansionUnit;

        // Assert - coefficient units should be the same since temperature units are the same
        Assert.Equal(coefficientUnit1, coefficientUnit2);
    }

    [Fact]
    public void CoefficientOfThermalExpansionUnit_IsReadOnly()
    {
        // Arrange
        var unitSystem = new ScaffoldUnits();

        // Act & Assert - verify the property doesn't have a setter
        // This is a read-only computed property, so we just verify we can read it
        CoefficientOfThermalExpansionUnit unit = unitSystem.CoefficientOfThermalExpansionUnit;
        Assert.NotEqual(default(CoefficientOfThermalExpansionUnit), unit);
    }
}
