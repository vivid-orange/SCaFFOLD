using System.Numerics;
using Scaffold.Geometry;
using Scaffold.Reader.Images;
using Scaffold.Report;
using SkiaSharp;

namespace Scaffold.Calculations
{
    public enum SectionType { Rectangular, Circular, IShaped, TShaped }

    public class TestCalc2 : Calculation, IInteractiveGeometry
    {
        public override string CalculationTitle { get; } = "This is my test calc";
        public override string EntityLabel { get; } = "Test calc";

        // public property with getter and setter will be treated as input by reader
        public double Multiplier { get; set; }

        [InputParameter("S_t", "Section Type", ["Geometry"])]
        public SectionType Section { get; set; } = SectionType.Rectangular;

        // Decorate a property with attribute to set custom symbol
        [InputParameter("M")]
        public Moment Moment { get; set; } = new Moment(20, MomentUnit.KilonewtonMeter);

        // Optionally add headings for grouping in UI
        [InputParameter("B", "Breadth", ["Geometry", "Section"])]
        public Length Breadth { get; set; } = new Length(200, LengthUnit.Millimeter);

        // Using specialized attribute for input parameter
        [InputParameter("C_x", "Centre X", ["Geometry", "Centre"])]
        public Length Offset1 { get; set; } = new Length(5, LengthUnit.Millimeter);


        [InputParameter("C_y", "Centre Y", ["Geometry", "Centre"])]
        public Length Offset2 { get; set; } = new Length(5, LengthUnit.Millimeter);

        [InputParameter("E", "Column height", ["Misc"])]
        public EmbeddedCalc ReducedHeight { get; set; } = new EmbeddedCalc();

        [InputParameter("H", "Height", ["Geometry", "Section"])]
        public Length Height { get; set; } = new Length(500, LengthUnit.Millimeter);

        [InputParameter("T", "Flange", ["Geometry", "Section"])]
        public Length FlangeThickness { get; set; } = new Length(25, LengthUnit.Millimeter);

        [InputParameter("t", "Web", ["Geometry", "Section"])]
        public Length WebThickness { get; set; } = new Length(12, LengthUnit.Millimeter);

        [InputParameter("r", "Root radius", ["Geometry", "Section"])]
        public Length RootRadius { get; set; } = new Length(5, LengthUnit.Millimeter);


        // Public property with private setter will be treated as output by reader
        public Moment MomentOut { get; private set; } = new Moment(0, MomentUnit.KilonewtonMeter);

        // Decorate a property with attribute to set custom symbol and display name
        [OutputParameter("F_{req}", "Force required")]
        public Force ForceRequired { get; private set; } = new Force(0, ForceUnit.Kilonewton);

        [InputParameter("C", "Complex Input type", ["Misc"])]
        public MyDataHolder ComplexValue { get; set; } = new MyDataHolder();

        [InputParameter("L", "List of things", ["Misc"])]
        public List<MyOtherDataHolder> Things { get; set; } = new List<MyOtherDataHolder> { new MyOtherDataHolder(35, 20, 0.35), new MyOtherDataHolder(40, 20, 0.33), new MyOtherDataHolder(45, 20, 0.31) };

        [InputParameter("LL2", "List of lists of more things", ["Misc"])]
        public List<List<MyDataHolder>> MoreThings { get; set; } = [[new MyDataHolder(100, 200), new MyDataHolder(300, 400)], [new MyDataHolder(500, 600)]];

        [CalcIgnore]
        public List<IInteractiveGeometryItem> InteractiveGeometryItems => _geometry;

        [CalcIgnore]
        public List<GeometryBase> Geometry => _geometryBases;

        private List<IInteractiveGeometryItem> _geometry = [];
        private List<GeometryBase> _geometryBases = [];

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
            _geometry.Add(xg);

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
            _geometry.Add(xg2);

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
            _geometry.Add(xg3);

        }



        public override void Calculate()
        {
            MomentOut = Moment * Multiplier;

            ForceRequired = (Moment / Breadth).ToUnit(ForceUnit.Kilonewton);

            var lines = new List<Line>();
            (double, double) topLeft = (Offset1.Value - (Breadth.Value / 2), Offset2.Value + (Height.Value / 2));
            (double, double) topRight = (Offset1.Value + (Breadth.Value / 2), Offset2.Value + (Height.Value / 2));
            (double, double) bottomRight = (Offset1.Value + (Breadth.Value / 2), Offset2.Value - (Height.Value / 2));
            (double, double) bottomLeft = (Offset1.Value - (Breadth.Value / 2), Offset2.Value - (Height.Value / 2));
            lines.AddRange(CreateContinuousPath(new List<(double x, double y)> { topLeft, topRight, bottomRight, bottomLeft, topLeft }));

            _geometryBases.Clear();
            _geometryBases.AddRange(lines);


        }
        public override IList<IOutputItem> GetFormulae()
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
            {
                return lines;
            }

            // Iterate up to the second-to-last point
            for (int i = 0; i < points.Count - 1; i++)
            {
                (double x, double y) current = points[i];
                (double x, double y) next = points[i + 1];

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
        [InputParameter("L_{col}", "Prop 1")]
        public Length FirstLength { get; set; } = new Length(10, LengthUnit.Meter);
        [InputParameter("P_2", "Prop Two")]
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
        [InputParameter("f_{ck}", "Char compressive strength")]
        public Pressure ComeStr { get; set; } = new Pressure(35, PressureUnit.NewtonPerSquareMillimeter);
        [InputParameter("P_2", "Prop Two")]
        public Force ForceyForce { get; set; } = new Force(100, ForceUnit.Kilonewton);
        [InputParameter(@"\epsilon_t", "")]
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
