using System.Collections.Generic;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Profiles;
using Scaffold.Core.CalcObjects.Sections;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Calculations;
public class SectionPropertiesCalculation : ICalculation
{
    public string DisplayName { get; set; }
    public string CalculationName { get; set; } = "Section Properties";
    public CalcStatus Status { get; set; } = CalcStatus.None;

    [InputCalcValue]
    public ICalcProfile Profile { get; set; }
        = new CalcRectangularProfile(new Length(400, LengthUnit.Millimeter), new Length(800, LengthUnit.Millimeter), "400 x 800mm");

    [OutputCalcValue("Section Properties", "SP")]
    public CalcSectionProperties Properties => new CalcSectionProperties(Profile, DisplayName);

    [OutputCalcValue]
    public CalcLength CentroidY => Properties.Centroid.Y;

    [OutputCalcValue]
    public CalcLength CentroidZ => Properties.Centroid.Z;

    [OutputCalcValue]
    public CalcLength Perimeter => Properties.Perimeter;

    [OutputCalcValue]
    public CalcArea Area => Properties.Area;

    [OutputCalcValue]
    public CalcVolume ElasticSectionModulusYy => Properties.ElasticSectionModulusYy;

    [OutputCalcValue]
    public CalcVolume ElasticSectionModulusZz => Properties.ElasticSectionModulusZz;

    [OutputCalcValue]
    public CalcInertia MomentOfInertiaYy => Properties.MomentOfInertiaYy;

    [OutputCalcValue]
    public CalcInertia MomentOfInertiaZz => Properties.MomentOfInertiaZz;

    [OutputCalcValue]
    public CalcLength RadiusOfGyrationYy => Properties.RadiusOfGyrationYy;

    [OutputCalcValue]
    public CalcLength RadiusOfGyrationZz => Properties.RadiusOfGyrationZz;

    public List<IOutputItem> Expressions = new List<IOutputItem>();
    public List<IOutputItem> GetFormulae() => Expressions;

    public SectionPropertiesCalculation()
    {
        Calculate();
    }

    public void Calculate() { }

    public List<ICalcValue> GetInputs() => throw new System.NotImplementedException();
    public List<ICalcValue> GetOutputs() => throw new System.NotImplementedException();
}
