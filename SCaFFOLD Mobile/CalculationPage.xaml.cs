using Scaffold.Core.Interfaces;

namespace SCaFFOLD_Mobile;

    public partial class CalculationPage : ContentPage
{
    // Constructor that accepts the specific calculation instance
    public CalculationPage(ICalculation calculation)
    {
        InitializeComponent();
        BindingContext = new CalculationViewModel(calculation);
    }
}

