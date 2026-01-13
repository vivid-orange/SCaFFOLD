// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Interfaces;
using UnitsNet;

namespace Scaffold.Calculations.Eurocode.Concrete.PunchingShear
{
    public class BetaFactorCalculation : CalculationBase
    {
        [InputCalcValue]
        public CalcSelectionList _colType { get; } = new CalcSelectionList("Column condition", 0, new List<string> { "INTERNAL", "EDGE", "CORNER", "RE-ENTRANT" });
        [InputCalcValue]
        public CalcSIQuantity<Torque> YMoment { get; } = new CalcSIQuantity<Torque>("Moment about Y axis", "M_y", new Torque(100, UnitsNet.Units.TorqueUnit.KilonewtonMeter));
        [InputCalcValue]
        public CalcSIQuantity<Torque> ZMoment { get; } = new CalcSIQuantity<Torque>("Moment about Z axis", "M_z", new Torque(20, UnitsNet.Units.TorqueUnit.KilonewtonMeter));
        [InputCalcValue]
        public CalcSIQuantity<Force> PunchingLoad { get; } = new CalcSIQuantity<Force>("Punching Shear Load", "P_z", new Force(300, UnitsNet.Units.ForceUnit.Kilonewton));
        [InputCalcValue]
        public CalcSIQuantity<Length> by { get; } = new CalcSIQuantity<Length>("by", "b_y", new Length(500, UnitsNet.Units.LengthUnit.Millimeter));
        [InputCalcValue]
        public CalcSIQuantity<Length> bz { get; } = new CalcSIQuantity<Length>("bz", "b_z", new Length(500, UnitsNet.Units.LengthUnit.Millimeter));
        [OutputCalcValue]
        public CalcSIQuantity<Ratio> BetaFactor { get; } = new CalcSIQuantity<Ratio>("Beta Facor", @"\beta", new Ratio(1, UnitsNet.Units.RatioUnit.DecimalFraction));


        public override void Calculate()
        {
            List<OutputItem> returnList = new List<OutputItem>();
            OutputItem betaFormula = new OutputItem("cl 6.4.3", "", "", new TextItem("Calculate Beta factor.\"")); //{ Narrative = "Calculate Beta factor." + Environment.NewLine, Ref = "cl 6.4.3" };

            switch (_colType.Value)
            {
                case "INTERNAL":
                    Length ey = YMoment.Quantity / PunchingLoad.Quantity;
                    Length ez = ZMoment.Quantity / PunchingLoad.Quantity;
                    //double by = (controlPerimeterNoHoles.Segments.Max(a => a.Start.X) - controlPerimeterNoHoles.Segments.Min(a => a.Start.X));
                    //double bz = (controlPerimeterNoHoles.Segments.Max(a => a.Start.Y) - controlPerimeterNoHoles.Segments.Min(a => a.Start.Y));
                    double term1 = Math.Pow(ey / bz.Quantity, 2);
                    double term2 = Math.Pow(ez / by.Quantity, 2);
                    BetaFactor.Value = 1 + 1.8 * Math.Sqrt(term1 + term2);
                    //betaFormula.Narrative += "Calculated based on a rectangular internal column with loading eccentric to both axes. Control perimeter dimensions as Fig 6.13.";
                    //betaFormula.Expression.Add(_beta.Symbol + @"=1 + 1.8\sqrt{\left(\frac{e_y}{b_z}\right)^2+\left(\frac{e_z}{b_y}\right)^2} =" + Math.Round(_beta.Value, 3));
                    //betaFormula.Expression.Add(@"e_y =\frac{" + _my.Symbol + @"}{" + _punchingLoad.Symbol + "}=" + Math.Round(ey, 1) + "mm");
                    //betaFormula.Expression.Add(@"e_z =\frac{" + _mz.Symbol + @"}{" + _punchingLoad.Symbol + "}=" + Math.Round(ez, 1) + "mm");
                    //betaFormula.Expression.Add(@"b_y =" + Math.Round(by, 1) + "mm");
                    //betaFormula.Expression.Add(@"b_z =" + Math.Round(bz, 1) + "mm");
                    //betaFormula.Image = _fig6_13;
                    break;

                case "EDGE":
                    double epar = Math.Abs((YMoment.Quantity / PunchingLoad.Quantity).Value);
                    double c1 = _columnAdim.Value;
                    double c2 = _columnBdim.Value;
                    double k = calck(c1 / c2);
                    double w1 = Math.Pow(c2, 2) / 4 + c1 * c2 + 4 * c1 * d_average + 8 * Math.Pow(d_average, 2) + Math.PI * d_average * c2;
                    var u1noHoles = controlPerimeterNoHoles.Length;
                    if (u1noHoles > u1LimitedPerimeterTR64 && (_columnAdim.Value > 3 * d_average || _columnBdim.Value > 3 * d_average))
                    {
                        betaFormula.Narrative += " Perimeter u1 for beta calc limited in accordance with TR64 clause 4.3.2.";
                        u1noHoles = u1LimitedPerimeterTR64;
                    }
                    var u1red = u1reducedNoHoles.Sum(a => a.Length);
                    _beta.Value = (u1noHoles / u1red) + k * (u1noHoles / w1) * epar;
                    betaFormula.Narrative += "Calculated on the basis of eccentricities about both axes, but moment about the axis parallel to slab edge is towards the interior of the slab.";
                    betaFormula.Expression.Add(_beta.Symbol + @"=\frac{u_1}{u_{1^*}}+k\frac{u_1}{W_1}e_{par}=" + Math.Round(_beta.Value, 3));
                    betaFormula.Expression.Add(@"u_{1,no holes limited}=" + Math.Round(u1noHoles, 2) + "mm");
                    betaFormula.Expression.Add(@"u_{1^*,no holes}=" + Math.Round(u1red, 2) + "mm");
                    betaFormula.Expression.Add(@"k=" + Math.Round(k, 2));
                    betaFormula.Expression.Add(@"e_{par} =\left|\frac{" + _my.Symbol + @"}{" + _punchingLoad.Symbol + @"}\right|=" + Math.Round(epar, 1) + "mm");
                    betaFormula.Expression.Add(@"W_1=\frac{c_2^2}{4}+c_1c_2+4c_1d+8d^2+\pi dc_2=" + Math.Round(w1, 2));
                    betaFormula.Image = _fig6_20;
                    break;
                case "CORNER":
                    u1noHoles = controlPerimeterNoHoles.Length;
                    if (u1noHoles > u1LimitedPerimeterTR64 && (_columnAdim.Value > 3 * d_average || _columnBdim.Value > 3 * d_average))
                    {
                        betaFormula.Narrative += " Perimeter u1 for beta calc limited in accordance with TR64 clause 4.3.2.";
                        u1noHoles = u1LimitedPerimeterTR64;
                    }
                    u1red = u1reducedNoHoles.Sum(a => a.Length);
                    _beta.Value = u1noHoles / u1red;
                    betaFormula.Expression.Add(_beta.Symbol + @"=\frac{u_{1,no holes}}{u_{1^*,no holes}}=" + Math.Round(_beta.Value, 3));
                    betaFormula.Expression.Add(@"u_{1 no holes limited}=" + Math.Round(u1noHoles, 2) + "mm");
                    betaFormula.Expression.Add(@"u_{1^*,no holes}=" + Math.Round(u1red, 2) + "mm");
                    betaFormula.Image = _fig6_20;
                    break;
                case "RE-ENTRANT":
                    _beta.Value = 1.275;
                    betaFormula.Narrative += "Default value for beta - use with care.";
                    betaFormula.Expression.Add(_beta.Symbol + @"=" + Math.Round(_beta.Value, 3));
                    break;
                default:
                    break;
            }
        }


        public override List<IOutputItem> GetFormulae() => null;

        private double calck(double c1overc2)
        {
            if (c1overc2 <= 0.5) return 0.45;
            else if (c1overc2 < 1) return (c1overc2 - 0.5) * (0.15 / 0.5) + 0.45;
            else if (c1overc2 < 3) return (c1overc2 - 1) * (0.2 / 2) + 0.6;
            else return 0.8;
        }
    }
}






//            if (_betaCheck.ValueAsString == "MANUAL")
//            {
//                _beta.Value = _betaProposed.Value;
//                betaFormula = new Formula()
//                {
//                    Narrative = "Manally entered beta factor",
//                    Expression = new List<string>
//                    {
//                        _beta.Symbol + "=" + _betaProposed.Value
//                    }
//                };
//            }
//            else if (_betaCheck.ValueAsString == "CODE_DEFAULT")
//            {
//                if (_colType.ValueAsString == "INTERNAL")
//                    _beta.Value = 1.15;
//                else if (_colType.ValueAsString == "CORNER")
//                    _beta.Value = 1.5;
//                else if (_colType.ValueAsString == "EDGE")
//                    _beta.Value = 1.4;
//                else
//                    _beta.Value = 1.275;
//                betaFormula = new Formula()
//                {
//                    Narrative = "Use code defaults for beta factor. May be used where stability does not rely on" +
//                    "frame action between column and slab and adjacent spans differ by no more than 25%.",
//                    Ref = "cl 6.4.3(6)",
//                    Expression = new List<string>
//                    {
//                        _beta.Symbol + "=" + _beta.ValueAsString
//                    }
//                };
//                if (_colType.ValueAsString == "RE-ENTRANT")
//                {
//                    betaFormula.Narrative = "Value for re-entrant column is interpolated from" +
//                        " internal and edge column. Use with care.";
//                }
//            }
//        }
//        public override List<IOutputItem> GetFormulae() => null;
//    }
//}
