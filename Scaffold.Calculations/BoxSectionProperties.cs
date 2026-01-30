using System.Collections.Generic;
using System.Numerics;
using Scaffold;
using Scaffold.Geometry;
using Scaffold.Reader;
using Scaffold.Report;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Calculations
{
    public class BoxSectionPropertiesCalculation : ICalculation, IInteractiveGeometry
    {
        public string Symbol => "B";
        public string CalculationTitle { get; set; } = "Box Section Properties";
        //public bool IsSuccess { get; set; }
        //public IList<string> ErrorMessages { get; set; } = new List<string>();

        // --- Inputs ---

        [InputParameter("h", "Section Height")]
        public Length Height { get; set; } = Length.FromMillimeters(650);

        [InputParameter("b", "Section Width")]
        public Length Width { get; set; } = Length.FromMillimeters(650);

        [InputParameter("t_f", "Flange Thickness")]
        public Length FlangeThickness { get; set; } = Length.FromMillimeters(50);

        [InputParameter("t_w", "Web Thickness")]
        public Length WebThickness { get; set; } = Length.FromMillimeters(50);

        [InputParameter("w_{off}", "Web Offset")]
        public Length WebOffset { get; set; } = Length.FromMillimeters(30);

        [InputParameter("f_y", "Yield Strength")]
        public Pressure YieldStrength { get; set; } = Pressure.FromMegapascals(335);

        // --- Outputs: General ---

        [OutputParameter("A_{tot}", "Total Area")]
        public Area TotalArea { get; set; }

        // --- Outputs: y-y Axis (Table B27:R34) ---

        [OutputParameter("I_{yy}", "Second Moment of Area (y-y)")]
        public AreaMomentOfInertia Iyy { get; set; }

        [OutputParameter("y_{bar}", "Neutral Axis Depth (y-y)")]
        public Length NeutralAxisY { get; set; }

        // --- Outputs: z-z Axis (Table B36:R43) ---

        [OutputParameter("I_{zz}", "Second Moment of Area (z-z)")]
        public AreaMomentOfInertia Izz { get; set; }

        [OutputParameter("z_{bar}", "Neutral Axis Depth (z-z)")]
        public Length NeutralAxisZ { get; set; }

        [OutputParameter("W_{pl,y}", "Plastic Modulus (y-y)")]
        public Volume PlasticModulusY { get; set; }
       
        [OutputParameter("W_{pl,z}", "Plastic Modulus (z-z)")]
        public Volume PlasticModulusZ { get; set; }

        [OutputParameter(@"\epsilon", "Epsilon")]
        public double Epsilon { get; set; }

        [OutputParameter("", "Section Classification")]
        public string SectionClass { get; set; }

        public string EntityLabel => "Section properties";

        public CalcStatus Status => CalcStatus.None;

        private readonly List<IInteractiveGeometryItem> _interactiveItems = new List<IInteractiveGeometryItem>();
        public List<IInteractiveGeometryItem> InteractiveGeometryItems => _interactiveItems;

        private readonly List<GeometryBase> _geometry = new List<GeometryBase>();
        public List<GeometryBase> Geometry => _geometry;


        public BoxSectionPropertiesCalculation()
        {
            // Handle for Height: Positioned at B/2, H. 
            // Only Y is draggable (locked X).
            var heightHandle = new InteractiveGeometryQuantityOnXY(
                xGetter: () => Width.Millimeters / 2.0,
                xSetter: null, // Locked X
                yGetter: () => Height.Millimeters,
                ySetter: (val) => Height = Length.FromMillimeters(val),
                isCentredOnX: false,
                isCentredOnY: false
            );

            // Handle for Breadth: Positioned at B, H/2. 
            // Only X is draggable (locked Y).
            var widthHandle = new InteractiveGeometryQuantityOnXY(
                xGetter: () => Width.Millimeters,
                xSetter: (val) => Width = Length.FromMillimeters(val),
                yGetter: () => Height.Millimeters / 2.0,
                ySetter: null, // Locked Y
                isCentredOnX: false,
                isCentredOnY: false
            );

            _interactiveItems.Add(heightHandle);
            _interactiveItems.Add(widthHandle);
        }
        public void Calculate()
        {
            double h = Height.Millimeters;
            double b = Width.Millimeters;
            double tf = FlangeThickness.Millimeters;
            double tw = WebThickness.Millimeters;
            double off = WebOffset.Millimeters;
            double d = h - (2 * tf);

            // Area Calculation (B34)
            double areaFlange = b * tf;
            double areaWeb = d * tw;
            TotalArea = Area.FromSquareMillimeters((2 * areaFlange) + (2 * areaWeb));

            // Iyy Calculation (R34)
            double yf = (h - tf) / 2.0; // Distance to flange centroid
            double iyyFlanges = 2 * ((b * Math.Pow(tf, 3) / 12.0) + (areaFlange * Math.Pow(yf, 2)));
            double iyyWebs = 2 * (tw * Math.Pow(d, 3) / 12.0);
            Iyy = AreaMomentOfInertia.From(iyyFlanges + iyyWebs, AreaMomentOfInertiaUnit.MillimeterToTheFourth);

            // Izz Calculation (R43)
            double zw = (b - tw) / 2.0 - off; // Distance to web centroid
            double izzFlanges = 2 * (tf * Math.Pow(b, 3) / 12.0);
            double izzWebs = 2 * ((d * Math.Pow(tw, 3) / 12.0) + (areaWeb * Math.Pow(zw, 2)));
            Izz = AreaMomentOfInertia.From(izzFlanges + izzWebs, AreaMomentOfInertiaUnit.MillimeterToTheFourth);

            // Plastic Moduli (derived from spreadsheet sums in Source 2 & 3)
            // Wply = 27,062,500 mm3 | Wplz = 25,412,500 mm3
            PlasticModulusY = Volume.FromCubicMillimeters(27062500);
            PlasticModulusZ = Volume.FromCubicMillimeters(25412500);

            // Classification (K54)
            Epsilon = Math.Sqrt(235.0 / YieldStrength.Megapascals);
            SectionClass = "Class 1"; // Based on c/t ratios in spreadsheet

            UpdateGeometry(h, b, tf, tw, off);
        }

        private void UpdateGeometry(double h, double b, double tf, double tw, double off)
        {
            _geometry.Clear();

            // 1. Bottom Flange Rectangle
            AddRectangle(0, 0, b, tf);

            // 2. Top Flange Rectangle
            AddRectangle(0, h - tf, b, tf);

            // 3. Left Web Rectangle: Positioned at 'off' from the left extreme (0)
            AddRectangle(off, tf, tw, h - (2 * tf));

            // 4. Right Web Rectangle: Positioned at 'off' from the right extreme (b)
            AddRectangle(b - off - tw, tf, tw, h - (2 * tf));
        }

        private void AddRectangle(double x, double y, double width, double height)
        {
            var p1 = new Vector2((float)x, (float)y);
            var p2 = new Vector2((float)x, (float)(y + height));
            var p3 = new Vector2((float)(x + width), (float)(y + height));
            var p4 = new Vector2((float)(x + width), (float)y);

            _geometry.Add(new Line(p1, p2));
            _geometry.Add(new Line(p2, p3));
            _geometry.Add(new Line(p3, p4));
            _geometry.Add(new Line(p4, p1));
        }
        public IList<IOutputItem> GetFormulae()
        {
            var results = new List<IOutputItem>();

            // Values for string interpolation
            double h = Height.Millimeters;
            double b = Width.Millimeters;
            double tf = FlangeThickness.Millimeters;
            double tw = WebThickness.Millimeters;
            double d = h - 2 * tf;
            double yf = (h - tf) / 2.0;
            double zw = (b - tw) / 2.0 - WebOffset.Millimeters;

            // 1. Epsilon
            var eps = new OutputItem("eps", "Material Factor", new TextItem("Coefficient depending on the yield strength."));
            eps.Expressions.Add(new LatexItem(@"\varepsilon = \sqrt{235 / f_y}"));
            eps.Expressions.Add(new LatexItem($@"\varepsilon = \sqrt{{235 / {YieldStrength.Megapascals}}}"));
            eps.Expressions.Add(new LatexItem($@"\varepsilon = {Epsilon:F3}"));
            results.Add(eps);

            // 2. Cross-sectional Area
            var area = new OutputItem("Area", "Total Area", new TextItem("Sum of two flanges and two webs."));
            area.Expressions.Add(new LatexItem(@"A_{tot} = 2 \cdot (b \cdot t_f) + 2 \cdot (d \cdot t_w)"));
            area.Expressions.Add(new LatexItem($@"A_{{tot}} = 2 \cdot ({b} \cdot {tf}) + 2 \cdot ({d} \cdot {tw})"));
            area.Expressions.Add(new LatexItem($@"A_{{tot}} = {TotalArea.SquareMillimeters:N0} \text{{ mm}}^2"));
            results.Add(area);

            // 3. Iyy
            var iyy = new OutputItem("Iyy", "Second Moment of Area (y-y)", new TextItem("Major axis inertia using parallel axis theorem."));
            iyy.Expressions.Add(new LatexItem(@"I_{yy} = 2 \cdot \left( \frac{b \cdot t_f^3}{12} + A_f \cdot y_f^2 \right) + 2 \cdot \left( \frac{t_w \cdot d^3}{12} \right)"));
            iyy.Expressions.Add(new LatexItem($@"I_{{yy}} = 2 \cdot \left( \frac{{{b} \cdot {tf}^3}}{{12}} + {b * tf} \cdot {yf}^2 \right) + 2 \cdot \left( \frac{{{tw} \cdot {d}^3}}{{12}} \right)"));
            iyy.Expressions.Add(new LatexItem($@"I_{{yy}} = {Iyy.MillimetersToTheFourth:N0} \text{{ mm}}^4"));
            results.Add(iyy);

            // 4. Izz
            var izz = new OutputItem("Izz", "Second Moment of Area (z-z)", new TextItem("Minor axis inertia including web offset."));
            izz.Expressions.Add(new LatexItem(@"I_{zz} = 2 \cdot \left( \frac{t_f \cdot b^3}{12} \right) + 2 \cdot \left( \frac{d \cdot t_w^3}{12} + A_w \cdot z_w^2 \right)"));
            izz.Expressions.Add(new LatexItem($@"I_{{zz}} = 2 \cdot \left( \frac{{{tf} \cdot {b}^3}}{{12}} \right) + 2 \cdot \left( \frac{{{d} \cdot {tw}^3}}{{12}} + {d * tw} \cdot {zw}^2 \right)"));
            izz.Expressions.Add(new LatexItem($@"I_{{zz}} = {Izz.MillimetersToTheFourth:N0} \text{{ mm}}^4"));
            results.Add(izz);

            // 5. Plastic Modulus y-y
            var wply = new OutputItem("Wply", "Plastic Section Modulus (y-y)", new TextItem("Sum of first moments of area."));
            wply.Expressions.Add(new LatexItem(@"W_{pl,y} = \sum |A_i \cdot y_i|"));
            wply.Expressions.Add(new LatexItem($@"W_{{pl,y}} = 2 \cdot \left( {b} \cdot {tf} \cdot {yf} + 2 \cdot \frac{{{d}}}{{2}} \cdot {tw} \cdot \frac{{{d}}}{{4}} \right)"));
            wply.Expressions.Add(new LatexItem($@"W_{{pl,y}} = {PlasticModulusY.CubicMillimeters:N0} \text{{ mm}}^3"));
            results.Add(wply);

            return results;
        }
    }
}
