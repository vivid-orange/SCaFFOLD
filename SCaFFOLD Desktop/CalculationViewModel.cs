using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using Scaffold.Geometry;
using Scaffold.Reader;
using Scaffold.Report;

namespace Scaffold.Desktop
{
    public class CalculationViewModel : ViewModelBase
    {
        private ICalculation _currentCalculation;
        private readonly Stack<ICalculation> _navigationStack = new Stack<ICalculation>();

        public ObservableCollection<ICalculation> Breadcrumbs { get; } = [];
        public ObservableCollection<CalcNodeViewModel> Inputs { get; } = [];
        public ObservableCollection<CalcNodeViewModel> Outputs { get; } = [];
        public ObservableCollection<OutputItemViewModel> CalculationDetails { get; } = [];

        public string CurrentTitle => _currentCalculation.CalculationTitle;
        public ICommand NavigateUpCommand { get; }

        private InteractiveGeometryViewModel _geometryVm;
        public InteractiveGeometryViewModel Geometry
        {
            get => _geometryVm;
            set { _geometryVm = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasGeometry)); }
        }
        public bool HasGeometry => Geometry != null;

        public CalculationViewModel(ICalculation rootCalculation)
        {
            NavigateUpCommand = new RelayCommand(NavigateBack);
            NavigateTo(rootCalculation);
        }

        private void NavigateTo(ICalculation calculation)
        {
            if (_currentCalculation != null && _currentCalculation != calculation)
            {
                _navigationStack.Push(_currentCalculation);
            }

            _currentCalculation = calculation;
            UpdateBreadcrumbs();
            RefreshData();
        }

        private void NavigateBack(object targetCalculation)
        {
            var target = targetCalculation as ICalculation;
            if (target == null)
            {
                return;
            }

            if (_currentCalculation == target)
            {
                return;
            }

            if (_navigationStack.Contains(target))
            {
                while (_navigationStack.Count > 0 && _navigationStack.Peek() != target)
                {
                    _navigationStack.Pop();
                }
                if (_navigationStack.Count > 0)
                {
                    _currentCalculation = _navigationStack.Pop();
                }
            }
            else if (_navigationStack.Count > 0)
            {
                _currentCalculation = _navigationStack.Pop();
            }

            UpdateBreadcrumbs();
            RefreshData();
        }

        private void UpdateBreadcrumbs()
        {
            Breadcrumbs.Clear();
            foreach (ICalculation? item in _navigationStack.Reverse())
            {
                Breadcrumbs.Add(item);
            }
            if (_currentCalculation != null)
            {
                Breadcrumbs.Add(_currentCalculation);
            }
            OnPropertyChanged(nameof(CurrentTitle));
        }

        private void RefreshData()
        {
            Inputs.Clear();
            Outputs.Clear();
            CalculationDetails.Clear();

            Geometry = null;

            if (_currentCalculation == null)
            {
                return;
            }

            // 1. Inputs
            List<ICalcValue> rawInputs = CalculationReader.GetInputs(_currentCalculation);
            BuildTreeNodes(Inputs, rawInputs, isInput: true);

            // 2. Outputs
            List<ICalcValue> rawOutputs = CalculationReader.GetOutputs(_currentCalculation);
            BuildTreeNodes(Outputs, rawOutputs, isInput: false);

            // 3. Details
            RebuildCalculationDetails();

            // 4. Interactive Geometry
            if (_currentCalculation is IInteractiveGeometry interactiveCalc)
            {
                Geometry = new InteractiveGeometryViewModel(interactiveCalc, OnCalculationUpdate);
            }
        }

        private void BuildTreeNodes(ObservableCollection<CalcNodeViewModel> collection, List<ICalcValue> values, bool isInput)
        {
            collection.Clear();
            foreach (ICalcValue item in values)
            {
                AddRecursive(collection, item, isInput);
            }
        }

        private void AddRecursive(ObservableCollection<CalcNodeViewModel> nodes, ICalcValue model, bool isInput)
        {
            var vm = new CalcValueViewModel(model, OnCalculationUpdate, (calc) => NavigateTo(calc));

            // Headings Logic
            ObservableCollection<CalcNodeViewModel> currentLevel = nodes;
            if (model.Headings != null)
            {
                foreach (string? heading in model.Headings)
                {
                    CalcNodeViewModel? group = currentLevel.FirstOrDefault(n => n.Name == heading && n.IsGroup);
                    if (group != null)
                    {
                        currentLevel = group.Children;
                    }
                    else
                    {
                        var newGroup = new CalcNodeViewModel(heading);
                        currentLevel.Add(newGroup);
                        currentLevel = newGroup.Children;
                    }
                }
            }

            var itemNode = new CalcNodeViewModel(vm);
            currentLevel.Add(itemNode);

            // --- STOP RECURSION CHECKS ---

            // 1. If it's a Calculation, stop (displayed as link)
            if (model.IsICalculation)
            {
                return;
            }

            // 2. If it's a Table (Single List of Complex Objects), configure and stop
            // The ViewModel will populate TableHeaders/TableRows, and the View will switch template.
            if (isInput && vm.TryConfigureAsTable())
            {
                return;
            }

            // --- CONTINUE RECURSION ---

            if (model.IsComplexValue)
            {
                List<ICalcValue> children = isInput ? model.GetChildInputs() : model.GetChildOutputs();
                foreach (ICalcValue? child in children)
                {
                    AddRecursive(itemNode.Children, child, isInput);
                }
            }
            else if (model.IsCollection)
            {
                if (vm.RawValue is IList list)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        ICalcValue itemWrapper = CreateCollectionItemWrapper(list, i);
                        if (itemWrapper != null)
                        {
                            AddRecursive(itemNode.Children, itemWrapper, isInput);
                        }
                    }
                }
            }
        }

        private ICalcValue CreateCollectionItemWrapper(IList collection, int index)
        {
            object item = collection[index];
            if (item == null)
            {
                return null;
            }

            Type itemType = item.GetType();

            MethodInfo method = typeof(CalculationViewModel).GetMethod(nameof(CreateWrapperGeneric), BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo generic = method.MakeGenericMethod(itemType);

            return (ICalcValue)generic.Invoke(this, new object[] { collection, index });
        }

        private ICalcValue CreateWrapperGeneric<T>(IList collection, int index)
        {
            Func<T> getter = () => (T)collection[index];
            Action<T> setter = (val) => collection[index] = val;
            string name = $"[{index}]";

            return new DelegateCalcValue<T>(getter, setter, string.Empty, name, null);
        }

        private void OnCalculationUpdate()
        {
            _currentCalculation.Calculate();
            RefreshNodes(Inputs);
            Outputs.Clear();
            List<ICalcValue> rawOutputs = CalculationReader.GetOutputs(_currentCalculation);
            BuildTreeNodes(Outputs, rawOutputs, isInput: false);
            RebuildCalculationDetails();
            Geometry?.Refresh();
        }

        private void RefreshNodes(ObservableCollection<CalcNodeViewModel> nodes)
        {
            foreach (CalcNodeViewModel node in nodes)
            {
                if (node.IsData)
                {
                    node.Value.Refresh();
                }

                if (node.Children.Count > 0)
                {
                    RefreshNodes(node.Children);
                }
            }
        }

        private void RebuildCalculationDetails()
        {
            CalculationDetails.Clear();
            IList<IOutputItem> newItems = _currentCalculation.GetFormulae();
            if (newItems != null)
            {
                foreach (IOutputItem? item in newItems)
                {
                    CalculationDetails.Add(new OutputItemViewModel(item));
                }
            }
        }
    }
}
