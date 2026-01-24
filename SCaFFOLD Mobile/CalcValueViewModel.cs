using Microsoft.Maui.Graphics;
using Scaffold.Core.CalcValues; // Ensure you have access to ICalcValue
using Scaffold.Core.Interfaces;

namespace SCaFFOLD_Mobile
{
    public class CalcValueViewModel : BaseViewModel
    {
        private readonly ICalcValue _model;

        public CalcValueViewModel(ICalcValue model)
        {
            _model = model;
        }

        // Display Name (e.g., "Beam Length")
        public string Name => _model.TypeName;

        // Symbol (e.g., "L")
        public string Symbol => _model.Symbol;

        // The value bound to the UI Entry
        public string Value
        {
            get => _model.GetValueAsString();
            set
            {
                // Attempt to parse the string back into the underlying type
                if (_model.TryParse(value))
                {
                    OnPropertyChanged(); // Notify UI that value updated successfully
                }
                else
                {
                    // Optional: Handle validation error logic here 
                    // (e.g., set a property IsInErrorState = true)
                    OnPropertyChanged(); // Refresh to revert to valid value or keep user input depending on UX preference
                }
            }
        }

        // Helper to force UI to refresh (used when Output values change after calculation)
        public void Refresh()
        {
            OnPropertyChanged(nameof(Value));
        }
    }
}
