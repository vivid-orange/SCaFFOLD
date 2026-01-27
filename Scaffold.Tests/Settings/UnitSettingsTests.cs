using Xunit;

namespace Scaffold.Tests.Settings;

public class UnitSettingsTests
{
    [Fact]
    public void DefaultConstructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var settings = new UnitSettings();

        // Assert
        Assert.Equal(4, settings.SignificantDigits);
        Assert.NotNull(settings.UnitSystem);
    }

    [Fact]
    public void SignificantDigits_CanBeSet()
    {
        // Arrange
        var settings = new UnitSettings();

        // Act
        settings.SignificantDigits = 6;

        // Assert
        Assert.Equal(6, settings.SignificantDigits);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(8)]
    [InlineData(12)]
    public void SignificantDigits_CanBeSetToDifferentValues(int digits)
    {
        // Arrange
        var settings = new UnitSettings();

        // Act
        settings.SignificantDigits = digits;

        // Assert
        Assert.Equal(digits, settings.SignificantDigits);
    }

    [Fact]
    public void UnitSystem_IsInitializedOnConstruction()
    {
        // Arrange & Act
        var settings = new UnitSettings();

        // Assert
        Assert.NotNull(settings.UnitSystem);
    }

    [Fact]
    public void UnitSystem_CanBeReplaced()
    {
        // Arrange
        var settings = new UnitSettings();
        var newUnitSystem = new UnitSystem();

        // Act
        settings.UnitSystem = newUnitSystem;

        // Assert
        Assert.Same(newUnitSystem, settings.UnitSystem);
    }

    [Fact]
    public void MultipleInstances_HaveIndependentUnitSystems()
    {
        // Arrange & Act
        var settings1 = new UnitSettings();
        var settings2 = new UnitSettings();

        // Assert
        Assert.NotSame(settings1.UnitSystem, settings2.UnitSystem);
    }
}
