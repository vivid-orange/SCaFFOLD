using System.Diagnostics;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Sections.Reinforcement;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using UnitsNet.Units;
using VividOrange.Taxonomy.Sections;

namespace Scaffold.Calculations.Sections.Concrete.Reinforcement;

public class CreateFaceReinforcementLayerBySpacing : CalcObjectInput<CalcFaceReinforcementLayer>
{
    public override string CalculationName { get; set; } = "Create Face Reinforcement Layer By Spacing";

    [InputCalcValue("Face", "Section Face")]
    public CalcSelectionList SectionFace { get; set; }
            = new CalcSelectionList("Section Face", "Bottom", EnumSelectionListParser.SectionFaces);

    [InputCalcValue("Bar", "Rebar")]
    public CalcRebar Rebar { get; set; } = new CreateRebar();

    [InputCalcValue("a", "Max. rebar spacing")]
    public CalcLength Spacing { get; set; } = new CalcLength(200, LengthUnit.Millimeter, "Spacing", "a");

    public CreateFaceReinforcementLayerBySpacing() { }

    public CreateFaceReinforcementLayerBySpacing(SectionFace face)
    {
        SectionFace.Value = face.ToString();
    }

    protected override CalcFaceReinforcementLayer InitialiseOutput()
    {
        SectionFace face = SectionFace.GetEnum<SectionFace>();
        return new CalcFaceReinforcementLayer(face, Rebar, Spacing, ReferenceName ?? $"{SectionFace.Value} {Rebar.DisplayName} @ {Spacing} c/c");
    }
}
