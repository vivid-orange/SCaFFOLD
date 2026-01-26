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
        Assert.NotNull(instance.ProjectSettings);
        Assert.IsType<ProjectSettings>(instance.ProjectSettings);
    }

    [Fact]
    public void Instance_InitializesUnitSettings()
    {
        // Arrange & Act
        ScaffoldSettings instance = SettingsSingleton.Instance;

        // Assert
        Assert.NotNull(instance.UnitSettings);
        Assert.IsType<UnitSettings>(instance.UnitSettings);
    }

    [Fact]
    public void Instance_ProjectSettings_HasDefaultValues()
    {
        // Arrange & Act
        ScaffoldSettings instance = SettingsSingleton.Instance;

        // Assert
        Assert.Equal(NationalAnnex.UnitedKingdom, instance.ProjectSettings.NationalAnnex);
        Assert.Equal(string.Empty, instance.ProjectSettings.ProjectName);
        Assert.Equal(string.Empty, instance.ProjectSettings.ProjectNumber);
    }

    [Fact]
    public void Instance_UnitSettings_HasDefaultValues()
    {
        // Arrange & Act
        ScaffoldSettings instance = SettingsSingleton.Instance;

        // Assert
        Assert.Equal(4, instance.UnitSettings.SignificantDigits);
        Assert.NotNull(instance.UnitSettings.UnitSystem);
    }

    [Fact]
    public void Singleton_ProjectSettings_CanBeModified()
    {
        // Arrange
        ScaffoldSettings instance = SettingsSingleton.Instance;

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
        ScaffoldSettings instance = SettingsSingleton.Instance;
        int originalDigits = instance.UnitSettings.SignificantDigits;

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

    [Fact]
    public void ScaffoldSettings_CanBeSavedToJsonFile()
    {
        // Arrange
        string tempDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string tempFilePath = Path.Combine(tempDir, "Scaffold", $"settings-{Guid.NewGuid()}.json");
        var settings = new ScaffoldSettings
        {
            ProjectSettings = new ProjectSettings
            {
                ProjectName = "Save Test",
                ProjectNumber = "ST-001",
                NationalAnnex = NationalAnnex.Germany
            },
            UnitSettings = new UnitSettings { SignificantDigits = 5 }
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
            ProjectSettings = new ProjectSettings
            {
                ProjectName = "Load Test",
                ProjectNumber = "LT-002",
                NationalAnnex = NationalAnnex.France
            },
            UnitSettings = new UnitSettings { SignificantDigits = 8 }
        };

        try
        {
            originalSettings.SaveToJsonFile(tempFilePath);

            // Act
            ScaffoldSettings loadedSettings = JsonSerializationExtensions.LoadFromJsonFile<ScaffoldSettings>(tempFilePath);

            // Assert
            Assert.NotNull(loadedSettings);
            Assert.Equal("Load Test", loadedSettings.ProjectSettings.ProjectName);
            Assert.Equal("LT-002", loadedSettings.ProjectSettings.ProjectNumber);
            Assert.Equal(NationalAnnex.France, loadedSettings.ProjectSettings.NationalAnnex);
            Assert.Equal(8, loadedSettings.UnitSettings.SignificantDigits);
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
        string originalProjectName = instance.ProjectSettings.ProjectName;
        int originalDigits = instance.UnitSettings.SignificantDigits;

        try
        {
            // Act - modify singleton and save
            instance.ProjectSettings.ProjectName = "Singleton Test";
            instance.UnitSettings.SignificantDigits = 9;
            instance.SaveToJsonFile(tempFilePath);

            // Assert
            Assert.True(File.Exists(tempFilePath));
            string fileContent = File.ReadAllText(tempFilePath);
            Assert.Contains("Singleton Test", fileContent);
        }
        finally
        {
            // Cleanup
            instance.ProjectSettings.ProjectName = originalProjectName;
            instance.UnitSettings.SignificantDigits = originalDigits;
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
        string originalProjectName = instance.ProjectSettings.ProjectName;
        string originalProjectNumber = instance.ProjectSettings.ProjectNumber;
        int originalDigits = instance.UnitSettings.SignificantDigits;

        try
        {
            // Arrange - create a saved settings file
            var savedSettings = new ScaffoldSettings
            {
                ProjectSettings = new ProjectSettings
                {
                    ProjectName = "Loaded From File",
                    ProjectNumber = "LFF-999",
                    NationalAnnex = NationalAnnex.UnitedKingdom
                },
                UnitSettings = new UnitSettings { SignificantDigits = 10 }
            };
            savedSettings.SaveToJsonFile(tempFilePath);

            // Act
            ScaffoldSettings loaded = JsonSerializationExtensions.LoadFromJsonFile<ScaffoldSettings>(tempFilePath);
            if (loaded != null)
            {
                instance.ProjectSettings = loaded.ProjectSettings;
                instance.UnitSettings = loaded.UnitSettings;
            }

            // Assert
            Assert.Equal("Loaded From File", instance.ProjectSettings.ProjectName);
            Assert.Equal("LFF-999", instance.ProjectSettings.ProjectNumber);
            Assert.Equal(10, instance.UnitSettings.SignificantDigits);
        }
        finally
        {
            // Cleanup
            instance.ProjectSettings.ProjectName = originalProjectName;
            instance.ProjectSettings.ProjectNumber = originalProjectNumber;
            instance.UnitSettings.SignificantDigits = originalDigits;
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
            ProjectSettings = new ProjectSettings
            {
                ProjectName = "Parse Test",
                ProjectNumber = "PT-001",
                NationalAnnex = NationalAnnex.Germany
            },
            UnitSettings = new UnitSettings { SignificantDigits = 6 }
        };
        string json = ((IFormattable)original).ToString(null, null);

        // Act
        var parsed = ScaffoldSettings.Parse(json, null);

        // Assert
        Assert.NotNull(parsed);
        Assert.Equal("Parse Test", parsed.ProjectSettings.ProjectName);
        Assert.Equal("PT-001", parsed.ProjectSettings.ProjectNumber);
        Assert.Equal(NationalAnnex.Germany, parsed.ProjectSettings.NationalAnnex);
        Assert.Equal(6, parsed.UnitSettings.SignificantDigits);
    }

    [Fact]
    public void ScaffoldSettings_TryParse_SucceedsWithValidJson()
    {
        // Arrange
        var original = new ScaffoldSettings
        {
            ProjectSettings = new ProjectSettings { ProjectName = "TryParse Test" },
            UnitSettings = new UnitSettings { SignificantDigits = 5 }
        };
        string json = ((IFormattable)original).ToString(null, null);

        // Act
        bool success = ScaffoldSettings.TryParse(json, null, out ScaffoldSettings parsed);

        // Assert
        Assert.True(success);
        Assert.NotNull(parsed);
        Assert.Equal("TryParse Test", parsed.ProjectSettings.ProjectName);
        Assert.Equal(5, parsed.UnitSettings.SignificantDigits);
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
            ProjectSettings = new ProjectSettings
            {
                ProjectName = "ToString Test",
                ProjectNumber = "TT-001"
            },
            UnitSettings = new UnitSettings { SignificantDigits = 7 }
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
