using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Sections.Reinforcement;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Sections.Reinforcement;
using Scaffold.Core.CalcValues;
using UnitsNet;

namespace Scaffold.Calculations.Sections.Concrete.Reinforcement;
public class CreateRebar : CalcObjectInput<CalcRebar>
{
    public override string DisplayName { get; set; } = "Create Rebar";

    [InputCalcValue("Grd", "Grade")]
    public CalcSelectionList RebarGrade { get; set; }
            = new CalcSelectionList("Rebar Grade", "B500B", EnumSelectionListParser.RebarGrades);

    [InputCalcValue("Ø", "Diameter")]
    public CalcSelectionList BarDiameter { get; set; }
            = new CalcSelectionList("Bar Diameter", "12", EnumSelectionListParser.BarDiameters);

    protected override CalcRebar InitialiseOutput()
    {
        EnRebarGrade grade = RebarGrade.GetEnum<EnRebarGrade>();
        var material = new EnRebarMaterial(grade, NationalAnnex.UnitedKingdom);
        Length diameter = Length.Parse($"{BarDiameter.Value}mm");
        return new CalcRebar(material, diameter, DisplayName ?? $"Ø{diameter} {grade} Rebar");
    }

    public CreateRebar() { }

    public CreateRebar(BarDiameter diameter)
    {
        BarDiameter.Value = diameter.ToString().Replace("D", string.Empty);
    }
}
