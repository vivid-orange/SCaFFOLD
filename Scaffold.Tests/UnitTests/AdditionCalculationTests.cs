using FluentAssertions;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Interfaces;
using Scaffold.Tests.ExampleCalcsForTests;
using Scaffold.XUnitTests.Core;

namespace Scaffold.Tests.UnitTests;

public class AdditionCalculationTests
{
    private CalculationReader Reader { get; } = new();
    private const string TypeName = "Add values";
    private const string Title = "Core library tester";

    [Fact]
    public void UnreadCalculation_Ok()
    {
        var calc = new AdditionCalculation();
        calc.InstanceName.Should().BeNull();
        calc.ReferenceName.Should().BeNull();
    }

    [Fact]
    public void Default_Read_Ok()
    {
        var calc = new AdditionCalculation();

        Core.Models.CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        calc.InstanceName.Should().Be(TypeName);
        calc.ReferenceName.Should().Be(Title);

        metadata.Type.Should().Be(TypeName);
        metadata.Title.Should().Be(Title);

        inputs.Count.Should().Be(2);
        outputs.Count.Should().Be(1);

        inputs[0].TypeName.Should().Be("Left assignment");
        inputs[1].TypeName.Should().Be("Right Assignment",
            because: "Fallback to property name, class did not set DisplayName");
        outputs[0].TypeName.Should().Be("Result");
    }

    [Fact]
    public void Fluent_Read_Ok()
    {
        var calc = new AdditionCalculationFluent();

        Core.Models.CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        metadata.Type.Should().Be(TypeName);
        metadata.Title.Should().Be(Title);

        inputs.Count.Should().Be(2);
        outputs.Count.Should().Be(1);

        inputs[0].TypeName.Should().Be("Left assignment");
        inputs[1].TypeName.Should().Be("Right Assignment",
            because: "Fallback to property name, class did not set DisplayName");
        outputs[0].TypeName.Should().Be("Result");
    }

    [Fact]
    public void Fluent_DisplayNameSingle_Ok()
    {
        var calc = new FluentDisplayNameSingle();
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        outputs[0].TypeName.Should().Be("Result");
    }

    [Fact]
    public void Fluent_DisplayNameMultiple_Ok()
    {
        var calc = new FluentDisplayNameMultiple();
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        outputs[0].TypeName.Should().Be("Result (1)");
        outputs[1].TypeName.Should().Be("Result (2)");

        outputs[0].Symbol.Should().Be("=");
        outputs[1].Symbol.Should().Be("=");
    }

    [Fact]
    public void Fluent_InvokedByActivator_Ok()
    {
        object activated = Activator.CreateInstance(typeof(FluentDisplayNameMultiple));
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs((ICalculation)activated);

        outputs[0].TypeName.Should().Be("Result (1)");
        outputs[1].TypeName.Should().Be("Result (2)");

        outputs[0].Symbol.Should().Be("=");
        outputs[1].Symbol.Should().Be("=");
    }

    [Fact]
    public void FluentPrimitives_Ok()
    {
        var calc = new AdditionCalculationFluentPrimitives();

        //
        // Initial values
        //
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        inputs.Count.Should().Be(2);
        outputs.Count.Should().Be(1);

        inputs[0].TypeName.Should().Be("Left assignment");
        inputs[1].TypeName.Should().Be("Right assignment");

        outputs[0].TypeName.Should().Be("Result");

        inputs[0].GetValueAsString().Should().Be("2");
        inputs[1].GetValueAsString().Should().Be("3");

        outputs[0].GetValueAsString().Should().Be("5");
        calc.Result.Should().Be(5);

        //
        // Updated values (checking that InternalCalcValue works, pairing with true primitive value)
        //
        inputs[0].TryParse("6");

        Reader.Update(calc);
        outputs[0].GetValueAsString().Should().Be("9");
        calc.Result.Should().Be(9);
    }

    [Fact]
    public void FluentPrimitivesJoined_Ok()
    {
        var calc = new AdditionCalculationFluentPrimitivesJoined();

        //
        // Initial values
        //
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        inputs.Count.Should().Be(2);
        outputs.Count.Should().Be(1);

        inputs[0].TypeName.Should().Be("Value (1)");
        inputs[1].TypeName.Should().Be("Value (2)");

        outputs[0].TypeName.Should().Be("Result");

        inputs[0].GetValueAsString().Should().Be("2");
        inputs[1].GetValueAsString().Should().Be("3");

        outputs[0].GetValueAsString().Should().Be("5");
        calc.Result.Should().Be(5);

        //
        // Updated values (checking that InternalCalcValue works, pairing with true primitive value)
        //
        inputs[0].TryParse("6");

        Reader.Update(calc);
        outputs[0].GetValueAsString().Should().Be("9");
        calc.Result.Should().Be(9);
    }

    [Fact]
    public void AttributePrimitives_Ok()
    {
        var calc = new AdditionCalculationAttributePrimitives();

        //
        // Initial values
        //
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        inputs.Count.Should().Be(2);
        outputs.Count.Should().Be(1);

        inputs[0].TypeName.Should().Be("Left Assignment");
        inputs[0].Symbol.Should().Be("L");

        inputs[1].TypeName.Should().Be("Right Assignment");
        inputs[1].Symbol.Should().Be("R");

        outputs[0].TypeName.Should().Be("Result");
        outputs[0].Symbol.Should().Be("=");

        inputs[0].GetValueAsString().Should().Be("2");
        inputs[1].GetValueAsString().Should().Be("3");

        outputs[0].GetValueAsString().Should().Be("5");
        calc.Result.Should().Be(5);

        //
        // Updated values (checking that InternalCalcValue works, pairing with true primitive value)
        //
        inputs[0].TryParse("6");

        Reader.Update(calc);
        outputs[0].GetValueAsString().Should().Be("9");
        calc.Result.Should().Be(9);
    }

    [Fact]
    public void Updated_FromUI_Ok()
    {
        var calc = new AdditionCalculation { LeftAssignment = { Value = 5 }, RightAssignment = { Value = 4 } };

        calc.Result.Value.Should().Be(5, because: "result has not changed yet through the update method.");

        Reader.Update(calc);

        calc.Result.Value.Should().Be(9);

        var formulae = Reader.GetFormulae(calc).ToList();
        formulae.Count.Should().Be(3);
        formulae[0].Expression.Count.Should().Be(1);
        formulae[0].Expression[0].Should().Be("x &=& a + b");
    }

    [Fact]
    public void DuplicateDisplayNames_Ok()
    {
        var calc = new DuplicateDisplayNames();
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        inputs[0].TypeName.Should().Be("Value");
        inputs[1].TypeName.Should().Be("Value (1)");
    }

    [Fact]
    public void DuplicateDisplayNames_Fluent_Ok()
    {
        var calc = new DuplicateDisplayNamesFluent();
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        inputs[0].TypeName.Should().Be("Value");
        inputs[1].TypeName.Should().Be("Value (1)");
    }
}
