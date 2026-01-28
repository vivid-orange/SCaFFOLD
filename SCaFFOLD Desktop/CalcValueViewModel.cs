using Scaffold.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SCaFFOLD_Desktop
{
    public class CalcValueViewModel : ViewModelBase
    {
        private readonly ICalcValue _model;
        private readonly Action _onValueChanged;
        private readonly Action<ICalculation> _onNavigateRequest;

        // ... (Other fields like _declaredType, _onReplaceRequest if needed) ...

        public ICalcValue Model => _model;

        public CalcValueViewModel(
            ICalcValue model,
            Action onValueChanged,
            Action<ICalculation> onNavigateRequest = null)
        {
            _model = model;
            _onValueChanged = onValueChanged;
            _onNavigateRequest = onNavigateRequest;
        }

        // --- Properties ---
        public string DisplayName => _model.EntityLabel; // Mapped from DelegateCalcValue
        public string Symbol => _model.Symbol;

        public string Value
        {
            get => _model.ValueAsString(); // Simplified for brevity
            set
            {
                if (IsStandard && _model.ValueAsString() != value)
                {
                    _model.TryParse(value);
                    Refresh();
                    _onValueChanged?.Invoke();
                }
            }
        }

        public string Unit => (_model as ICalcQuantity)?.Unit ?? "";

        // Structure Flags
        // The View binds to 'IsComplex' to show the "..." button.
        // This remains true for both ICalculation and other Complex objects.
        public bool IsComplex => _model.IsComplexValue || _model.IsICalculation;
        public bool IsCollection => _model.IsCollection;
        public bool IsStandard => !IsComplex && !IsCollection;

        public bool IsSelectionList => false;
        public IEnumerable<string> SelectionOptions => Enumerable.Empty<string>();
        public int SelectedIndex { get => -1; set { } }

        // --- Helper to get the actual object inside the DelegateCalcValue wrapper ---
        public object RawValue
        {
            get
            {
                var prop = _model.GetType().GetProperty("Value");
                return prop?.GetValue(_model);
            }
        }

        // --- Commands ---

        public ICommand EditCommand => new RelayCommand(_ =>
        {
            // FIX: Check the underlying value, not the wrapper
            if (RawValue is ICalculation calc)
            {
                _onNavigateRequest?.Invoke(calc);
            }
        });

        public void Refresh()
        {
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(Unit));
            OnPropertyChanged(nameof(Symbol));
        }
    }
}
