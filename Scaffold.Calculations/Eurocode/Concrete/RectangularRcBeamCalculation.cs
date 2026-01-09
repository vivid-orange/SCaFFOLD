using System;
using System.Collections.Generic;
using MagmaWorks.Taxonomy.Profiles;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Profiles;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Core;

[CalculationMetadata("Rectangular RC beam calculation")]
public class RectangularRcBeamCalculation : ICalculation
{
    public string DisplayName { get; set; }
    public string CalculationName { get; set; }
    public CalcStatus Status { get; }

    [InputCalcValue] public CalcRectangularProfile Profile { get; set; }

    [InputCalcValue] public CalcDouble aggregateAdjustmentFactor { get; set; }

    [InputCalcValue] public CalcSelectionList Grade { get; set; }

    [InputCalcValue] public CalcDouble ultStrain { get; set; }

    [InputCalcValue] public CalcDouble shortStrain { get; set; }

    [InputCalcValue] public CalcDouble effCompZoneHtFac { get; set; }

    [InputCalcValue] public CalcDouble effStrFac { get; set; }

    [InputCalcValue] public CalcDouble k1 { get; set; }

    [InputCalcValue] public CalcDouble k3 { get; set; }

    [InputCalcValue] public CalcDouble delta { get; set; }

    [InputCalcValue] public CalcDouble partialFacConc { get; set; }

    [InputCalcValue] public CalcDouble compStrCoeff { get; set; }

    [InputCalcValue] public CalcDouble compStrCoeffw { get; set; }

    [InputCalcValue] public CalcDouble hAgg { get; set; }

    [InputCalcValue] public CalcDouble monolithicSimpleSupportFactor { get; set; }

    [InputCalcValue] public CalcDouble rebarCharYieldStr { get; set; }

    [InputCalcValue] public CalcDouble rebarPartialFactor { get; set; }

    [InputCalcValue] public CalcDouble TopCover { get; set; }

    [InputCalcValue] public CalcDouble BottomCover { get; set; }

    [InputCalcValue] public CalcDouble SideCover { get; set; }

    [InputCalcValue] public CalcDouble FireResistance { get; set; }

    [InputCalcValue] public CalcDouble sidesExposed { get; set; }

    [InputCalcValue] public CalcDouble minWidth { get; set; }


    [InputCalcValue] public CalcDouble bendingMom { get; }

    [OutputCalcValue] public CalcDouble charCompStr { get; }

    [OutputCalcValue] public CalcDouble meanCompStr { get; }

    [OutputCalcValue] public CalcDouble meanAxialTenStr { get; }

    [OutputCalcValue] public CalcDouble secModElasticicity { get; }

    [OutputCalcValue] public CalcDouble k2 { get; }

    [OutputCalcValue] public CalcDouble k4 { get; }

    [OutputCalcValue] public CalcDouble desCompStrConc { get; }

    [OutputCalcValue] public CalcDouble desCompStrConcw { get; }

    [OutputCalcValue] public CalcDouble rebarDesYieldStr { get; }

    [OutputCalcValue] public CalcDouble effDepth { get; }

    [OutputCalcValue] public CalcDouble redistRatio { get; }

    [OutputCalcValue] public CalcDouble redistRatio2 { get; }

    [OutputCalcValue] public CalcDouble leverArm { get; }

    [OutputCalcValue] public CalcDouble naDepth { get; }

    [OutputCalcValue] public CalcDouble rebarAsReqd { get; }

    [OutputCalcValue] public CalcDouble rebarAsProv { get; }

    [OutputCalcValue] public CalcDouble rebarMinArea { get; }

    [OutputCalcValue] public CalcDouble rebarMaxArea { get; }

    List<IOutputItem> Expressions { get; set; }

    public RectangularRcBeamCalculation()
    {
        Profile = new CalcRectangularProfile(new Length(500, LengthUnit.Millimeter),
            new Length(800, LengthUnit.Millimeter), "", "");

        bendingMom = new CalcDouble(100, "Bending moment", "M", "kNm");
        aggregateAdjustmentFactor = new CalcDouble(1, "Aggregate adjustment factor", "AAF", "");
        Grade = new CalcSelectionList("Concrete grade", "C40/50",
            new List<string>() { @"C30/40", @"C35/42", @"C40/50" });
        ultStrain = new CalcDouble(0.0035, "Ultimate strain", @"\epsilon_{cu2}", "");
        shortStrain = new CalcDouble(0.0035, "Shortening strain", @"\epsilon_{cu3}", "");
        effCompZoneHtFac = new CalcDouble(0.8, "Effective compression zone height factor", @"\lambda", "");
        effStrFac = new CalcDouble(1.0, "Effective strength factor", @"\eta", "");
        k1 = new CalcDouble(0.4, "Coefficient k1", "k_1", "");
        k3 = new CalcDouble(0.4, "Coefficient k3", "k_3", "");
        delta = new CalcDouble(0.85, "Redistribution factor", @"\delta", "");
        partialFacConc = new CalcDouble(1.5, "Partial factor for concrete", @"\gamma_c", "");
        compStrCoeff = new CalcDouble(0.85, "Compressive strength coefficient", @"\alpha_{cc}", "");
        compStrCoeffw = new CalcDouble(1.0, "Compressive strength coefficient", @"\alpha_{cw}", "");
        hAgg = new CalcDouble(20, "Maximum aggregate size", "h_{agg}", "mm");
        monolithicSimpleSupportFactor = new CalcDouble(0.25, "Monolithic simple support moment factor", @"\beta_1", "");
        rebarCharYieldStr =
            new CalcDouble(500, "Characteristic yield strength for reinforcing steel", "f_yk", "N/mm²");
        rebarPartialFactor = new CalcDouble(1.15, "Partial factor for reinforcing steel", @"\gamma_s", "");
        TopCover = new CalcDouble(35, "Top cover", "c_{top}", "mm");
        BottomCover = new CalcDouble(35, "Bottom cover", "c_{btm}", "mm");
        SideCover = new CalcDouble(35, "Side cover", "c_{sides}", "mm");
        FireResistance = new CalcDouble(60, "Fire resistance", "R", "mins");
        sidesExposed = new CalcDouble(3, "Sides exposed", "", "No.");
        minWidth = new CalcDouble(120, "Minimum width", "b_{min}", "mm");

        charCompStr = new CalcDouble(35, "Characteristic compressive cylinder strength", "f_{ck}", "N/mm²");
        meanCompStr = new CalcDouble(0, "Mean compressive strength", "f_{cm}", "N/mm²");
        meanAxialTenStr = new CalcDouble(0.3, "Mean axial tensile strength", "f_{ctm}", "N/mm²");
        secModElasticicity = new CalcDouble(22, "Secant modulus of elasticity of concrete", "E_{cm}", "kN/mm²");
        k2 = new CalcDouble(0, "Coefficient k2", "k_2", "");
        k4 = new CalcDouble(0, "Coefficient k4", "k_4", "");
        desCompStrConc = new CalcDouble(0, "Design compressive strength", "f_{cd}", "N/mm²");
        desCompStrConcw = new CalcDouble(0, "Strength of concrete cracked in shear", "f_{cwd}", "N/mm²");
        rebarDesYieldStr = new CalcDouble(0, "Design yield strength for reinforcing steel", "f_{yd}", "N/mm²");
        effDepth = new CalcDouble(0, "Effective depth", "d", "mm");
        redistRatio = new CalcDouble(0, "Redistribution ratio", "K", "");
        redistRatio2 = new CalcDouble(0, "Redistribution ratio limit", "K'", "");
        leverArm = new CalcDouble(0, "Lever arm", "z", "mm");
        naDepth = new CalcDouble(0, "Neutral axis depth", "x", "mm");
        rebarAsReqd = new CalcDouble(0, "Tension reinforcement required", "A_{s,reqd}", "mm²");
        rebarAsProv = new CalcDouble(0, "Tension reinforcement provided", "A_{s,prov}", "mm²");
        rebarMinArea = new CalcDouble(0, "Minimum tension reinforcement", "A_{s,min}", "mm²");
        rebarMaxArea = new CalcDouble(0, "Maximum tension reinforcement", "A_{s,max}", "mm²");
    }

    public void Calculate()
    {
        if (Profile is not IRectangle)
        {
            throw new ArgumentException("Calculation only supports rectangular profiles");
        }

        Expressions = new List<IOutputItem>
        {
            new Formula()
            {
                Narrative = "Beam calculations to BS EN 1992-1-1 2004. Currently in beta so check thoroughly!",
            }
        };
        List<string> expression = new();

        charCompStr.Value = getConcreteStrength(Grade.Value);
        meanCompStr.Value = charCompStr.Value + 8;
        expression.Add(string.Format("{0}={1}+8={2}{3}", meanCompStr.Symbol, charCompStr.Symbol, meanCompStr.Value,
            meanCompStr.Unit));

        meanAxialTenStr.Value = 0.3 * Math.Pow(charCompStr.Value, 2f / 3f);
        expression.Add(string.Format(@"{0}=0.3\times {1}^{{2/3}}={2}{3}", meanAxialTenStr.Symbol, charCompStr.Symbol,
            Math.Round(meanAxialTenStr.Value, 2), meanAxialTenStr.Unit));

        secModElasticicity.Value = 22 * Math.Pow(meanCompStr.Value / 10f, 0.3) * aggregateAdjustmentFactor.Value;
        expression.Add(string.Format(@"{0}=22\times \left(\frac{{{1}}}{{10}}\right)^{{0.3}}={2}{3}",
            secModElasticicity.Symbol, meanCompStr.Symbol, Math.Round(secModElasticicity.Value, 3),
            secModElasticicity.Unit));
        k2.Value = 0.6 + 0.0014 / ultStrain.Value;
        k4.Value = 0.6 + 0.0014 / ultStrain.Value;
        desCompStrConc.Value = compStrCoeff.Value * charCompStr.Value / partialFacConc.Value;
        desCompStrConcw.Value = compStrCoeffw.Value * charCompStr.Value / partialFacConc.Value;
        rebarDesYieldStr.Value = rebarCharYieldStr.Value / rebarPartialFactor.Value;

        Expressions.Add(new Formula() { Expressions = expression, Reference = "Property calcs", Narrative = "" });
        expression = new List<string>();

        // Lever arm
        effDepth.Value = Profile.Height.Value - BottomCover.Value - 10 - 16;

        redistRatio.Value = bendingMom.Value * 1000000 /
                            (Profile.Width.Value * Math.Pow(effDepth.Value, 2) * charCompStr.Value);
        //redistRatio2.Value = (2 * effStrFac.Value * compStrCoeff.Value/partialFacConc.Value) * (1 - effCompZoneHtFac.Value * (1 - k1.Value)/(2*k2.Value)) * (effCompZoneHtFac.Value * (1-k1.Value)/(2*k2.Value));
        redistRatio2.Value = effStrFac.Value * compStrCoeff.Value / (partialFacConc.Value * k2.Value) *
                             effCompZoneHtFac.Value * (delta.Value - k1.Value) *
                             (1 - effCompZoneHtFac.Value * (delta.Value - k1.Value) / (2 * k2.Value));

        expression.Add(effDepth.Symbol + @" = h - c - \phi_{link} - \phi_{bar}/2 = " + Profile.Height.Value + " - " +
                       BottomCover.Value + " - 10 - 32/2 = " + effDepth.Value + " mm");
        expression.Add(redistRatio.Symbol + @" = \frac{ " + bendingMom.Symbol + "}{" + Profile.Width.Unit +
                       effDepth.Symbol + "²f_{ck}} = " + Math.Round(redistRatio.Value, 3));
        expression.Add(redistRatio2.Symbol + @" = " + effStrFac.Symbol + @" \frac{" + compStrCoeff.Symbol + "}{" +
                       partialFacConc.Symbol + k2.Symbol + "}" + effCompZoneHtFac.Symbol
                       + @"\frac{" + delta.Symbol + "- " + k1.Symbol + "}{" + k2.Symbol + @"}\left(1 -" +
                       effCompZoneHtFac.Symbol + @"\frac{" + delta.Symbol + " - " + k1.Symbol + "}{2" + k2.Symbol +
                       @"}\right) = " + Math.Round(redistRatio2.Value, 3));

        if (redistRatio.Value < redistRatio2.Value)
        {
            double val0 = 0.5 * effDepth.Value *
                          (1 + Math.Pow(
                              (1 - 2 * redistRatio.Value /
                                  (effStrFac.Value * compStrCoeff.Value / partialFacConc.Value)), 0.5));
            double val1 = 0.95 * effDepth.Value;
            leverArm.Value = Math.Min(val0, val1);
            naDepth.Value = 2 * (effDepth.Value - leverArm.Value) / effCompZoneHtFac.Value;
            rebarAsReqd.Value = bendingMom.Value * 1000000 / (rebarDesYieldStr.Value * leverArm.Value);

            expression.Add(redistRatio.Symbol + @" \leq " + redistRatio2.Symbol);
            expression.Add(leverArm.Symbol +
                           @" = min\left[\frac{d}{2}\left(1+\sqrt{1-\frac{2K}{\eta\alpha_{cc}/\gamma_c}}\right),0.95d\right] = min(" +
                           Math.Round(val0) + "," + Math.Round(val1) + ") = " + Math.Round(leverArm.Value) + " mm");
            expression.Add(naDepth.Symbol + @" = \frac{2(" + effDepth.Symbol + "-" + leverArm.Symbol + ")}{" +
                           effCompZoneHtFac.Symbol + "} = " + Math.Round(naDepth.Value) + " mm");
            expression.Add(string.Format(@"{0}=\frac{{{1}}}{{{2}{3}}}={4}{5}", rebarAsReqd.Symbol, bendingMom.Symbol,
                rebarDesYieldStr.Symbol, leverArm.Symbol, Math.Round(rebarAsReqd.Value, 0), rebarAsReqd.Unit));
        }
        else
        {
            double val0 = 0.5 * effDepth.Value *
                          (1 + Math.Pow(
                              (1 - 2 * redistRatio2.Value /
                                  (effStrFac.Value * compStrCoeff.Value / partialFacConc.Value)), 0.5));
            double val1 = 0.95 * effDepth.Value;
            leverArm.Value = Math.Min(val0, val1);
            naDepth.Value = 2 * (effDepth.Value - leverArm.Value) / effCompZoneHtFac.Value;

            double Mp = Profile.Width.Value * 1e-3 * Math.Pow(effDepth.Value * 1e-3, 2) * charCompStr.Value * 1e3 *
                        (redistRatio.Value - redistRatio2.Value);
            double d2 = TopCover.Value + 10 + 8;
            rebarAsReqd.Value = (redistRatio2.Value * charCompStr.Value * 1e3 * Profile.Width.Value * 1e-3 *
                                 Math.Pow(effDepth.Value * 1e-3, 2) / (rebarDesYieldStr.Value * leverArm.Value)
                                 + Mp / (rebarDesYieldStr.Value * (leverArm.Value - d2))) * 1e6;


            expression.Add(redistRatio.Symbol + @" > " + redistRatio2.Symbol);
            expression.Add(leverArm.Symbol +
                           @" = min\left[\frac{d}{2}\left(1+\sqrt{1-\frac{2K}{\eta\alpha_{cc}/\gamma_c}}\right),0.95d\right] = min(" +
                           Math.Round(val0) + "," + Math.Round(val1) + ") = " + Math.Round(leverArm.Value) + " mm");
            expression.Add(naDepth.Symbol + @" = \frac{2(" + effDepth.Symbol + "-" + leverArm.Symbol + ")}{" +
                           effCompZoneHtFac.Symbol + "} = " + Math.Round(naDepth.Value) + " mm");
            expression.Add("M' = " + Profile.Width.Unit + effDepth.Symbol + "²" + charCompStr.Symbol + "(" +
                           redistRatio.Symbol + " - " + redistRatio2.Symbol + ") = " + Math.Round(Mp, 1) + "kNm");
            expression.Add("d_2 = " + TopCover.Symbol + @"\phi_{link} + \phi_{bar/2} = " + d2 + "mm");
            expression.Add(rebarAsReqd.Symbol + @"= \frac{" + redistRatio2.Symbol + charCompStr.Symbol +
                           Profile.Width.Unit + effDepth.Symbol + "²}{" + rebarDesYieldStr.Symbol + leverArm.Symbol +
                           "} +"
                           + @"\frac{M'}{" + rebarDesYieldStr.Symbol + "(" + effDepth.Symbol + " - d2)} = " +
                           Math.Round(rebarAsReqd.Value) + "mm²");
        }

        Tuple<double, double> bottomBars = calcBarSizeAndDia(rebarAsReqd.Value);
        rebarAsProv.Value = bottomBars.Item1 * Math.PI * Math.Pow(bottomBars.Item2 / 2, 2);
        if (double.IsNaN(rebarAsProv.Value))
            rebarAsProv.Status = CalcStatus.Fail;
        else
            rebarAsProv.Status = CalcStatus.None;
        expression.Add(string.Format("{0}={1}{2}", rebarAsProv.Symbol, Math.Round(rebarAsProv.Value, 0),
            rebarAsProv.Unit));
        Expressions.Add(new Formula()
        {
            Expressions = expression,
            Reference = "cl. 6.1",
            Narrative = "Calculates the tension reinforcement required to resist bending.",
            Conclusion = string.Format("{0}No. H{1} bars provided", bottomBars.Item1, bottomBars.Item2),
            Status = rebarAsProv.Status
        });

        // Check As_prov exceeds As_min
        expression = new List<string>();
        rebarMinArea.Value = Math.Max(0.26 * meanAxialTenStr.Value / rebarCharYieldStr.Value, 0.0013) *
                             Profile.Width.Value * effDepth.Value;
        expression.Add(string.Format(@"{0}=max(0.26\frac{{{1}}}{{{2}}}, 0.0013)\times {3}\times {4}={5}{6}",
            rebarMinArea.Symbol,
            meanAxialTenStr.Symbol,
            rebarCharYieldStr.Symbol,
            Profile.Width.Unit,
            effDepth.Symbol,
            Math.Round(rebarMinArea.Value, 0),
            rebarMinArea.Unit));
        if (rebarAsProv.Value >= rebarMinArea.Value)
        {
            expression.Add(string.Format(@"{0}>={1}", rebarAsProv.Symbol, rebarMinArea.Symbol));
            Expressions.Add(new Formula()
            {
                Expressions = expression,
                Reference = "equ. 9.1N",
                Narrative = "Check that provided reinforcement exceeds minimum",
                Conclusion = "OK",
                Status = CalcStatus.Pass
            });
        }
        else
        {
            expression.Add(string.Format(@"{0}<{1}", rebarAsProv.Symbol, rebarMinArea.Symbol));
            Expressions.Add(new Formula()
            {
                Expressions = expression,
                Reference = "equ. 9.1N",
                Narrative = "Check that provided reinforcement exceeds minimum",
                Conclusion = "Too little reinforcing steel",
                Status = CalcStatus.Fail
            });
        }

        // Check As_prov is less than As_max
        expression = new List<string>();
        rebarMaxArea.Value = 0.04 * Profile.Width.Value * Profile.Height.Value;
        expression.Add(string.Format(@"{0}=0.04\times {1}\times {2}={3}{4}",
            rebarMaxArea.Symbol,
            Profile.Width.Unit,
            Profile.Height.Unit,
            Math.Round(rebarMaxArea.Value, 0),
            rebarMaxArea.Unit));
        if (rebarAsProv.Value <= rebarMaxArea.Value)
        {
            expression.Add(string.Format(@"{0}<={1}", rebarAsProv.Symbol, rebarMaxArea.Symbol));
            Expressions.Add(new Formula()
            {
                Expressions = expression,
                Reference = "cl. 9.2.1(3)",
                Narrative = "Check that provided reinforcement is less than maximum",
                Conclusion = "OK",
                Status = CalcStatus.Pass
            });
        }
        else
        {
            expression.Add(string.Format(@"{0}>{1}", rebarAsProv.Symbol, rebarMaxArea.Symbol));
            Expressions.Add(new Formula()
            {
                Expressions = expression,
                Reference = "cl. 9.2.1.1(3)",
                Narrative = "Check that provided reinforcement is less than maximum",
                Conclusion = "Too much reinforcing steel",
                Status = CalcStatus.Fail
            });
        }
    }

    public List<IOutputItem> GetFormulae()
    {
        return Expressions;
    }

    private static Tuple<double, double> calcBarSizeAndDia(double area)
    {
        int maxBars = 5;
        List<int> barSizes = new()
        {
            10,
            12,
            16,
            20,
            25,
            32,
            40
        };
        foreach (int item in barSizes)
        {
            for (int i = 2; i < maxBars; i++)
            {
                if (i * Math.PI * Math.Pow(item / 2, 2) > area)
                {
                    return new Tuple<double, double>(i, item);
                }
            }
        }

        return new Tuple<double, double>(double.NaN, double.NaN);
    }

    private static double getConcreteStrength(string grade)
    {
        var concGradeToStrength =
            new Dictionary<string, double> { { @"C30/40", 30 }, { @"C35/42", 35 }, { @"C40/50", 40 } };
        return concGradeToStrength[grade];
    }

    public List<ICalcValue> GetInputs() => throw new NotImplementedException();
    public List<ICalcValue> GetOutputs() => throw new NotImplementedException();
}
