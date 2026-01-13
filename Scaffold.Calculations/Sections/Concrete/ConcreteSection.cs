using System;
using System.Collections.Generic;
using System.IO;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Calculations.Sections.Concrete.Reinforcement;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Materials.StandardMaterials.En;
using Scaffold.Core.CalcObjects.Profiles;
using Scaffold.Core.CalcObjects.Sections;
using Scaffold.Core.CalcObjects.Sections.Reinforcement;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Images.Drawing;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using SkiaSharp;
using UnitsNet;
using UnitsNet.Units;
using VividOrange.Taxonomy.Materials.StandardMaterials.En;
using VividOrange.Taxonomy.Sections;
using VividOrange.Taxonomy.Standards.Eurocode;

namespace Scaffold.Calculations.Sections.Concrete;

public class ConcreteSection : CalcObjectInput<CalcConcreteSection>
{
    public override string CalculationName { get; set; } = "Concrete Section";

    [InputCalcValue("Grd", "Concrete Grade")]
    public CalcSelectionList ConcreteGrade { get; set; }
            = new CalcSelectionList("Concrete Grade", "C30/37", EnumSelectionListParser.ConcreteGrades);

    [InputCalcValue] // todo - implement generic profile input
    public ICalcProfile Profile { get; set; }
        = new CalcRectangularProfile(new Length(500, LengthUnit.Millimeter), new Length(1000, LengthUnit.Millimeter), "500 x 1000mm");

    [InputCalcValue]
    public CalcLength Cover { get; set; } = new CalcLength(35, LengthUnit.Millimeter, "Concrete Cover", "Cvr");

    [InputCalcValue("Lnk", "Link rebar")]
    public CalcRebar Link { get; set; } = new CreateRebar();

    [InputCalcValue("Top", "Top rebars")]
    public CalcFaceReinforcementLayer Top { get; set; } = new CreateFaceReinforcementLayerByCount(SectionFace.Top);

    [InputCalcValue("Btm", "Bottom rebars")]
    public CalcFaceReinforcementLayer Bottom { get; set; } = new CreateFaceReinforcementLayerByCount(SectionFace.Bottom);

    [InputCalcValue("Sds", "Side rebars")]
    public CalcFaceReinforcementLayer Sides { get; set; } = new CreateFaceReinforcementLayerBySpacing(SectionFace.Sides);

    public override IList<IFormula> GetFormulae()
    {
        ICalcImage image = Core.Images.Drawing.Sections.DrawSection(Output);
        var formula = new Formula();
        formula.SetImage(image);
        return new List<IFormula>() { formula };
    }

    protected override CalcConcreteSection InitialiseOutput()
    {
        var material = new CalcEnConcreteMaterial(ConcreteGrade.GetEnum<EnConcreteGrade>("/", "_"),
                                       NationalAnnex.RecommendedValues, "Concrete", "C");
        var section = new CalcConcreteSection(Profile, material, Link, Cover, "Concrete Section");
        section.AddRebarLayer(Top);
        section.AddRebarLayer(Bottom);
        section.AddRebarLayer(Sides);
        return section;
    }

    public ConcreteSection() { }
}
