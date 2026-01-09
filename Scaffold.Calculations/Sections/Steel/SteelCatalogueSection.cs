using System;
using System.Collections.Generic;
using System.IO;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Calculations.Sections.Steel.Catalogue;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Materials.StandardMaterials.En;
using Scaffold.Core.CalcObjects.Sections;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Images.Drawing;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using SkiaSharp;

namespace Scaffold.Calculations.Sections.Steel;
public class SteelCatalogueSection : CalcObjectInput<CalcSection>
{
    public override string DisplayName { get; set; } = "Steel Catalogue Section";

    [InputCalcValue("Grd", "Steel Grade")]
    public CalcSelectionList SteelGrade { get; set; }
        = new CalcSelectionList("Steel Grade", "S355", EnumSelectionListParser.SteelGrades);

    [InputCalcValue]
    public CreateEuropeanCatalogueProfile Profile { get; set; } = new CreateEuropeanCatalogueProfile();

    public override List<IOutputItem> GetFormulae()
    {
        ICalcImage image = Core.Images.Drawing.Sections.DrawSection(Output);
        var formula = new Formula();
        formula.SetImage(image);
        return new List<IOutputItem>() { formula };
    }

    public SteelCatalogueSection() { }

    protected override CalcSection InitialiseOutput()
    {
        var material = new CalcEnSteelMaterial(SteelGrade.GetEnum<EnSteelGrade>(),
                        NationalAnnex.RecommendedValues, "Steel", "S");
        return new CalcSection(Profile.Output.Value, material, "Steel Section");
    }
}
