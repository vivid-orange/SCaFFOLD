using Scaffold.Calculations;
using Scaffold.Core.Interfaces;
using Scaffold.Core.CalcValues;
using Scaffold.Core;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Scaffold.Calculations.Eurocode.Steel;

namespace SCaFFOLD_Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //ICalculation calc = new SteelMaterialProperties();
            ICalculation calc = new TestCalc();

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
}
