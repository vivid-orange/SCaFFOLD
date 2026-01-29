using System.Collections.Generic;
using Scaffold;
using Scaffold.Report;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Calculations
{
    public class BoxSectionPropertiesCalculation : ICalculation
    {
        public string CalculationTitle { get; set; } = "Box Section Properties";
        public bool IsSuccess { get; set; }
        public IList<string> ErrorMessages { get; set; } = new List<string>();

        // --- Inputs ---

        [InputCalcValue("h", "Section Height")]
        public Length Height { get; set; } = Length.FromMillimeters(650);

        [InputCalcValue("b", "Section Width")]
        public Length Width { get; set; } = Length.FromMillimeters(650);

        [InputCalcValue("t_f", "Flange Thickness")]
        public Length FlangeThickness { get; set; } = Length.FromMillimeters(50);

        [InputCalcValue("t_w", "Web Thickness")]
        public Length WebThickness { get; set; } = Length.FromMillimeters(50);

        [InputCalcValue("offset", "Web Offset")]
        public Length WebOffset { get; set; } = Length.FromMillimeters(30);

        // --- Outputs: General ---

        [OutputCalcValue("A_{tot}", "Total Area")]
        public Area TotalArea { get; set; }

        // --- Outputs: y-y Axis (Table B27:R34) ---

        [OutputCalcValue("I_{yy}", "Second Moment of Area (y-y)")]
        public AreaMomentOfInertia Iyy { get; set; }

        [OutputCalcValue("y_{bar}", "Neutral Axis Depth (y-y)")]
        public Length NeutralAxisY { get; set; }

        // --- Outputs: z-z Axis (Table B36:R43) ---

        [OutputCalcValue("I_{zz}", "Second Moment of Area (z-z)")]
        public AreaMomentOfInertia Izz { get; set; }

        [OutputCalcValue("z_{bar}", "Neutral Axis Depth (z-z)")]
        public Length NeutralAxisZ { get; set; }

        public string EntityLabel => "Section properties";

        public CalcStatus Status => CalcStatus.None;

        public BoxSectionPropertiesCalculation()
        {

        }
        public void Calculate()
        {
            // Internal variable dimensions for clarity based on spreadsheet logic
            double h = Height.Millimeters;
            double b = Width.Millimeters;
            double tf = FlangeThickness.Millimeters;
            double tw = WebThickness.Millimeters;
            double d = h - (2 * tf); // Depth of web

            // 1. Areas
            double areaFlange = b * tf;
            double areaWeb = d * tw;
            double totalAreaMm2 = (2 * areaFlange) + (2 * areaWeb);
            TotalArea = Area.FromSquareMillimeters(totalAreaMm2);

            // 2. y-y Axis Calculation (Horizontal Bending)
            // Neutral axis is at h/2 due to symmetry in Y
            double yBar = h / 2.0;
            NeutralAxisY = Length.FromMillimeters(yBar);

            double iyyFlanges = 2 * ((b * Math.Pow(tf, 3) / 12.0) + (areaFlange * Math.Pow(yBar - (tf / 2.0), 2)));
            double iyyWebs = 2 * (tw * Math.Pow(d, 3) / 12.0); // Webs centered on Y-axis
            Iyy = AreaMomentOfInertia.From(iyyFlanges + iyyWebs, AreaMomentOfInertiaUnit.MillimeterToTheFourth);

            // 3. z-z Axis Calculation (Vertical Bending)
            // Note: The spreadsheet assumes symmetry or handles offsets in the table B36:R43
            double zBar = b / 2.0;
            NeutralAxisZ = Length.FromMillimeters(zBar);

            // Calculation based on "Izzi + A*yi^2" logic in spreadsheet
            double izzFlanges = 2 * (tf * Math.Pow(b, 3) / 12.0); // Flanges are full width b

            // Webs are offset from the center
            double webDistFromCenter = (b / 2.0) - (tw / 2.0) - WebOffset.Millimeters;
            double izzWebs = 2 * ((d * Math.Pow(tw, 3) / 12.0) + (areaWeb * Math.Pow(webDistFromCenter, 2)));

            Izz = AreaMomentOfInertia.From(izzFlanges + izzWebs, AreaMomentOfInertiaUnit.MillimeterToTheFourth);

            IsSuccess = true;
        }

        public IList<IOutputItem> GetFormulae()
        {
            var formulae = new List<IOutputItem>();

            // 1. Total Area
            var areaOut = new OutputItem("A_tot", "Total Cross-Sectional Area",
                new TextItem("The total area is the sum of the two flanges and two webs."));
            areaOut.Expressions.Add(new LatexItem(@"A_{tot} = 2 \cdot (b \cdot t_f) + 2 \cdot (d \cdot t_w)"));
            formulae.Add(areaOut);

            // 2. Iyy
            var iyyOut = new OutputItem("Iyy_calc", "Second Moment of Area (y-y)",
                new TextItem("Calculated using the parallel axis theorem about the horizontal neutral axis."));
            iyyOut.Expressions.Add(new LatexItem(@"I_{yy} = \sum (I_{local} + A \cdot y^2)"));
            iyyOut.Expressions.Add(new TextItem($"Result: {Iyy}", true));
            formulae.Add(iyyOut);

            // 3. Izz
            var izzOut = new OutputItem("Izz_calc", "Second Moment of Area (z-z)",
                new TextItem("Calculated about the vertical axis, accounting for the web offset."));
            izzOut.Expressions.Add(new LatexItem(@"I_{zz} = 2 \cdot \left( \frac{t_f \cdot b^3}{12} \right) + 2 \cdot \left( \frac{d \cdot t_w^3}{12} + A_{web} \cdot z_{offset}^2 \right)"));
            formulae.Add(izzOut);

            return formulae;
        }
    }
}
