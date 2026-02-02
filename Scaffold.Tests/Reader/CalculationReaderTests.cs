using FluentAssertions;
using Scaffold.Reader;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.Reader;

public class CalculationReaderTests
{
    #region Test Fixtures

    private class SimpleCalc : Calculation
    {
        public override string CalculationTitle => "Simple";

        [InputParameter("H", "Height")]
        public Length Height { get; set; } = new Length(5, LengthUnit.Meter);

        [OutputParameter("A", "Area")]
        public Area Area { get; private set; } = new Area(0, AreaUnit.SquareMeter);

        public override void Calculate()
        {
            Area = Height * Height;
        }
    }

    private class AutoDiscoveryCalc : Calculation
    {
        public override string CalculationTitle => "Auto";

        // Public setter -> auto-discovered as input
        public Length ColumnHeight { get; set; } = new Length(3, LengthUnit.Meter);

        // Private setter -> auto-discovered as output
        public Force ShearForce { get; private set; } = new Force(0, ForceUnit.Kilonewton);
    }

    private class MixedAttributeCalc : Calculation
    {
        public override string CalculationTitle => "Mixed";

        [InputParameter("B", "Breadth")]
        public Length Breadth { get; set; } = new Length(200, LengthUnit.Millimeter);

        // No attribute, public setter -> auto input
        public Length Depth { get; set; } = new Length(400, LengthUnit.Millimeter);

        [OutputParameter("A", "Area")]
        public Area Area { get; private set; }
    }

    private class IgnoredPropertyCalc : Calculation
    {
        public override string CalculationTitle => "Ignored";

        [InputParameter("H", "Height")]
        public Length Height { get; set; } = new Length(5, LengthUnit.Meter);

        [CalcIgnore]
        public string SomeInternalThing => "ignored";

        [CalcIgnore]
        public double AnotherIgnored { get; set; } = 42;
    }

    private class PlainObject
    {
        [InputParameter("X", "X Value")]
        public Length XValue { get; set; } = new Length(10, LengthUnit.Meter);

        [OutputParameter("Y", "Y Value")]
        public Length YValue { get; private set; } = new Length(20, LengthUnit.Meter);

        // No attribute -> not picked up for plain objects
        public double Ignored { get; set; } = 99;
    }

    private class NoAttributeObject
    {
        public double Value1 { get; set; } = 1.0;
        public double Value2 { get; set; } = 2.0;
    }

    #endregion

    #region ICalculation - Explicit Attributes

    [Fact]
    public void GetInputs_WithExplicitAttributes_ReturnsInputs()
    {
        // Arrange
        var calc = new SimpleCalc();

        // Act
        var inputs = CalculationReader.GetInputs(calc);

        // Assert
        inputs.Should().HaveCount(1);
        inputs[0].Symbol.Should().Be("H");
        inputs[0].EntityLabel.Should().Be("Height");
    }

    [Fact]
    public void GetOutputs_WithExplicitAttributes_ReturnsOutputs()
    {
        // Arrange
        var calc = new SimpleCalc();

        // Act
        var outputs = CalculationReader.GetOutputs(calc);

        // Assert
        outputs.Should().HaveCount(1);
        outputs[0].Symbol.Should().Be("A");
        outputs[0].EntityLabel.Should().Be("Area");
    }

    #endregion

    #region ICalculation - Auto Discovery

    [Fact]
    public void GetInputs_AutoDiscovery_PublicSetterIsInput()
    {
        // Arrange
        var calc = new AutoDiscoveryCalc();

        // Act
        var inputs = CalculationReader.GetInputs(calc);

        // Assert
        inputs.Should().HaveCount(1);
        inputs[0].EntityLabel.Should().Be("Column height");
    }

    [Fact]
    public void GetOutputs_AutoDiscovery_PrivateSetterIsOutput()
    {
        // Arrange
        var calc = new AutoDiscoveryCalc();

        // Act
        var outputs = CalculationReader.GetOutputs(calc);

        // Assert
        outputs.Should().HaveCount(1);
        outputs[0].EntityLabel.Should().Be("Shear force");
    }

    [Fact]
    public void GetInputs_MixedAttributes_ReturnsExplicitAndAutoDiscovered()
    {
        // Arrange
        var calc = new MixedAttributeCalc();

        // Act
        var inputs = CalculationReader.GetInputs(calc);

        // Assert
        inputs.Should().HaveCount(2);
        inputs.Should().Contain(v => v.Symbol == "B" && v.EntityLabel == "Breadth");
        inputs.Should().Contain(v => v.EntityLabel == "Depth");
    }

    #endregion

    #region CalcIgnore

    [Fact]
    public void GetInputs_CalcIgnore_SkipsIgnoredProperties()
    {
        // Arrange
        var calc = new IgnoredPropertyCalc();

        // Act
        var inputs = CalculationReader.GetInputs(calc);
        var outputs = CalculationReader.GetOutputs(calc);

        // Assert
        inputs.Should().HaveCount(1);
        inputs[0].Symbol.Should().Be("H");
        outputs.Should().BeEmpty();
    }

    #endregion

    #region Plain Objects (non-ICalculation)

    [Fact]
    public void GetInputs_PlainObject_ReadsExplicitAttributesOnly()
    {
        // Arrange
        var obj = new PlainObject();

        // Act
        var inputs = CalculationReader.GetInputs(obj);

        // Assert
        inputs.Should().HaveCount(1);
        inputs[0].Symbol.Should().Be("X");
    }

    [Fact]
    public void GetOutputs_PlainObject_ReadsExplicitAttributesOnly()
    {
        // Arrange
        var obj = new PlainObject();

        // Act
        var outputs = CalculationReader.GetOutputs(obj);

        // Assert
        outputs.Should().HaveCount(1);
        outputs[0].Symbol.Should().Be("Y");
    }

    [Fact]
    public void GetInputs_PlainObjectNoAttributes_ReturnsEmpty()
    {
        // Arrange
        var obj = new NoAttributeObject();

        // Act
        var inputs = CalculationReader.GetInputs(obj);

        // Assert
        inputs.Should().BeEmpty();
    }

    #endregion

    #region Null Handling

    [Fact]
    public void GetInputs_Null_ReturnsEmptyList()
    {
        // Act
        var inputs = CalculationReader.GetInputs((object)null);

        // Assert
        inputs.Should().BeEmpty();
    }

    [Fact]
    public void GetOutputs_Null_ReturnsEmptyList()
    {
        // Act
        var outputs = CalculationReader.GetOutputs((object)null);

        // Assert
        outputs.Should().BeEmpty();
    }

    #endregion

    #region Caching

    [Fact]
    public void GetInputs_SameInstance_ReturnsSameList()
    {
        // Arrange
        var calc = new SimpleCalc();

        // Act
        var first = CalculationReader.GetInputs(calc);
        var second = CalculationReader.GetInputs(calc);

        // Assert
        first.Should().BeSameAs(second);
    }

    [Fact]
    public void GetInputs_DifferentInstances_ReturnDifferentLists()
    {
        // Arrange
        var calc1 = new SimpleCalc();
        var calc2 = new SimpleCalc();

        // Act
        var first = CalculationReader.GetInputs(calc1);
        var second = CalculationReader.GetInputs(calc2);

        // Assert
        first.Should().NotBeSameAs(second);
    }

    #endregion

    #region Value Reading and Writing

    [Fact]
    public void GetInputs_ValueReflectsInstance()
    {
        // Arrange
        var calc = new SimpleCalc { Height = new Length(10, LengthUnit.Meter) };

        // Act
        var inputs = CalculationReader.GetInputs(calc);

        // Assert
        inputs[0].ToString().Should().Contain("10");
    }

    [Fact]
    public void GetInputs_WritingValueUpdatesInstance()
    {
        // Arrange
        var calc = new SimpleCalc();
        var inputs = CalculationReader.GetInputs(calc);

        // Act
        inputs[0].TryParse("10");

        // Assert
        calc.Height.Value.Should().Be(10);
    }

    #endregion

    #region Core Properties Excluded

    [Fact]
    public void GetInputs_CoreProperties_AreExcluded()
    {
        // Arrange
        var calc = new SimpleCalc();

        // Act
        var inputs = CalculationReader.GetInputs(calc);
        var outputs = CalculationReader.GetOutputs(calc);
        var all = inputs.Concat(outputs).ToList();

        // Assert - CalculationTitle, Status, Symbol, EntityLabel should not appear
        all.Should().NotContain(v => v.EntityLabel == "Calculation title");
        all.Should().NotContain(v => v.EntityLabel == "Status");
    }

    #endregion

    #region GetValueKind

    [Fact]
    public void GetValueKind_Calculation_ReturnsCalculation()
    {
        CalculationReader.GetValueKind(typeof(SimpleCalc)).Should().Be(CalcValueKind.Calculation);
    }

    [Fact]
    public void GetValueKind_List_ReturnsCollection()
    {
        CalculationReader.GetValueKind(typeof(List<int>)).Should().Be(CalcValueKind.Collection);
    }

    [Fact]
    public void GetValueKind_String_ReturnsStandard()
    {
        CalculationReader.GetValueKind(typeof(string)).Should().Be(CalcValueKind.Standard);
    }

    [Fact]
    public void GetValueKind_Primitive_ReturnsStandard()
    {
        CalculationReader.GetValueKind(typeof(double)).Should().Be(CalcValueKind.Standard);
    }

    [Fact]
    public void GetValueKind_Length_ReturnsStandard()
    {
        CalculationReader.GetValueKind(typeof(Length)).Should().Be(CalcValueKind.Standard);
    }

    [Fact]
    public void GetValueKind_ObjectWithCalcParameterAttributes_ReturnsComplex()
    {
        CalculationReader.GetValueKind(typeof(PlainObject)).Should().Be(CalcValueKind.Complex);
    }

    #endregion
}
