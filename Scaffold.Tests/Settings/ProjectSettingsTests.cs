using Scaffold.Settings;
using VividOrange.Standards.Eurocode;

namespace Scaffold.Tests.Settings;

public class ProjectSettingsTests
{
    [Fact]
    public void DefaultConstructor_SetsDefaultValues()
    {
        // Arrange & Act
        var settings = new ProjectSettings();

        // Assert
        Assert.Equal(NationalAnnex.UnitedKingdom, settings.NationalAnnex);
        Assert.Equal(string.Empty, settings.ProjectName);
        Assert.Equal(string.Empty, settings.ProjectNumber);
    }

    [Fact]
    public void NationalAnnex_CanBeSet()
    {
        // Arrange
        var settings = new ProjectSettings();

        // Act
        settings.NationalAnnex = NationalAnnex.Germany;

        // Assert
        Assert.Equal(NationalAnnex.Germany, settings.NationalAnnex);
    }

    [Theory]
    [InlineData("TestProject")]
    [InlineData("My Project 123")]
    [InlineData(string.Empty)]
    public void ProjectName_CanBeSet(string projectName)
    {
        // Arrange
        var settings = new ProjectSettings();

        // Act
        settings.ProjectName = projectName;

        // Assert
        Assert.Equal(projectName, settings.ProjectName);
    }

    [Theory]
    [InlineData("PRJ-001")]
    [InlineData("123456")]
    [InlineData(string.Empty)]
    public void ProjectNumber_CanBeSet(string projectNumber)
    {
        // Arrange
        var settings = new ProjectSettings();

        // Act
        settings.ProjectNumber = projectNumber;

        // Assert
        Assert.Equal(projectNumber, settings.ProjectNumber);
    }

    [Fact]
    public void MultipleProperties_CanBeSetIndependently()
    {
        // Arrange
        var settings = new ProjectSettings();

        // Act
        settings.NationalAnnex = NationalAnnex.France;
        settings.ProjectName = "Bridge Project";
        settings.ProjectNumber = "BP-2024-001";

        // Assert
        Assert.Equal(NationalAnnex.France, settings.NationalAnnex);
        Assert.Equal("Bridge Project", settings.ProjectName);
        Assert.Equal("BP-2024-001", settings.ProjectNumber);
    }
}
