using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Images.Models;
using Scaffold.Core.Interfaces;
using UnitsNet;

namespace Scaffold.Calculations
{
    public class SteelProfile : CalculationBase, ISteelProfile
    {
        public new string DisplayName { get; set; } = "Steel profile";

        public override string TypeName => "Built up steel I beam profile";

        [InputCalcValue]
        public SIQuantity<Length> Breadth { get; set; } = new("Breadth", "B", new Length(150, UnitsNet.Units.LengthUnit.Millimeter));
        [InputCalcValue]
        public SIQuantity<Length> FlangeThickness { get; set; } = new("Flange thickness", "T", new Length(30, UnitsNet.Units.LengthUnit.Millimeter));
        [InputCalcValue]
        public SIQuantity<Length> Height { get; set; } = new("Height", "H", new Length(300, UnitsNet.Units.LengthUnit.Millimeter));
        [InputCalcValue]
        public SIQuantity<Length> WebThickness { get; set; } = new("Web thickness", "t", new Length(10, UnitsNet.Units.LengthUnit.Millimeter));
        [InputCalcValue]
        public SIQuantity<Length> RootRadius { get; } = new("Root radius", "r", new Length(10, UnitsNet.Units.LengthUnit.Millimeter));

        [InputCalcValue]
        public SteelGrade SteelGradeMember { get; } = new SteelGrade();
        [InputCalcValue]
        public SteelGrade SteelGradeBaseplate { get; } = new SteelGrade();

        [OutputCalcValue]
        public SIQuantity<Area> Area { get; } = new SIQuantity<Area>("Area", "A", new Area(0, UnitsNet.Units.AreaUnit.SquareMillimeter));
        [OutputCalcValue]
        public SIQuantity<Force> CompressionResistance { get; } = new SIQuantity<Force>("Compression Resistance", "P", new Force(0, UnitsNet.Units.ForceUnit.Kilonewton));


        public string Symbol { get => ""; }

        public SteelProfile()
        {
            Calculate();
        }



        public override List<IOutputItem> GetFormulae()
        {
            var returnList = new List<IOutputItem>();

            var outputs = new OutputItem("cl 1.A", "", "OK", new TextItem("Beam profile:"));
            outputs.Expressions.Add(new ImageOutputItem(new ImageFromSkBitmap(Utilities.CreateDetailedISectionBitmap(Height.Value, Breadth.Value, FlangeThickness.Value, WebThickness.Value, RootRadius.Value, SkiaSharp.SKColors.Gray))));
            outputs.Expressions.Add(new LatexItem(@"A = 2(BT) + t(H - 2T)"));
            outputs.Expressions.Add(new LatexItem(@"A = " + Area.Value));
            outputs.Expressions.Add(new TextItem("root radius neglected because i couldn't be arsed"));


            returnList.Add(outputs);

            return returnList;
        }

        public override void Calculate()
        {
            Area.Quantity = (2.0 * Breadth.Quantity * FlangeThickness.Quantity + WebThickness.Quantity * (Height.Quantity - 2 * FlangeThickness.Quantity)).ToUnit(UnitsNet.Units.AreaUnit.SquareMillimeter);
            CompressionResistance.Quantity = (Area.Quantity * SteelGradeMember.Gradestrength.Quantity).ToUnit(UnitsNet.Units.ForceUnit.Kilonewton);
        }

        public bool TryParse(string strValue)
        {
            return false;
        }

        public string GetValueAsString()
        {
            return "FB " + Height.Value + " x " + Breadth.Value;
        }
    }
}
