using Xunit;
using VividOrange.Standards.Eurocode;

namespace Scaffold.Tests.Settings;

public class SettingsSingletonTests
{
    [Fact]
    public void Instance_ReturnsScaffoldSettings()
    {
        // Arrange & Act
        var instance = SettingsSingleton.Instance;

        // Assert
        Assert.NotNull(instance);
        Assert.IsType<ScaffoldSettings>(instance);
    }

    [Fact]
    public void Instance_ReturnsSameInstanceOnMultipleCalls()
    {
        // Arrange & Act
        var instance1 = SettingsSingleton.Instance;
        var instance2 = SettingsSingleton.Instance;

        // Assert
        Assert.Same(instance1, instance2);
    }

    [Fact]
    public void Instance_InitializesProjectSettings()
    {
        // Arrange & Act
        var instance = SettingsSingleton.Instance;

        // Assert
        Assert.NotNull(instance.ProjectSettings);
        Assert.IsType<ProjectSettings>(instance.ProjectSettings);
    }

    [Fact]
    public void Instance_InitializesUnitSettings()
    {
        // Arrange & Act
        var instance = SettingsSingleton.Instance;

        // Assert
        Assert.NotNull(instance.UnitSettings);
        Assert.IsType<UnitSettings>(instance.UnitSettings);
    }

    [Fact]
    public void Instance_ProjectSettings_HasDefaultValues()
    {
        // Arrange & Act
        var instance = SettingsSingleton.Instance;

        // Assert
        Assert.Equal(NationalAnnex.UnitedKingdom, instance.ProjectSettings.NationalAnnex);
        Assert.Equal(string.Empty, instance.ProjectSettings.ProjectName);
        Assert.Equal(string.Empty, instance.ProjectSettings.ProjectNumber);
    }

    [Fact]
    public void Instance_UnitSettings_HasDefaultValues()
    {
        // Arrange & Act
        var instance = SettingsSingleton.Instance;

        // Assert
        Assert.Equal(4, instance.UnitSettings.SignificantDigits);
        Assert.NotNull(instance.UnitSettings.UnitSystem);
    }

    [Fact]
    public void Singleton_ProjectSettings_CanBeModified()
    {
        // Arrange
        var instance = SettingsSingleton.Instance;

        // Act
        instance.ProjectSettings.ProjectName = "Test Project";

        // Assert
        Assert.Equal("Test Project", instance.ProjectSettings.ProjectName);

        // Cleanup
        instance.ProjectSettings.ProjectName = string.Empty;
    }

    [Fact]
    public void Singleton_UnitSettings_CanBeModified()
    {
        // Arrange
        var instance = SettingsSingleton.Instance;
        var originalDigits = instance.UnitSettings.SignificantDigits;

        // Act
        instance.UnitSettings.SignificantDigits = 7;

        // Assert
        Assert.Equal(7, instance.UnitSettings.SignificantDigits);

        // Cleanup
        instance.UnitSettings.SignificantDigits = originalDigits;
    }

    [Fact]
    public void ScaffoldSettings_DefaultConstructor_InitializesProperties()
    {
        // Arrange & Act
        var settings = new ScaffoldSettings();

        // Assert
        Assert.NotNull(settings.ProjectSettings);
        Assert.NotNull(settings.UnitSettings);
    }

    [Fact]
    public void ScaffoldSettings_ProjectSettings_CanBeReplaced()
    {
        // Arrange
        var settings = new ScaffoldSettings();
        var newProjectSettings = new ProjectSettings { ProjectName = "New Project" };

        // Act
        settings.ProjectSettings = newProjectSettings;

        // Assert
        Assert.Same(newProjectSettings, settings.ProjectSettings);
        Assert.Equal("New Project", settings.ProjectSettings.ProjectName);
    }

    [Fact]
    public void ScaffoldSettings_UnitSettings_CanBeReplaced()
    {
        // Arrange
        var settings = new ScaffoldSettings();
        var newUnitSettings = new UnitSettings { SignificantDigits = 6 };

        // Act
        settings.UnitSettings = newUnitSettings;

        // Assert
        Assert.Same(newUnitSettings, settings.UnitSettings);
        Assert.Equal(6, settings.UnitSettings.SignificantDigits);
    }
}
