using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Sections.Reinforcement;
using Scaffold.Core.CalcValues;
using VividOrange.Taxonomy.Sections;

namespace Scaffold.Calculations.Sections.Concrete.Reinforcement;
public class CreateFaceReinforcementLayerByCount : CalcObjectInput<CalcFaceReinforcementLayer>
{
    public override string CalculationName { get; set; } = "Create Face Reinforcement Layer By Count";

    [InputCalcValue("Face", "Section Face")]
    public CalcSelectionList SectionFace { get; set; }
            = new CalcSelectionList("Section Face", "Bottom", EnumSelectionListParser.SectionFaces);

    [InputCalcValue("Bar", "Rebar")]
    public CalcRebar Rebar { get; set; } = new CreateRebar();

    [InputCalcValue("No.", "Number of Bars")]
    public CalcInt Count { get; set; } = 4;

    public CreateFaceReinforcementLayerByCount() { }

    public CreateFaceReinforcementLayerByCount(SectionFace face)
    {
        SectionFace.Value = face.ToString();
    }

    protected override CalcFaceReinforcementLayer InitialiseOutput()
    {
        SectionFace face = SectionFace.GetEnum<SectionFace>();
        return new CalcFaceReinforcementLayer(face, Rebar, Count, ReferenceName ?? $"{SectionFace.Value} {Rebar.DisplayName} {Count}No.");
    }
}
