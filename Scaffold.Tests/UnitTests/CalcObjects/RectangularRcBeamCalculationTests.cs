using FluentAssertions;
using Scaffold.Core;
using Scaffold.Core.Abstract;
using Scaffold.Core.CalcObjects.Profiles;
using Scaffold.Core.CalcValues;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Tests.UnitTests;

public class RectangularRcBeamCalculationTests
{
    private CalculationReader Reader { get; } = new();
    private const string TypeName = "Rectangular RC beam calculation";
    private const string Title = "Rectangular Rc Beam Calculation";

    [Fact]
    public void UnreadCalculation_Ok()
    {
        var calc = new RectangularRcBeamCalculation();
        calc.CalculationName.Should().BeNull();
        calc.ReferenceName.Should().BeNull();
    }

    [Fact]
    public void Default_Read_Ok()
    {
        var calc = new RectangularRcBeamCalculation();

        Core.Models.CalculationMetadata metadata = Reader.GetMetadata(calc);
        IReadOnlyList<ICalcValue> inputs = Reader.GetInputs(calc);
        IReadOnlyList<ICalcValue> outputs = Reader.GetOutputs(calc);

        calc.CalculationName.Should().Be(TypeName);
        calc.ReferenceName.Should().BeNull();

        metadata.Type.Should().Be(TypeName);
        metadata.Title.Should().BeNull();

        inputs.Count.Should().Be(24);
        outputs.Count.Should().Be(18);

        inputs[0].DisplayName.Should().Be("Profile");

        calc.Calculate();

        calc.rebarAsReqd.Value.Should().BeApproximately(327.6, 1.0);
    }

    [Fact]
    public void Updated_FromUI_Ok()
    {
        var calc = new RectangularRcBeamCalculation();
        calc.Calculate();
        var test = new CalcRectangularProfile(new Length(800, LengthUnit.Millimeter),
            new Length(500, LengthUnit.Millimeter), "", "");
        string x = test.GetValue();

        calc.Profile = CalcRectangularProfile.Parse(x, null);

        calc.rebarAsReqd.Value.Should().BeApproximately(327.6, 1.0, because: "result has not changed yet through the update method.");

        Reader.Update(calc);

        calc.rebarAsReqd.Value.Should().BeApproximately(551.5, 1.0, because: "result has not changed yet through the update method.");

        var formulae = Reader.GetFormulae(calc).ToList();
        formulae.Count.Should().Be(5);
    }
}
