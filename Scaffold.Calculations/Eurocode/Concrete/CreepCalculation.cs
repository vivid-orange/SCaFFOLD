using System;
using System.Collections.Generic;
using Scaffold.Core.Attributes;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using UnitsNet;
using UnitsNet.Units;
using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Sections.SectionProperties;

namespace Scaffold.Calculations.Eurocode.Concrete;

public class CreepCalculation : ICalculation
{
    public string ReferenceName { get; set; }
    public string CalculationName { get; set; } = "Concrete Creep";
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [InputCalcValue("CMP", "Concrete Material Property")]
    public ConcreteMaterialProperties Concrete { get; set; } = new();

    [InputCalcValue("RH", "Relative humidity")]
    public RelativeHumidity RelativeHumidity { get; set; } = new(70, RelativeHumidityUnit.Percent);

    [InputCalcValue(@"t_0\", @"Time load applied")]
    public Duration Time0 { get; set; } = new(28, DurationUnit.Day);

    [InputCalcValue("t", "Time")]
    public Duration Time { get; set; } = new(50, DurationUnit.JulianYear);

    [InputCalcValue("L", "Length")]
    public Length Length { get; set; } = new(500, LengthUnit.Millimeter);

    [InputCalcValue("W", "Width")]
    public Length Width { get; set; } = new(500, LengthUnit.Millimeter);

    [OutputCalcValue("A_c", "Cross section area")]
    public Area Area { get; private set; }

    [OutputCalcValue("u", "Section perimeter")]
    public Length Perimeter { get; private set; }

    [OutputCalcValue(@"\varphi(t,t_0)", "Notional Creep Coefficient")]
    public double NotionalCreepCoefficient { get; private set; }

    [OutputCalcValue(@"\beta(t,t_0)", "Coefficient for creep with time")]
    public double CreepTimeCoefficient { get; private set; }

    [OutputCalcValue(@"\varphi_0", "Creep coefficient")]
    public double CreepCoefficient { get; private set; }

    public List<IFormula> Expressions = new List<IFormula>();
    public IList<IFormula> GetFormulae() => Expressions;

    public CreepCalculation()
    {
        Calculate();
    }

    public void Calculate()
    {
        Expressions = new List<IFormula>();
        Pressure fcm = Concrete.fcm;
        IProfile profile = new Rectangle(Width, Length);
        var sectionProperties = new SectionProperties(profile);
        Area = sectionProperties.Area;
        Perimeter = sectionProperties.Perimeter;
        Length h0 = 2 * Area / Perimeter;

        double factorRH = 0;
        double betafcm = 0;
        double betat0 = 0;
        double betaH = 0;
        double alpha1 = Math.Pow(35 / fcm.Megapascals, 0.7);
        double alpha2 = Math.Pow(35 / fcm.Megapascals, 0.2);
        double alpha3 = Math.Pow(35 / fcm.Megapascals, 0.5);
        //expressions.Add(
        //    Formula.FormulaWithNarrative("Calculate alpha values")
        //    .AddExpression(@"\alpha_1=\left[ \frac{35}{f_{cm}} \right]^{0.7}" + Math.Round(alpha1, 2))
        //    .AddExpression(@"\alpha_2=\left[ \frac{35}{f_{cm}} \right]^{0.2}" + Math.Round(alpha2, 2))
        //    .AddExpression(@"\alpha_3=\left[ \frac{35}{f_{cm}} \right]^{0.5}" + Math.Round(alpha3, 2))
        //    .AddRef("B.8c")
        //    );

        if (fcm.Megapascals <= 35)
        {
            factorRH = 1 + (1 - RelativeHumidity.Value / 100) / (0.1 * Math.Pow(h0.Millimeters, 1d / 3d));
            //expressions.Add(
            //    Formula.FormulaWithNarrative("Calculate factor to allow for effect of relative humidity")
            //    .AddRef("B.3a")
            //    .AddExpression(@"f_{cm}\leq35 \Rightarrow")
            //    .AddExpression(@"\phi_{RH}=1+\frac{1-RH/100}{0.1\sqrt[3]{h_0}}")
            //    );
        }
        else
        {
            factorRH = (1 + (1 - RelativeHumidity.Value / 100) / (0.1 * Math.Pow(h0.Millimeters, (double)(1d / 3d))) * alpha1) * alpha2;
            //expressions.Add(
            //    Formula.FormulaWithNarrative("Calculate factor to allow for effect of relative humidity")
            //    .AddRef("B.3b")
            //    .AddExpression(@"f_{cm}>35 \Rightarrow")
            //    .AddExpression(@"\phi_{RH}=\left[ 1+\frac{1-RH/100}{0.1\sqrt[3]{h_0}}\alpha_1 \right]\alpha_2")
            //    );
        }

        betafcm = 16.8 / Math.Sqrt(fcm.Megapascals);
        //expressions.Add(
        //    Formula.FormulaWithNarrative("")
        //    .AddExpression(@"\beta(f_{cm})=\frac{16.8}{\sqrt{f_{cm}}}=" + Math.Round(betafcm, 2))
        //    .AddRef("B.4")
        //    );

        betat0 = 1 / (0.1 + Math.Pow(Time0.Days, 0.20));
        //expressions.Add(
        //    Formula.FormulaWithNarrative("Factor to allow for effect of concrete" +
        //    "strength on the notional creep coefficient")
        //    .AddExpression(@"\beta(f_{cm})")
        //    );

        if (fcm.Megapascals <= 35)
        {
            betaH = Math.Min(1.5 * (1 + Math.Pow(0.012 * RelativeHumidity.Value, 18)) * h0.Millimeters + 250, 1500);
            //expressions.Add(
            //    Formula.FormulaWithNarrative("Calculate coefficient depending on relative humidity and notional member size.")
            //    .AddExpression(meanCompStr.Symbol + @"\leq 35 \Rightarrow")
            //    .AddExpression(@"\beta_H = 1.5\left[ 1 + (0.012 RH)^{18} \right]h_0 + 250 \leq 1500")
            //    .AddRef("B.8a")
            //    );
        }
        else
        {
            betaH = Math.Min(1.5 * (1 + Math.Pow(0.012 * RelativeHumidity.Value, 18)) * h0.Millimeters + 250 * alpha3, 1500 * alpha3);
            //expressions.Add(
            //    Formula.FormulaWithNarrative("Calculate coefficient depending on relative humidity and notional member size.")
            //    .AddExpression(meanCompStr.Symbol + @"> 35 \Rightarrow")
            //    .AddExpression(@"\beta_H = 1.5\left[ 1 + (0.012 RH)^{18} \right]h_0 + 250\alpha_3 \leq 1500\alpha_3")
            //    .AddRef("B.8b")
            //    );
        }

        NotionalCreepCoefficient = factorRH * betafcm * betat0;

        CreepTimeCoefficient = Math.Pow((Time.Days - Time0.Days) / (betaH + Time.Days - Time0.Days), 0.3);

        CreepCoefficient = NotionalCreepCoefficient * CreepTimeCoefficient;
    }
}
