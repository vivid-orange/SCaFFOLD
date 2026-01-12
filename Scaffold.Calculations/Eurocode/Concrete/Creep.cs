using System;
using System.Collections.Generic;
using VividOrange.Taxonomy.Materials.StandardMaterials.En;
using VividOrange.Taxonomy.Profiles;
using VividOrange.Taxonomy.Sections.SectionProperties;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Calculations.Eurocode.Concrete;
public class Creep : ICalculation
{
    public string ReferenceName { get; set; }
    public string CalculationName { get; set; } = "Concrete Creep";
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [InputCalcValue("Grd", "Grade")]
    public CalcSelectionList ConcreteGrade { get; set; }
        = new CalcSelectionList("Concrete Grade", "C30/37", EnumSelectionListParser.ConcreteGrades);

    [InputCalcValue]
    public CalcQuantityWrapper<RelativeHumidity> RelativeHumidity { get; set; } = new CalcQuantityWrapper<RelativeHumidity>(
        new RelativeHumidity(70, RelativeHumidityUnit.Percent), "Relative humidity", "RH");

    [InputCalcValue]
    public CalcQuantityWrapper<Duration> Time0 { get; set; } = new CalcQuantityWrapper<Duration>(
        new Duration(28, DurationUnit.Day), @"Time load applied", @"t_0\");

    [InputCalcValue]
    public CalcQuantityWrapper<Duration> Time { get; set; } = new CalcQuantityWrapper<Duration>(
        new Duration(10000000, DurationUnit.Day), "Time", "t");

    [InputCalcValue]
    public CalcLength Length { get; set; } = new CalcLength(500, LengthUnit.Millimeter, "Length", "L");

    [InputCalcValue]
    public CalcLength Width { get; set; } = new CalcLength(500, LengthUnit.Millimeter, "Width", "W");

    [OutputCalcValue("Cross section area", "A_c")]
    public CalcArea Area { get; private set; }

    [OutputCalcValue("Section perimeter", "u")]
    public CalcLength Perimeter { get; private set; }

    [OutputCalcValue("Notional Creep Coefficient", @"\varphi(t,t_0)")]
    public CalcDouble NotionalCreepCoefficient { get; private set; }

    [OutputCalcValue("Coefficient for creep with time", @"\beta(t,t_0)")]
    public CalcDouble CreepTimeCoefficient { get; private set; }

    [OutputCalcValue("Creep coefficient", @"\varphi_0")]
    public CalcDouble CreepCoefficient { get; private set; }

    public List<IFormula> Expressions = new List<IFormula>();
    public IList<IFormula> GetFormulae() => Expressions;

    public Creep()
    {
        Calculate();
    }

    public void Calculate()
    {
        Expressions = new List<IFormula>();
        CalcStress fcm = new ConcreteMaterialProperties(ConcreteGrade.GetEnum<EnConcreteGrade>("/", "_")).fcm;
        IProfile profile = new Rectangle(Width, Length);
        var sectionProperties = new SectionProperties(profile);
        Area = new CalcArea(sectionProperties.Area.ToUnit(AreaUnit.SquareMillimeter), string.Empty);
        Perimeter = new CalcLength(sectionProperties.Perimeter.ToUnit(LengthUnit.Millimeter), string.Empty);
        CalcLength h0 = 2 * Area / Perimeter;

        double factorRH = 0;
        double betafcm = 0;
        double betat0 = 0;
        double betaH = 0;
        double alpha1 = Math.Pow(35 / fcm, 0.7);
        double alpha2 = Math.Pow(35 / fcm, 0.2);
        double alpha3 = Math.Pow(35 / fcm, 0.5);
        //expressions.Add(
        //    Formula.FormulaWithNarrative("Calculate alpha values")
        //    .AddExpression(@"\alpha_1=\left[ \frac{35}{f_{cm}} \right]^{0.7}" + Math.Round(alpha1, 2))
        //    .AddExpression(@"\alpha_2=\left[ \frac{35}{f_{cm}} \right]^{0.2}" + Math.Round(alpha2, 2))
        //    .AddExpression(@"\alpha_3=\left[ \frac{35}{f_{cm}} \right]^{0.5}" + Math.Round(alpha3, 2))
        //    .AddRef("B.8c")
        //    );

        if (fcm <= 35)
        {
            factorRH = 1 + (1 - RelativeHumidity.Value / 100) / (0.1 * Math.Pow(h0, 1d / 3d));
            //expressions.Add(
            //    Formula.FormulaWithNarrative("Calculate factor to allow for effect of relative humidity")
            //    .AddRef("B.3a")
            //    .AddExpression(@"f_{cm}\leq35 \Rightarrow")
            //    .AddExpression(@"\phi_{RH}=1+\frac{1-RH/100}{0.1\sqrt[3]{h_0}}")
            //    );
        }
        else
        {
            factorRH = (1 + (1 - RelativeHumidity.Value / 100) / (0.1 * Math.Pow(h0, (double)(1d / 3d))) * alpha1) * alpha2;
            //expressions.Add(
            //    Formula.FormulaWithNarrative("Calculate factor to allow for effect of relative humidity")
            //    .AddRef("B.3b")
            //    .AddExpression(@"f_{cm}>35 \Rightarrow")
            //    .AddExpression(@"\phi_{RH}=\left[ 1+\frac{1-RH/100}{0.1\sqrt[3]{h_0}}\alpha_1 \right]\alpha_2")
            //    );
        }

        betafcm = 16.8 / Math.Sqrt(fcm);
        //expressions.Add(
        //    Formula.FormulaWithNarrative("")
        //    .AddExpression(@"\beta(f_{cm})=\frac{16.8}{\sqrt{f_{cm}}}=" + Math.Round(betafcm, 2))
        //    .AddRef("B.4")
        //    );

        betat0 = 1 / (0.1 + Math.Pow(Time0, 0.20));
        //expressions.Add(
        //    Formula.FormulaWithNarrative("Factor to allow for effect of concrete" +
        //    "strength on the notional creep coefficient")
        //    .AddExpression(@"\beta(f_{cm})")
        //    );

        if (fcm <= 35)
        {
            betaH = Math.Min(1.5 * (1 + Math.Pow(0.012 * RelativeHumidity, 18)) * h0 + 250, 1500);
            //expressions.Add(
            //    Formula.FormulaWithNarrative("Calculate coefficient depending on relative humidity and notional member size.")
            //    .AddExpression(meanCompStr.Symbol + @"\leq 35 \Rightarrow")
            //    .AddExpression(@"\beta_H = 1.5\left[ 1 + (0.012 RH)^{18} \right]h_0 + 250 \leq 1500")
            //    .AddRef("B.8a")
            //    );
        }
        else
        {
            betaH = Math.Min(1.5 * (1 + Math.Pow(0.012 * RelativeHumidity, 18)) * h0 + 250 * alpha3, 1500 * alpha3);
            //expressions.Add(
            //    Formula.FormulaWithNarrative("Calculate coefficient depending on relative humidity and notional member size.")
            //    .AddExpression(meanCompStr.Symbol + @"> 35 \Rightarrow")
            //    .AddExpression(@"\beta_H = 1.5\left[ 1 + (0.012 RH)^{18} \right]h_0 + 250\alpha_3 \leq 1500\alpha_3")
            //    .AddRef("B.8b")
            //    );
        }

        NotionalCreepCoefficient = factorRH * betafcm * betat0;

        CreepTimeCoefficient = Math.Pow((Time - Time0) / (betaH + Time - Time0), 0.3);

        CreepCoefficient = NotionalCreepCoefficient * CreepTimeCoefficient;
    }
}
