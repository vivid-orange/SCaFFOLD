using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Images.Models;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using SkiaSharp;

namespace Scaffold.Tests.ExampleCalcsForTests;

[CalculationMetadata("Add values", "Core library tester")]
public class AdditionCalculationFluent : ICalculation, ICalculationConfiguration<AdditionCalculationFluent>
{
    public string ReferenceName { get; set; }
    public string CalculationName { get; set; }
    public CalcStatus Status { get; }
    public CalcDouble LeftAssignment { get; set; }
    public CalcDouble RightAssignment { get; set; }
    public CalcDouble Result { get; set; }

    public AdditionCalculationFluent()
    {
        LeftAssignment = new CalcDouble(2, "Left assignment");
        RightAssignment = new CalcDouble(3);
        Result = new CalcDouble(Add());
    }

    public void Calculate()
    {
        Result.Value = Add();
    }

    public void Configure(CalculationConfigurationBuilder<AdditionCalculationFluent> builder)
    {
        builder
            .Define(x => new { x.LeftAssignment, x.RightAssignment })
            .AsInput();

        builder.Define(x => x.Result)
            .WithDisplayName("Result")
            .AsOutput();
    }

    public IList<IFormula> GetFormulae()
    {
        var keyImage = new SKBitmap(400, 400);
        using (var canvas = new SKCanvas(keyImage))
        {

            //var paintText = new SKPaint { TextSize = 25, TextAlign = SKTextAlign.Left, Color = SKColors.Black };
            //canvas.DrawText("Drawn from SKBitmap", 25, 25, paintText);

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

    private double Add() => LeftAssignment + RightAssignment;
}
