using Scaffold.Settings;
using VividOrange.Standards.Eurocode;

namespace Scaffold.Tests.Settings;

public class SettingsSingletonTests
{
    [Fact]
    public void Instance_ReturnsScaffoldSettings()
    {
        // Arrange & Act
        ScaffoldSettings instance = SettingsSingleton.Instance;

        // Assert
        Assert.NotNull(instance);
        Assert.IsType<ScaffoldSettings>(instance);
    }

    [Fact]
    public void Instance_ReturnsSameInstanceOnMultipleCalls()
    {
        // Arrange & Act
        ScaffoldSettings instance1 = SettingsSingleton.Instance;
        ScaffoldSettings instance2 = SettingsSingleton.Instance;

        // Assert
        Assert.Same(instance1, instance2);
    }

    [Fact]
    public void Instance_InitializesProjectSettings()
    {
        // Arrange & Act
        ScaffoldSettings instance = SettingsSingleton.Instance;

        // Assert
        Assert.NotNull(instance.Project);
        Assert.IsType<ProjectSettings>(instance.Project);
    }

    [Fact]
    public void Instance_InitializesUnitSettings()
    {
        // Arrange & Act
        ScaffoldSettings instance = SettingsSingleton.Instance;

        // Assert
        Assert.NotNull(instance.Units);
        Assert.IsType<UnitSettings>(instance.Units);
    }

    [Fact]
    public void Instance_ProjectSettings_HasDefaultValues()
    {
        // Arrange & Act
        ScaffoldSettings instance = SettingsSingleton.Instance;

        // Assert
        Assert.Equal(NationalAnnex.UnitedKingdom, instance.Project.NationalAnnex);
        Assert.Equal(string.Empty, instance.Project.ProjectName);
        Assert.Equal(string.Empty, instance.Project.ProjectNumber);
    }

    [Fact]
    public void Instance_UnitSettings_HasDefaultValues()
    {
        // Arrange & Act
        ScaffoldSettings instance = SettingsSingleton.Instance;

        // Assert
        Assert.Equal(4, instance.Units.SignificantDigits);
        Assert.NotNull(instance.Units.Units);
    }

    [Fact]
    public void Singleton_ProjectSettings_CanBeModified()
    {
        // Arrange
        ScaffoldSettings instance = SettingsSingleton.Instance;

        // Act
        instance.Project.ProjectName = "Test Project";

        // Assert
        Assert.Equal("Test Project", instance.Project.ProjectName);

        // Cleanup
        instance.Project.ProjectName = string.Empty;
    }

    [Fact]
    public void Singleton_UnitSettings_CanBeModified()
    {
        // Arrange
        ScaffoldSettings instance = SettingsSingleton.Instance;
        int originalDigits = instance.Units.SignificantDigits;

        // Act
        instance.Units.SignificantDigits = 7;

        // Assert
        Assert.Equal(7, instance.Units.SignificantDigits);

        // Cleanup
        instance.Units.SignificantDigits = originalDigits;
    }

    [Fact]
    public void ScaffoldSettings_DefaultConstructor_InitializesProperties()
    {
        // Arrange & Act
        var settings = new ScaffoldSettings();

        // Assert
        Assert.NotNull(settings.Project);
        Assert.NotNull(settings.Units);
    }

    [Fact]
    public void ScaffoldSettings_ProjectSettings_CanBeReplaced()
    {
        // Arrange
        var settings = new ScaffoldSettings();
        var newProjectSettings = new ProjectSettings { ProjectName = "New Project" };

        // Act
        settings.Project = newProjectSettings;

        // Assert
        Assert.Same(newProjectSettings, settings.Project);
        Assert.Equal("New Project", settings.Project.ProjectName);
    }

    [Fact]
    public void ScaffoldSettings_UnitSettings_CanBeReplaced()
    {
        // Arrange
        var settings = new ScaffoldSettings();
        var newUnitSettings = new UnitSettings { SignificantDigits = 6 };

        // Act
        settings.Units = newUnitSettings;

        // Assert
        Assert.Same(newUnitSettings, settings.Units);
        Assert.Equal(6, settings.Units.SignificantDigits);
    }

    [Fact]
    public void ScaffoldSettings_CanBeSavedToJsonFile()
    {
        // Arrange
        string tempDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string tempFilePath = Path.Combine(tempDir, "Scaffold", $"settings-{Guid.NewGuid()}.json");
        var settings = new ScaffoldSettings
        {
            Project = new ProjectSettings
            {
                ProjectName = "Save Test",
                ProjectNumber = "ST-001",
                NationalAnnex = NationalAnnex.Germany
            },
            Units = new UnitSettings { SignificantDigits = 5 }
        };

        try
        {
            // Act
            settings.SaveToJsonFile(tempFilePath);

            // Assert
            Assert.True(File.Exists(tempFilePath));
            string fileContent = File.ReadAllText(tempFilePath);
            Assert.Contains("Save Test", fileContent);
            Assert.Contains("ST-001", fileContent);
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    [Fact]
    public void ScaffoldSettings_CanBeLoadedFromJsonFile()
    {
        // Arrange
        string tempDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string tempFilePath = Path.Combine(tempDir, "Scaffold", $"settings-{Guid.NewGuid()}.json");
        var originalSettings = new ScaffoldSettings
        {
            Project = new ProjectSettings
            {
                ProjectName = "Load Test",
                ProjectNumber = "LT-002",
                NationalAnnex = NationalAnnex.France
            },
            Units = new UnitSettings { SignificantDigits = 8 }
        };

        try
        {
            originalSettings.SaveToJsonFile(tempFilePath);

            // Act
            ScaffoldSettings loadedSettings = JsonSerializationExtensions.LoadFromJsonFile<ScaffoldSettings>(tempFilePath);

            // Assert
            Assert.NotNull(loadedSettings);
            Assert.Equal("Load Test", loadedSettings.Project.ProjectName);
            Assert.Equal("LT-002", loadedSettings.Project.ProjectNumber);
            Assert.Equal(NationalAnnex.France, loadedSettings.Project.NationalAnnex);
            Assert.Equal(8, loadedSettings.Units.SignificantDigits);
        }
        finally
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    [Fact]
    public void Singleton_CanBeSavedToJsonFile()
    {
        // Arrange
        string tempDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string tempFilePath = Path.Combine(tempDir, "Scaffold", $"singleton-{Guid.NewGuid()}.json");
        ScaffoldSettings instance = SettingsSingleton.Instance;
        string originalProjectName = instance.Project.ProjectName;
        int originalDigits = instance.Units.SignificantDigits;

        try
        {
            // Act - modify singleton and save
            instance.Project.ProjectName = "Singleton Test";
            instance.Units.SignificantDigits = 9;
            instance.SaveToJsonFile(tempFilePath);

            // Assert
            Assert.True(File.Exists(tempFilePath));
            string fileContent = File.ReadAllText(tempFilePath);
            Assert.Contains("Singleton Test", fileContent);
        }
        finally
        {
            // Cleanup
            instance.Project.ProjectName = originalProjectName;
            instance.Units.SignificantDigits = originalDigits;
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    [Fact]
    public void Singleton_CanBeLoadedFromJsonFile()
    {
        // Arrange
        string tempDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string tempFilePath = Path.Combine(tempDir, "Scaffold", $"singleton-{Guid.NewGuid()}.json");
        ScaffoldSettings instance = SettingsSingleton.Instance;
        string originalProjectName = instance.Project.ProjectName;
        string originalProjectNumber = instance.Project.ProjectNumber;
        int originalDigits = instance.Units.SignificantDigits;

        try
        {
            // Arrange - create a saved settings file
            var savedSettings = new ScaffoldSettings
            {
                Project = new ProjectSettings
                {
                    ProjectName = "Loaded From File",
                    ProjectNumber = "LFF-999",
                    NationalAnnex = NationalAnnex.UnitedKingdom
                },
                Units = new UnitSettings { SignificantDigits = 10 }
            };
            savedSettings.SaveToJsonFile(tempFilePath);

            // Act
            ScaffoldSettings loaded = JsonSerializationExtensions.LoadFromJsonFile<ScaffoldSettings>(tempFilePath);
            if (loaded != null)
            {
                instance.Project = loaded.Project;
                instance.Units = loaded.Units;
            }

            // Assert
            Assert.Equal("Loaded From File", instance.Project.ProjectName);
            Assert.Equal("LFF-999", instance.Project.ProjectNumber);
            Assert.Equal(10, instance.Units.SignificantDigits);
        }
        finally
        {
            // Cleanup
            instance.Project.ProjectName = originalProjectName;
            instance.Project.ProjectNumber = originalProjectNumber;
            instance.Units.SignificantDigits = originalDigits;
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    [Fact]
    public void ScaffoldSettings_Parse_DeserializesFromJsonString()
    {
        // Arrange
        var original = new ScaffoldSettings
        {
            Project = new ProjectSettings
            {
                ProjectName = "Parse Test",
                ProjectNumber = "PT-001",
                NationalAnnex = NationalAnnex.Germany
            },
            Units = new UnitSettings { SignificantDigits = 6 }
        };
        string json = ((IFormattable)original).ToString(null, null);

        // Act
        var parsed = ScaffoldSettings.Parse(json, null);

        // Assert
        Assert.NotNull(parsed);
        Assert.Equal("Parse Test", parsed.Project.ProjectName);
        Assert.Equal("PT-001", parsed.Project.ProjectNumber);
        Assert.Equal(NationalAnnex.Germany, parsed.Project.NationalAnnex);
        Assert.Equal(6, parsed.Units.SignificantDigits);
    }

    [Fact]
    public void ScaffoldSettings_TryParse_SucceedsWithValidJson()
    {
        // Arrange
        var original = new ScaffoldSettings
        {
            Project = new ProjectSettings { ProjectName = "TryParse Test" },
            Units = new UnitSettings { SignificantDigits = 5 }
        };
        string json = ((IFormattable)original).ToString(null, null);

        // Act
        bool success = ScaffoldSettings.TryParse(json, null, out ScaffoldSettings parsed);

        // Assert
        Assert.True(success);
        Assert.NotNull(parsed);
        Assert.Equal("TryParse Test", parsed.Project.ProjectName);
        Assert.Equal(5, parsed.Units.SignificantDigits);
    }

    [Fact]
    public void ScaffoldSettings_TryParse_ReturnsFalseWithInvalidJson()
    {
        // Arrange
        string invalidJson = "{ invalid json }";

        // Act
        bool success = ScaffoldSettings.TryParse(invalidJson, null, out ScaffoldSettings parsed);

        // Assert
        Assert.False(success);
        Assert.Null(parsed);
    }

    [Fact]
    public void ScaffoldSettings_ToString_SerializesToJson()
    {
        // Arrange
        var settings = new ScaffoldSettings
        {
            Project = new ProjectSettings
            {
                ProjectName = "ToString Test",
                ProjectNumber = "TT-001"
            },
            Units = new UnitSettings { SignificantDigits = 7 }
        };

        // Act
        string json = ((IFormattable)settings).ToString(null, null);

        // Assert
        Assert.NotNull(json);
        Assert.NotEmpty(json);
        Assert.Contains("ToString Test", json);
        Assert.Contains("TT-001", json);
    }
}
