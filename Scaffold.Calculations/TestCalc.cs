using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Geometry;
using Scaffold.Core.Geometry.Abstract;
using Scaffold.Core.Images.Models;
using Scaffold.Core.Interfaces;
using SkiaSharp;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Calculations
{
    public class TestCalc : CalculationBase, IInteractiveGeometry
    {
        public new string TypeName { get; } = "Test calc";
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
        public CalcStress newStress { get; } = new CalcStress(0, PressureUnit.NewtonPerSquareMillimeter, "", "");
        [InputCalcValue]
        public CalcListOfDoubleArrays Coordinates { get; } = new("Test List", "C", [[50, 20, 10], [5, 5, 10]]);

        [OutputCalcValue]
        public CalcListOfDoubleArrays Coords { get; } = new CalcListOfDoubleArrays("Test output", "CC", [[1, 10, 100], [200, 20, 2]]);

        [OutputCalcValue]
        public CalcSIQuantity<Length> NewLength { get; private set; } = new("Calculated length", "L", new Length(0, LengthUnit.Meter));


        List<IInteractiveGeometryItem> geometry = new List<IInteractiveGeometryItem>();
        public List<IInteractiveGeometryItem> InteractiveGeometryItems => geometry;

        List<GeometryBase> _geometryBases = new List<GeometryBase>();
        public List<GeometryBase> Geometry => _geometryBases;

        public TestCalc()
        {
            InteractiveGeometryDoubleArrays start = new InteractiveGeometryDoubleArrays(Coordinates.Symbol, Coordinates.Value[0]);
            geometry.Add(start);
            InteractiveGeometryDoubleArrays end = new InteractiveGeometryDoubleArrays(Coordinates.Symbol, Coordinates.Value[1]);
            geometry.Add(end);
            InteractiveGeometryQuantityOnY forcey = new InteractiveGeometryQuantityOnY(-10, CompressiveForce);
            geometry.Add(forcey);
        }

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

            var lines = new List<Line>();
            lines.Add(new Line(new System.Numerics.Vector2((float)Coordinates.Value[0][0], (float)Coordinates.Value[0][1]),
                    new System.Numerics.Vector2((float)Coordinates.Value[1][0], (float)Coordinates.Value[1][1])));

            lines.AddRange(CreateContinuousPath(new List<(double x, double y)> { (0, 0), (0, 100), (100, 100), (100, 0), (0, 0) }));
            lines.AddRange(CreateContinuousPath(new List<(double x, double y)> {(0,0),(-20,0),(-20,CompressiveForce.Value), (0,CompressiveForce.Value),(0,0)  }));

            _geometryBases.Clear();
            _geometryBases.AddRange(lines);


        }


        /// <summary>
        /// Converts a list of coordinates into a continuous chain of Line objects.
        /// </summary>
        private static List<Line> CreateContinuousPath(List<(double x, double y)> points)
        {
            var lines = new List<Line>();

            // We need at least 2 points to make a line
            if (points == null || points.Count < 2)
                return lines;

            // Iterate up to the second-to-last point
            for (int i = 0; i < points.Count - 1; i++)
            {
                var current = points[i];
                var next = points[i + 1];

                // Convert doubles to Vector2 (which usually takes floats)
                Vector2 start = new Vector2((float)current.x, (float)current.y);
                Vector2 end = new Vector2((float)next.x, (float)next.y);

                lines.Add(new Line(start, end));
            }

            return lines;
        }
    }

}
