using System.Windows;
using Scaffold.Calculations;

namespace Scaffold.Desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        //ICalculation calc = new SteelMaterialProperties();
        //ICalculation calc = new TestCalc2();
        ICalculation calc = new BoxSectionPropertiesCalculation();

        calc.Calculate();

        var viewModel = new CalculationViewModel(calc);
        this.DataContext = viewModel;

        InitializeComponent();
    }

    private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
    {
        if (((FrameworkElement)sender).DataContext is GeometryPointViewModel pointVm)
        {
            // Add delta to X/Y. The ViewModel Setter handles conversion back to Model units.
            pointVm.X += e.HorizontalChange;
            pointVm.Y += e.VerticalChange;
        }
    }
}
