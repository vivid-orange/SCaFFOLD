using System.Collections.ObjectModel;
using System.Windows.Input;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Services;

namespace SCaFFOLD_Mobile
{
    public class CalculationViewModel : BaseViewModel
    {
        private readonly ICalculation _calculation;

        public CalculationViewModel(ICalculation calculation)
        {
            _calculation = calculation;
            Title = _calculation.InstanceName ?? "Calculation";

            // Initialize Collections
            Inputs = new ObservableCollection<CalcValueViewModel>();
            Outputs = new ObservableCollection<CalcValueViewModel>();

            LoadData();

            CalculateCommand = new Command(ExecuteCalculate);
        }

        public string Title { get; set; }

        public ObservableCollection<CalcValueViewModel> Inputs { get; }
        public ObservableCollection<CalcValueViewModel> Outputs { get; }

        public ICommand CalculateCommand { get; }

        private void LoadData()
        {
            // Use your existing CalculationReader to extract properties
            var inputModels = CalculationReader.GetInputs(_calculation);
            var outputModels = CalculationReader.GetOutputs(_calculation);

            foreach (var item in inputModels)
                Inputs.Add(new CalcValueViewModel(item));

            foreach (var item in outputModels)
                Outputs.Add(new CalcValueViewModel(item));
        }

        private void ExecuteCalculate()
        {
            try
            {
                // 1. Perform the calculation logic
                _calculation.Calculate();

                // 2. Refresh the Outputs 
                // (Since the underlying objects updated, we just need to notify the UI)
                foreach (var output in Outputs)
                {
                    output.Refresh();
                }

                // Optional: Refresh inputs too if the calc modifies them (rare but possible)
            }
            catch (Exception ex)
            {
                // Handle Calculation errors (display alert, etc.)
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
