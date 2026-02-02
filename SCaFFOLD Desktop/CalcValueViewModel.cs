using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Scaffold.Reader; // Required for ICalculation

namespace Scaffold.Desktop
{
    public class CalcValueViewModel : ViewModelBase
    {
        private readonly ICalcValue _model;
        private readonly Action _onValueChanged;
        private readonly Action<ICalculation> _onNavigateRequest;

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
        public string DisplayName => _model.EntityLabel;
        public string Symbol => _model.Symbol;

        public string Value
        {
            get => _model.ToString();
            set
            {
                if (IsStandard && _model.ToString() != value)
                {
                    _model.TryParse(value);
                    Refresh();
                    _onValueChanged?.Invoke();
                }
            }
        }

        // Structure Flags
        public bool IsComplex => _model.IsComplexValue || _model.IsICalculation;
        public bool IsCollection => _model.IsCollection;
        public bool IsStandard => !IsComplex && !IsCollection;
        public bool IsCalculation => _model.IsICalculation;
        public bool IsSelectionList => false;
        public IEnumerable<string> SelectionOptions => Enumerable.Empty<string>();
        public int SelectedIndex { get => -1; set { } }

        public object RawValue
        {
            get
            {
                var prop = _model.GetType().GetProperty("Value");
                return prop?.GetValue(_model);
            }
        }

        // --- Table Logic ---
        public bool IsTable { get; private set; }
        public ObservableCollection<string> TableHeaders { get; } = new ObservableCollection<string>();
        public ObservableCollection<ObservableCollection<CalcValueViewModel>> TableRows { get; } = new ObservableCollection<ObservableCollection<CalcValueViewModel>>();

        public bool TryConfigureAsTable()
        {
            // 1. Must be a collection
            if (!IsCollection) return false;

            var list = RawValue as IList;
            if (list == null) return false;

            // 2. Determine the Item Type (T)
            Type itemType = null;
            var modelType = _model.GetType();
            // Try to get T from DelegateCalcValue<List<T>>
            if (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(DelegateCalcValue<>))
            {
                var listType = modelType.GetGenericArguments()[0];
                if (listType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(listType))
                {
                    itemType = listType.GetGenericArguments()[0];
                }
            }
            // Fallback: Check first item
            if (itemType == null && list.Count > 0 && list[0] != null) itemType = list[0].GetType();

            if (itemType == null) return false;

            // 3. Must be "Complex" Input
            // FIX: Check for the base CalcValueTypeAttribute and ensure Type is Input.
            // This covers both [CalcValueType(CalcValueType.Input)] AND [InputCalcValue].
            bool isComplex = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                     .Any(p =>
                                     {
                                         var attr = p.GetCustomAttribute<CalcParameterAttribute>();
                                         return attr != null && attr.Type == CalcParameterType.Input;
                                     });

            if (!isComplex) return false;

            // 4. Must NOT be a nested collection
            if (typeof(IEnumerable).IsAssignableFrom(itemType) && itemType != typeof(string)) return false;

            // --- It is a Table ---
            IsTable = true;
            TableHeaders.Clear();
            TableRows.Clear();

            // 5. Build Headers
            try
            {
                // Create dummy to read structure via CalculationReader (preferred)
                var dummy = Activator.CreateInstance(itemType);
                if (dummy != null)
                {
                    var prototypes = CalculationReader.GetInputs(dummy);
                    foreach (var p in prototypes)
                    {
                        TableHeaders.Add(p.Symbol);
                    }
                }
            }
            catch
            {
                // Fallback: Manual Reflection using the correct Attribute check
                var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(p => p.GetCustomAttribute<CalcParameterAttribute>())
                    .Where(attr => attr != null && attr.Type == CalcParameterType.Input)
                    .ToList();

                foreach (var attr in props) TableHeaders.Add(attr.Symbol);
            }

            // 6. Build Rows
            foreach (var item in list)
            {
                if (item == null) continue;

                var rowVMs = new ObservableCollection<CalcValueViewModel>();
                // Recursively read inputs for the row item
                var inputs = CalculationReader.GetInputs(item);

                foreach (var input in inputs)
                {
                    rowVMs.Add(new CalcValueViewModel(input, _onValueChanged, _onNavigateRequest));
                }
                TableRows.Add(rowVMs);
            }

            return true;
        }

        // --- Commands ---

        public ICommand EditCommand => new RelayCommand(_ =>
        {
            if (RawValue is ICalculation calc)
            {
                _onNavigateRequest?.Invoke(calc);
            }
        });

        public void Refresh()
        {
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(Symbol));
        }
    }
}
