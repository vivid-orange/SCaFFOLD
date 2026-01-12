using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects;
using VividOrange.Taxonomy.Profiles;

namespace Scaffold.Calculations.Sections.Steel.Catalogue;
public class CreateEuropeanCatalogueProfile : CalcObjectInput<CalcObjectWrapper<IEuropeanCatalogue>>
{
    public override string CalculationName { get; set; } = "European Catalogue Profile";

    [InputCalcValue("Prfl", "Profile")]
    // this is a CalcSelectionsList (ProfileType) but needs to be passed as a type so maintain access
    // to the underlying CalcSelectionList (CatalogueType)
    public CreateEuropeanCatalogueProfileType Profile { get; set; } = new CreateEuropeanCatalogueProfileType();

    public CreateEuropeanCatalogueProfile() { }

    protected override CalcObjectWrapper<IEuropeanCatalogue> InitialiseOutput()
    {
        return new(CatalogueFactory.CreateEuropean(Profile.Output.GetEnum<European>()),
                   "Catalogue Profile", Profile.Output);
    }
}
