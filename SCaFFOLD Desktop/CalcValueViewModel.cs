using Scaffold.Core;
using Scaffold.Core.CalcValues;
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
        private readonly Action<IComplex> _onNavigateRequest;
        private readonly Action<ICalcValue, ICalcValue> _onReplaceRequest;
        private readonly Type _declaredType;

        public ICalcValue Model => _model;
        public Type DeclaredType => _declaredType;

        public CalcValueViewModel(
            ICalcValue model,
            Action onValueChanged,
            Action<IComplex> onNavigateRequest = null,
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
        public string DisplayName => _model.DisplayName;
        public string Symbol => _model.Symbol;

        public string Value
        {
            get
            {
                if (_model is IListOfDoubleArrays arrayModel)
                    return FormatArrayOutput(arrayModel.Value);
                return _model.GetValue();
            }
            set
            {
                if (IsStandard && _model.GetValue() != value)
                {
                    _model.SetValue(value);
                    Refresh();
                    _onValueChanged?.Invoke();
                }
            }
        }

        public string Unit => (_model as IQuantity)?.Unit ?? string.Empty;
        public bool HasUnit => !string.IsNullOrEmpty(Unit);

        public bool IsStandard => !IsSelectionList && !IsDoubleListArray;
        public bool IsComplex => _model is IComplex && _onNavigateRequest != null;
        public bool IsSelectionList => _model is ISelectionList;
        public bool IsDoubleListArray => _model is IListOfDoubleArrays;

        public IEnumerable<string> SelectionOptions =>
            (_model as ISelectionList)?.Selections ?? (IEnumerable<string>)[];

        public int SelectedIndex
        {
            get => (_model as ISelectionList)?.SelectedItemIndex ?? -1;
            set
            {
                if (_model is ISelectionList listModel && listModel.SelectedItemIndex != value)
                {
                    listModel.SelectedItemIndex = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Value));
                    _onValueChanged?.Invoke();
                }
            }
        }

        public ObservableCollection<ArrayRowViewModel> TableRows { get; } = [];
        public ICommand AddRowCommand => new RelayCommand(_ => AddTableRow());

        private void InitializeComplexTypes()
        {
            if (IsDoubleListArray && _model is IListOfDoubleArrays arrayModel)
            {
                RebuildTable();
            }
        }

        private void RebuildTable()
        {
            TableRows.Clear();
            var list = (_model as IListOfDoubleArrays)?.Value;
            if (list == null) return;
            foreach (var row in list) TableRows.Add(new ArrayRowViewModel(row, _onValueChanged));
        }

        private void AddTableRow()
        {
            if (_model is IListOfDoubleArrays arrayModel)
            {
                int colCount = (arrayModel.Value.Count > 0) ? arrayModel.Value[0].Length : 1;
                var newRow = new double[colCount];
                arrayModel.Value.Add(newRow);
                TableRows.Add(new ArrayRowViewModel(newRow, _onValueChanged));
                _onValueChanged?.Invoke();
            }
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
            if (_model is IComplex complex) _onNavigateRequest?.Invoke(complex);
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
