using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;

namespace Scaffold.Calculations.Sections.Steel.Catalogue;
public class CreateEuropeanCatalogueProfileType : CalcObjectInput<CalcSelectionList>
{
    public override string TypeName { get;  } = "European Catalogue Profile Type";

    [InputCalcValue("Typ", "Profile Type")]
    public CalcSelectionList CatalogueType { get; set; }
            = new CalcSelectionList("Profile Type", "UB", CatalogueProfileSelectionList.Catalogues);

    public CreateEuropeanCatalogueProfileType() { }

    protected override CalcSelectionList InitialiseOutput()
    {
        return new CalcSelectionList("Profile", null,
                    CatalogueProfileSelectionList.GetCatalogueProfiles(CatalogueType.GetEnum<CatalogueType>()));
    }
}
