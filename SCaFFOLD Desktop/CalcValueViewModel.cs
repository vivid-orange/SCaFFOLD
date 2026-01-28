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
        private readonly Action<ICalcValue, ICalcValue> _onReplaceRequest;
        private readonly Type _declaredType;

        public ICalcValue Model => _model;
        public Type DeclaredType => _declaredType;

        public CalcValueViewModel(
            ICalcValue model,
            Action onValueChanged,
            Action<ICalculation> onNavigateRequest = null,
            Type declaredType = null,
            Action<ICalcValue, ICalcValue> onReplaceRequest = null)
        {
            _model = model;
            _onValueChanged = onValueChanged;
            _onNavigateRequest = onNavigateRequest;
            _declaredType = declaredType;
            _onReplaceRequest = onReplaceRequest;

            InitializeComplexTypes();
            InitializeTypeSelection();
        }

        // --- Type Selection Logic ---

        public ObservableCollection<Type> AvailableTypes { get; } = [];

        public Type SelectedType
        {
            get => _model.GetType();
            set
            {
                // Check value is valid and distinct from current
                if (value != null && value != _model.GetType())
                {
                    ChangeImplementation(value);
                }
            }
        }

        private void InitializeTypeSelection()
        {
            if (IsComplex && _declaredType != null && _onReplaceRequest != null)
            {
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => _declaredType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

                foreach (var t in types)
                {
                    AvailableTypes.Add(t);
                }
            }
        }

        private void ChangeImplementation(Type newType)
        {
            try
            {
                // Attempt to create the new object (Must have default constructor)
                var newInstance = Activator.CreateInstance(newType) as ICalcValue;

                if (newInstance != null)
                {
                    _onReplaceRequest?.Invoke(_model, newInstance);
                }
            }
            catch (Exception)
            {
                // If constructor fails, force UI to revert selection (by notifying property changed)
                // This prevents the ComboBox from showing the "new" type when the switch actually failed.
                OnPropertyChanged(nameof(SelectedType));
            }
        }

        // --- Standard Properties ---
        public string DisplayName => _model.EntityLabel;
        public string Symbol => _model.Symbol;

        public string Value
        {
            get
            {
                //if (_model is ICalcListOfDoubleArrays arrayModel)
                //    return FormatArrayOutput(arrayModel.Value);
                return _model.ValueAsString();
            }
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

        public string Unit => "";
        public bool HasUnit => !string.IsNullOrEmpty(Unit);

        public bool IsComplex => _model.IsComplexValue || _model.IsICalculation;
        public bool IsCollection => _model.IsCollection;
        public bool IsStandard => !IsComplex && !IsCollection;

        // GENERALLY : NEED TO REMOVE ALL TRACES OF SELECTION LIST
        public bool IsSelectionList => false; //_model is DelegateCalcValue<CalcSelectionList>;
        public bool IsDoubleListArray => false; // _model is ICalcListOfDoubleArrays;

        // --- Accessor for Collection Iteration ---
        public object RawValue
        {
            get
            {
                // We use reflection to get the 'Value' property from DelegateCalcValue<T>
                // because ICalcValue doesn't expose the generic T.
                // Alternatively, use 'dynamic' if your project supports it.
                var prop = _model.GetType().GetProperty("Value");
                return prop?.GetValue(_model);
            }
        }

        public List<ICalcValue> GetChildren()
        {
            // If it's a single complex object or calculation, use the Reader integration
            if (IsComplex)
            {
                return _model.GetChildInputs();
            }
            return new List<ICalcValue>();
        }

        public IEnumerable<string> SelectionOptions => (IEnumerable<string>)[]; 
            // (_model as DelegateCalcValue<CalcSelectionList>)?.Value.Selections ?? (IEnumerable<string>)[];

        public int SelectedIndex
        {
            get => -1; // (_model as DelegateCalcValue<CalcSelectionList>)?.Value.SelectedItemIndex ?? -1;
            set
            {
                //if (_model is ICalcSelectionList listModel && listModel.SelectedItemIndex != value)
                //{
                //    listModel.SelectedItemIndex = value;
                //    OnPropertyChanged();
                //    OnPropertyChanged(nameof(Value));
                //    _onValueChanged?.Invoke();
                //}
            }
        }

        public ObservableCollection<ArrayRowViewModel> TableRows { get; } = [];
        public ICommand AddRowCommand => new RelayCommand(_ => AddTableRow());

        private void InitializeComplexTypes()
        {
            //if (IsDoubleListArray && _model is ICalcListOfDoubleArrays arrayModel)
            //{
            //    RebuildTable();
            //}
        }

        private void RebuildTable()
        {
            //TableRows.Clear();
            //var list = (_model as ICalcListOfDoubleArrays)?.Value;
            //if (list == null) return;
            //foreach (var row in list) TableRows.Add(new ArrayRowViewModel(row, _onValueChanged));
        }

        private void AddTableRow()
        {
            //if (_model is ICalcListOfDoubleArrays arrayModel)
            //{
            //    int colCount = (arrayModel.Value.Count > 0) ? arrayModel.Value[0].Length : 1;
            //    var newRow = new double[colCount];
            //    arrayModel.Value.Add(newRow);
            //    TableRows.Add(new ArrayRowViewModel(newRow, _onValueChanged));
            //    _onValueChanged?.Invoke();
            //}
        }

        private string FormatArrayOutput(List<double[]> list)
        {
            if (list == null || list.Count == 0) return "Empty";
            var sb = new StringBuilder();
            foreach (var arr in list) { sb.Append(string.Join(", ", arr)); sb.Append("; "); }
            return sb.ToString().TrimEnd(';', ' ');
        }

        public ICommand EditCommand => new RelayCommand(_ =>
        {
            if (_model is ICalculation complex) _onNavigateRequest?.Invoke(complex);
        });

        public void Refresh()
        {
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(Unit));
            OnPropertyChanged(nameof(HasUnit));
            OnPropertyChanged(nameof(Symbol));
            OnPropertyChanged(nameof(SelectedIndex));
            if (IsDoubleListArray) RebuildTable();
        }
    }
}
