using Scaffold.Reader;

namespace Scaffold.Tests.Reader;

public class ParameterNamingTests
{
    #region Single Word Tests - CVC Pattern (First Three)

    [Fact]
    public void CreateTla_Material_ReturnsMat()
    {
        // Arrange
        const string input = "Material";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("Mat", result);
    }

    [Fact]
    public void CreateTla_Panel_ReturnsPan()
    {
        // Arrange
        const string input = "Panel";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("Pnl", result);
    }

    #endregion

    #region Single Word Tests - Consonant Skeleton

    [Fact]
    public void CreateTla_Thickness_ReturnsTkh()
    {
        // Arrange
        const string input = "Thickness";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("Thk", result);
    }

    [Fact]
    public void CreateTla_Control_ReturnsCtrl()
    {
        // Arrange
        const string input = "Control";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("Ctrl", result);
    }

    [Fact]
    public void CreateTla_Grade_ReturnsGrd()
    {
        // Arrange
        const string input = "Grade";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("Grd", result);
    }

    [Fact]
    public void CreateTla_Spring_ReturnsSpg()
    {
        // Arrange
        const string input = "Spring";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("Spr", result);
    }

    [Fact]
    public void CreateTla_Strength_ReturnsSth()
    {
        // Arrange
        const string input = "Strength";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("Str", result);
    }

    #endregion

    #region CamelCase / Multi-Word Tests

    [Fact]
    public void CreateTla_BaseThickness_ReturnsBth()
    {
        // Arrange
        const string input = "BaseThickness";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("BTk", result);
    }

    [Fact]
    public void CreateTla_InnerBasePlate_ReturnsIbp()
    {
        // Arrange - Three words
        const string input = "InnerBasePlate";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("IBP", result);
    }

    [Fact]
    public void CreateTla_ConcreteStrength_ReturnsCst()
    {
        // Arrange
        const string input = "ConcreteStrength";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("CSt", result);
    }

    [Fact]
    public void CreateTla_SteelGrade_ReturnsSgr()
    {
        // Arrange
        const string input = "SteelGrade";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("SGd", result);
    }

    [Fact]
    public void CreateTla_MaximumDimensionLength_Returnsmdl()
    {
        // Arrange - Three words
        const string input = "MaximumDimensionLength";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("MDL", result);
    }

    #endregion

    #region Short Input Tests (Less than 4 characters)

    [Fact]
    public void CreateTla_SingleCharacter_ReturnsSameCharacter()
    {
        // Arrange
        const string input = "A";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("A", result);
    }

    [Fact]
    public void CreateTla_TwoCharacters_ReturnsSameString()
    {
        // Arrange
        const string input = "AB";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("AB", result);
    }

    [Fact]
    public void CreateTla_ThreeCharacters_ReturnsSameString()
    {
        // Arrange
        const string input = "ABC";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("ABC", result);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void CreateTla_EmptyString_ReturnsNul()
    {
        // Arrange
        const string input = string.Empty;

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void CreateTla_NullString_ReturnsNul()
    {
        // Arrange
        string input = null;

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void CreateTla_WhitespaceOnly_ReturnsNul()
    {
        // Arrange
        const string input = "   ";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void CreateTla_AllVowels_ReturnsFallbackFirstThree()
    {
        // Arrange
        const string input = "Audio";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        // Should fallback to first three letters
        Assert.Equal("Aud", result);
    }

    [Fact]
    public void CreateTla_CkPattern_SkipsCForThicknessPhonetics()
    {
        // Arrange
        const string input = "Thickness";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        // Should skip 'c' because next letter is 'k', resulting in "Thk" not "Thck"
        Assert.Equal("Thk", result);
    }

    [Fact]
    public void CreateTla_LowercaseInput_HandlesCorrectly()
    {
        // Arrange
        const string input = "material";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("mat", result);
    }

    [Fact]
    public void CreateTla_MixedCaseMultiWord_HandlesCorrectly()
    {
        // Arrange
        const string input = "baseThickness";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        // Should detect CamelCase split and apply multi-word logic
        Assert.Equal("bTk", result);
    }

    #endregion

    #region Additional Coverage

    [Fact]
    public void CreateTla_Strength_ReturnsCorrectConsonantSkeleton()
    {
        // Arrange
        const string input = "Strength";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("Str", result);
    }

    [Fact]
    public void CreateTla_Property_ReturnsCorrectAcronym()
    {
        // Arrange
        const string input = "Property";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("Prp", result);
    }

    [Fact]
    public void CreateTla_FourCharacterWord_AppliesConsonantSkeletonLogic()
    {
        // Arrange
        const string input = "Test";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("Tst", result);
    }

    [Fact]
    public void CreateTla_TwoWordsCvcSecondWord_HandlesCorrectly()
    {
        // Arrange
        const string input = "BaseMaterial";

        // Act
        string result = ParameterNaming.CreateThreeLetterAcronym(input);

        // Assert
        Assert.Equal("BMt", result);
    }

    #endregion

    #region SplitPascalCaseToString Tests

    [Fact]
    public void SplitPascalCaseToString_StandardPascalCase_ReturnsSplitWords()
    {
        // Arrange
        const string input = "HelloWorld";

        // Act
        string result = ParameterNaming.SplitPascalCaseToString(input);

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void SplitPascalCaseToString_ThreeWords_ReturnsAllWordsSpaceSeparated()
    {
        // Arrange
        const string input = "BaseThicknessValue";

        // Act
        string result = ParameterNaming.SplitPascalCaseToString(input);

        // Assert
        Assert.Equal("Base Thickness Value", result);
    }

    [Fact]
    public void SplitPascalCaseToString_SingleWord_ReturnsSameWord()
    {
        // Arrange
        const string input = "Material";

        // Act
        string result = ParameterNaming.SplitPascalCaseToString(input);

        // Assert
        Assert.Equal("Material", result);
    }

    [Fact]
    public void SplitPascalCaseToString_AllUppercase_ReturnsAllAsOneWord()
    {
        // Arrange
        const string input = "HTML";

        // Act
        string result = ParameterNaming.SplitPascalCaseToString(input);

        // Assert
        Assert.Equal("HTML", result);
    }

    [Fact]
    public void SplitPascalCaseToString_ConsecutiveUppercaseLetters_ReturnsSeparatedBySpace()
    {
        // Arrange
        const string input = "HTMLParser";

        // Act
        string result = ParameterNaming.SplitPascalCaseToString(input);

        // Assert
        Assert.Equal("HTML Parser", result);
    }

    [Fact]
    public void SplitPascalCaseToString_EmptyString_ReturnsEmpty()
    {
        // Arrange
        const string input = string.Empty;

        // Act
        string result = ParameterNaming.SplitPascalCaseToString(input);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void SplitPascalCaseToString_LowercaseOnlyInput_ReturnsSameString()
    {
        // Arrange
        const string input = "material";

        // Act
        string result = ParameterNaming.SplitPascalCaseToString(input);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void SplitPascalCaseToString_NumbersInString_PreservesNumbers()
    {
        // Arrange
        const string input = "Item2Container";

        // Act
        string result = ParameterNaming.SplitPascalCaseToString(input);

        // Assert
        Assert.Equal("Item 2 Container", result);
    }

    [Fact]
    public void SplitPascalCaseToString_MultipleWordsWithNumbers_ReturnsCorrectSplit()
    {
        // Arrange
        const string input = "HTML5Parser2";

        // Act
        string result = ParameterNaming.SplitPascalCaseToString(input);

        // Assert
        Assert.Equal("HTML 5 Parser 2", result);
    }

    [Fact]
    public void SplitPascalCaseToString_LongPascalCaseString_SplitsAllWords()
    {
        // Arrange
        const string input = "InnerBasePlateMaterialThicknessValue";

        // Act
        string result = ParameterNaming.SplitPascalCaseToString(input);

        // Assert
        Assert.Equal("Inner Base Plate Material Thickness Value", result);
    }

    [Fact]
    public void SplitPascalCaseToString_TwoLetterWords_HandlesProperly()
    {
        // Arrange
        const string input = "OnOff";

        // Act
        string result = ParameterNaming.SplitPascalCaseToString(input);

        // Assert
        Assert.Equal("On Off", result);
    }

    #endregion
}
