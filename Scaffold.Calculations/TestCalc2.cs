using System.Numerics;
using Scaffold.Core;
using Scaffold.Geometry;
using Scaffold.Reader.Images;
using Scaffold.Report;
using SkiaSharp;

namespace Scaffold.Calculations
{
    public class TestCalc2 : ICalculation, IInteractiveGeometry
    {

        public string EntityLabel { get; } = "Test calc";
        public string CalculationTitle { get; set; } = "This is my test calc";


        [CalcParameter(CalcParameterType.Input, "I", "Multiplier")]
        public double Multiplier { get; set; }

        [CalcParameter(CalcParameterType.Input, "M", "Moment")]
        public Torque Moment { get; set; } = new Torque(20, TorqueUnit.KilonewtonMeter);

        [CalcParameter(CalcParameterType.Input, "B", "Breadth", ["Geometry", "Section"])]
        public Length Breadth { get; set; } = new Length(200, LengthUnit.Millimeter);

        [CalcParameter(CalcParameterType.Input, "C_x", "Centre X", ["Geometry", "Centre"])]
        public Length Offset1 { get; set; } = new Length(5, LengthUnit.Millimeter);

        [CalcParameter(CalcParameterType.Input, "C_y", "Centre Y", ["Geometry", "Centre"])]
        public Length Offset2 { get; set; } = new Length(5, LengthUnit.Millimeter);

        [CalcParameter(CalcParameterType.Input, "E", "Column height", ["Misc"])]
        public EmbeddedCalc ReducedHeight { get; set; } = new EmbeddedCalc();

        [CalcParameter(CalcParameterType.Input, "H", "Height", ["Geometry", "Section"])]
        public Length Height { get; set; } = new Length(500, LengthUnit.Millimeter);

        [CalcParameter(CalcParameterType.Input, "T", "Flange", ["Geometry", "Section"])]
        public Length FlangeThickness { get; set; } = new Length(25, LengthUnit.Millimeter);

        [CalcParameter(CalcParameterType.Input, "t", "Web", ["Geometry", "Section"])]
        public Length WebThickness { get; set; } = new Length(12, LengthUnit.Millimeter);

        [CalcParameter(CalcParameterType.Input, "r", "Root radius", ["Geometry", "Section"])]
        public Length RootRadius { get; set; } = new Length(5, LengthUnit.Millimeter);


        [CalcParameter(CalcParameterType.Output, "M_o", "Moment out")]
        public Torque MomentOut { get; private set; } = new Torque(0, TorqueUnit.KilonewtonMeter);


        [CalcParameter(CalcParameterType.Output, "F_req", "Force required")]
        public Force ForceRequired { get; private set; } = new Force(0, ForceUnit.Kilonewton);

        [CalcParameter(CalcParameterType.Input, "C", "Complex Input type", ["Misc"])]
        public MyDataHolder ComplexValue { get; set; } = new MyDataHolder();

        [CalcParameter(CalcParameterType.Input, "L", "List of things", ["Misc"])]
        public List<MyOtherDataHolder> Things { get; set; } = new List<MyOtherDataHolder> { new MyOtherDataHolder(35, 20, 0.35), new MyOtherDataHolder(40, 20, 0.33), new MyOtherDataHolder(45, 20, 0.31) };

        [CalcParameter(CalcParameterType.Input, "LL2", "List of lists of more things", ["Misc"])]
        public List<List<MyDataHolder>> MoreThings { get; set; } = [[new MyDataHolder(100, 200), new MyDataHolder(300, 400)], [new MyDataHolder(500, 600)]];

        public CalcStatus Status => CalcStatus.None;

        List<IInteractiveGeometryItem> geometry = new List<IInteractiveGeometryItem>();
        public List<IInteractiveGeometryItem> InteractiveGeometryItems => geometry;

        List<GeometryBase> _geometryBases = new List<GeometryBase>();
        public List<GeometryBase> Geometry => _geometryBases;

        public TestCalc2()
        {
            var xg = new InteractiveGeometryQuantityOnXY(
                xGetter: () => this.Offset1.Value,
                xSetter: (newValue) => { this.Offset1 = Length.From(newValue, Breadth.Unit); },
                yGetter: () => this.Offset2.Value,
                ySetter: (newValue) => { this.Offset2 = Length.From(newValue, Breadth.Unit); },
                false,
                false
                );
            geometry.Add(xg);

            var xg2 = new InteractiveGeometryQuantityOnXY(
                xGetter: () => 0,
                xSetter: null,
                yGetter: () => this.Height.Value,
                ySetter: (newValue) => { this.Height = Length.From(newValue, Height.Unit); },
                true,
                true,
                xOffset: () => this.Offset1.Value,
                yOffset: () => this.Offset2.Value
                );
            geometry.Add(xg2);

            var xg3 = new InteractiveGeometryQuantityOnXY(
                xGetter: () => this.Breadth.Value,
                xSetter: (newValue) => { this.Breadth = Length.From(newValue, Breadth.Unit); },
                yGetter: () => 0,
                ySetter: null,
                true,
                true,
                xOffset: () => this.Offset1.Value,
                yOffset: () => this.Offset2.Value
                );
            geometry.Add(xg3);

        }



        public void Calculate()
        {
            MomentOut = Moment * Multiplier;

            ForceRequired = (Moment / Breadth).ToUnit(ForceUnit.Kilonewton);

            var lines = new List<Line>();
            var topLeft = (Offset1.Value - Breadth.Value / 2, Offset2.Value + Height.Value / 2);
            var topRight = (Offset1.Value + Breadth.Value / 2, Offset2.Value + Height.Value / 2);
            var bottomRight = (Offset1.Value + Breadth.Value / 2, Offset2.Value - Height.Value / 2);
            var bottomLeft = (Offset1.Value - Breadth.Value / 2, Offset2.Value - Height.Value / 2);
            lines.AddRange(CreateContinuousPath(new List<(double x, double y)> { topLeft, topRight, bottomRight, bottomLeft, topLeft }));

            _geometryBases.Clear();
            _geometryBases.AddRange(lines);

        }
        public IList<IOutputItem> GetFormulae()
        {
            var returnList = new List<IOutputItem>();

            var outputs = new OutputItem("reffy", "This one goes first", new TextItem("We can explain a bit about the formula here. There is no longer a separate 'Narrative' property."));
            outputs.Expressions.Add(new LatexItem(@"M = \frac{wl^2} {8}"));
            outputs.Expressions.Add(new TextItem("and then a bit more text whcih can now be in-line", true));
            outputs.Expressions.Add(new TextItem("and then an image"));
            outputs.Expressions.Add(new ImageItem(
                new ImageFromSkBitmap(
                    Utilities.CreateDetailedISectionBitmap(
                        Height.Value,
                        Breadth.Value,
                        FlangeThickness.Value,
                        WebThickness.Value,
                        RootRadius.Value, SKColors.Orange))));
            //outputs.Expressions.Add(new ImageItem(new ImageFromSkBitmap(Utilities.CreateMultiCircleImage([[50, 20, 10], [10,10,2]], SKColors.Orange)), true));
            outputs.Expressions.Add(new TextItem("and then another formula", true));
            outputs.Expressions.Add(new LatexItem(@"E = mc^2"));
            outputs.Expressions.Add(new TextItem("all of which can be set to in-line or new line"));

            returnList.Add(outputs);

            return returnList;
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

    public class MyDataHolder
    {
        [CalcParameter(CalcParameterType.Input, "L_{col}", "Prop 1")]
        public Length FirstLength { get; set; } = new Length(10, LengthUnit.Meter);
        [CalcParameter(CalcParameterType.Input, "P_2", "Prop Two")]
        public Force ForceyForce { get; set; } = new Force(100, ForceUnit.Kilonewton);

        public MyDataHolder()
        {
        }

        public MyDataHolder(double length, double forceyForce)
        {
            FirstLength = Length.From(length, LengthUnit.Meter);
            ForceyForce = Force.From(forceyForce, ForceUnit.Kilonewton);
        }
    }

    public class MyOtherDataHolder
    {
        [CalcParameter(CalcParameterType.Input, "f_{ck}", "Char compressive strength")]
        public Pressure ComeStr { get; set; } = new Pressure(35, PressureUnit.NewtonPerSquareMillimeter);
        [CalcParameter(CalcParameterType.Input, "P_2", "Prop Two")]
        public Force ForceyForce { get; set; } = new Force(100, ForceUnit.Kilonewton);
        [CalcParameter(CalcParameterType.Input, @"\epsilon_t", "")]
        public Ratio MyRatio { get; set; } = new Ratio(0.35, RatioUnit.DecimalFraction);

        public MyOtherDataHolder()
        {
        }

        public MyOtherDataHolder(double strength, double forceyForce, double ratio)
        {
            ComeStr = Pressure.From(strength, PressureUnit.NewtonPerSquareMillimeter);
            ForceyForce = Force.From(forceyForce, ForceUnit.Kilonewton);
            MyRatio = Ratio.From(ratio, RatioUnit.DecimalFraction);
        }
    }
}
