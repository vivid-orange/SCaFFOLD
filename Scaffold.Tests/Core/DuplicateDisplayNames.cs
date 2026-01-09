using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Images.Models;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using Scaffold.Tests.ExampleCalcsForTests;
using SkiaSharp;

namespace Scaffold.XUnitTests.Core;

[CalculationMetadata("Duplicate", "Duplicate tester")]
public class DuplicateDisplayNames : ICalculation
{
    public string CalculationName { get; set; }
    public string ReferenceName { get; set; }
    public CalcStatus Status { get; }
    [InputCalcValue] public CalcDouble LeftAssignment { get; set; }
    [InputCalcValue] public CalcDouble RightAssignment { get; set; }
    [OutputCalcValue] public CalcDouble Result { get; set; }

    public DuplicateDisplayNames()
    {
        LeftAssignment = new CalcDouble(2, "Value");
        RightAssignment = new CalcDouble(3, "Value");
        Result = new CalcDouble(Add(), "Result");
    }

    public void Calculate()
    {
        Result.Value = Add();
    }

    public IList<IFormula> GetFormulae()
    {
        var keyImage = new SKBitmap(400, 400);
        using (var canvas = new SKCanvas(keyImage))
        {
            var font = new SKFont()
            {
                Size = 25
            };

            var paint = new SKPaint()
            {
                Color = SKColors.Black
            };

            canvas.DrawText("Drawn from SKBitmap", 25, 25, SKTextAlign.Left, font, paint);
        }

        var list = new List<IFormula>
        {
            OutputItem.New("Narrative to appear above the expression")
                .WithConclusion("Some text here")
                .WithReference("Some ref here")
                .AddExpression("x &=& a + b")
                .SetImage(new ImageFromEmbeddedResource<AdditionCalculation>("ImageAsEmbeddedResource.png")),


            OutputItem.New("2. Narrative to appear above the expression")
                .WithConclusion("2. Some text here")
                .WithReference("2. Some ref here")
                .AddExpression("x &=& a + b")
                .SetImage(new ImageFromSkBitmap(keyImage)),

            OutputItem.New("Final narrative")
                .WithReference("3.a")
                .SetImage(new ImageFromRelativePath("ImageAsRelativePath.png"))
        };

        return list;
    }

    private double Add()
        => LeftAssignment.Value + RightAssignment.Value;
}

public class DuplicateDisplayNamesFluent : ICalculation, ICalculationConfiguration<DuplicateDisplayNamesFluent>
{
    public string CalculationName { get; set; }
    public string ReferenceName { get; set; }
    public CalcStatus Status { get; }
    public double LeftAssignment { get; set; } = 2;
    public double RightAssignment { get; set; } = 3;

    public void Calculate()
    {
        //
    }

    public void Configure(CalculationConfigurationBuilder<DuplicateDisplayNamesFluent> builder)
    {
        builder.Define(x => x.LeftAssignment).WithDisplayName("Value").AsInput();
        builder.Define(x => x.RightAssignment).WithDisplayName("Value").AsInput();
    }

    public IList<IFormula> GetFormulae() => new List<IFormula>();
}
