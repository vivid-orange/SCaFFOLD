using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Images.Models;
using Scaffold.Core.Interfaces;
using SkiaSharp;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Calculations
{
    public class TestCalc : CalculationBase
    {
        public new string TypeName { get;  } = "Test calc";
        public override string InstanceName { get; set; } = "This is my test calc";

        [InputCalcValue]
        public CalcSIQuantity<Force> CompressiveForce { get; } = new("Compression", "F_c", new Force(20, ForceUnit.Kilonewton));

        [InputCalcValue]
        public CalcSIQuantity<Length> ColumnLength { get; } = new("Column Length", "L", new Length(3, LengthUnit.Meter));

        [InputCalcValue]
        public CalcSIQuantity<Length> ColumnBaseplate { get; } = new("Column baseplate thickness", "BT", new Length(30, LengthUnit.Millimeter));

        [OutputCalcValue]
        public CalcSIQuantity<Pressure> CompressiveStress { get; private set; } = new("Compressive stress", @"\sigma_c", new Pressure(20, PressureUnit.NewtonPerSquareMillimeter));

        [InputCalcValue]
        public ISteelProfile SteelProfile { get; set; } = new SteelProfile();

        [InputCalcValue]
        public ListOfDoubleArrays Coordinates { get; } = new("Test List", "C", [[50, 20, 10], [5, 5, 10]]);

        [OutputCalcValue]
        public ListOfDoubleArrays Coords { get; } = new ListOfDoubleArrays("Test output", "CC", [[1, 10, 100], [200, 20, 2]]);

        [OutputCalcValue]
        public CalcSIQuantity<Length> NewLength { get; private set; } = new("Calculated length", "L", new Length(0, LengthUnit.Meter));

        public override List<IOutputItem> GetFormulae()
        {
            var returnList = new List<IOutputItem>();

            var outputs = new OutputItem("reffy", "This one goes first", "Done", new TextItem("We can explain a bit about the formula here. There is no longer a separate 'Narrative' property."));
            outputs.Expressions.Add(new LatexItem(@"M = \frac{wl^2} {8}"));
            outputs.Expressions.Add(new TextItem("and then a bit more text whcih can now be in-line", true));
            outputs.Expressions.Add(new TextItem("and then an image"));
            outputs.Expressions.Add(new ImageOutputItem(new ImageFromSkBitmap(Utilities.CreateMultiCircleImage(Coordinates.Value, SKColors.Orange)), true));
            outputs.Expressions.Add(new TextItem("and then another formula", true));
            outputs.Expressions.Add(new LatexItem(@"E = mc^2"));
            outputs.Expressions.Add(new TextItem("all of which can be set to in-line or new line"));

            returnList.Add(outputs);

            return returnList;
        }

        public override void Calculate()
        {
            CompressiveStress.Quantity = CompressiveForce.Quantity / SteelProfile.Area.Quantity;

            NewLength.Quantity = ColumnLength.Quantity + ColumnBaseplate.Quantity;

            CalcForce newForce = new CalcForce(10, "testforce", "F");
            CalcArea newArea = new CalcArea(100, AreaUnit.SquareMeter, "area", "A");

        }

    }
}
