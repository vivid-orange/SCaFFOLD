using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Scaffold;
using Scaffold.Core;
using Scaffold.Core.Geometry;

using Scaffold.Desktop;
using Scaffold.Geometry;
using Scaffold.Reader;

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

        public string CurrentTitle => _currentCalculation?.CalculationTitle; // Updated to match ICalculationStatus property usually
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
            if (target == null) return;
            if (_currentCalculation == target) return;

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
            foreach (var item in _navigationStack.Reverse())
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
            // Geometry = ... (Reset geometry logic)

            if (_currentCalculation == null) return;

            // 1. Inputs - Recursive Build
            var rawInputs = CalculationReader.GetInputs(_currentCalculation);
            BuildTreeNodes(Inputs, rawInputs, isInput: true);

            // 2. Outputs - Recursive Build
            var rawOutputs = CalculationReader.GetOutputs(_currentCalculation);
            BuildTreeNodes(Outputs, rawOutputs, isInput: false);

            // 3. Details & Geometry
            RebuildCalculationDetails();
            if (_currentCalculation is IInteractiveGeometry interactiveCalc)
            {
                Geometry = new InteractiveGeometryViewModel(interactiveCalc, OnCalculationUpdate);
            }
        }

        private void BuildTreeNodes(ObservableCollection<CalcNodeViewModel> collection, List<ICalcValue> values, bool isInput)
        {
            collection.Clear();
            foreach (var item in values)
            {
                AddRecursive(collection, item, isInput);
            }
        }

        private void AddRecursive(ObservableCollection<CalcNodeViewModel> nodes, ICalcValue model, bool isInput)
        {
            // Create the VM for this value
            // We pass the NavigateTo method here so the child can request navigation
            var vm = new CalcValueViewModel(model, OnCalculationUpdate, (calc) => NavigateTo(calc));

            // 1. Find Insertion Point (Handle Headings)
            ObservableCollection<CalcNodeViewModel> currentLevel = nodes;
            if (model.Headings != null)
            {
                foreach (var heading in model.Headings)
                {
                    var group = nodes.FirstOrDefault(n => n.Name == heading && n.IsGroup);
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

            // 2. Create the Node for this Data Item
            var itemNode = new CalcNodeViewModel(vm);
            currentLevel.Add(itemNode);

            // 3. RECURSION LOGIC

            // Case A: It is a nested Calculation -> STOP recursion. 
            // The UI will show the "..." button (via IsComplex=true), but no children in the tree.
            if (model.IsICalculation)
            {
                return;
            }

            // Case B: It is a Complex Value (Structure/Class) -> Continue recursion
            if (model.IsComplexValue)
            {
                var children = isInput ? model.GetChildInputs() : model.GetChildOutputs();
                foreach (var child in children)
                {
                    AddRecursive(itemNode.Children, child, isInput);
                }
            }
            // Case C: It is a Collection -> Continue recursion
            else if (model.IsCollection)
            {
                // Access via RawValue helper
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

        // --- Helpers for Dynamic Collection Item Wrapping ---

        private ICalcValue CreateCollectionItemWrapper(IList collection, int index)
        {
            object item = collection[index];
            if (item == null) return null;

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

            // This constructor automatically detects IsICalculation, IsComplexValue etc.
            return new DelegateCalcValue<T>(getter, setter, "", name, null);
        }

        private void OnCalculationUpdate()
        {
            _currentCalculation.Calculate();
            RefreshNodes(Inputs);
            Outputs.Clear();
            var rawOutputs = CalculationReader.GetOutputs(_currentCalculation);
            BuildTreeNodes(Outputs, rawOutputs, isInput: false);
            RebuildCalculationDetails();
            Geometry?.Refresh();
        }

        private void RefreshNodes(ObservableCollection<CalcNodeViewModel> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.IsData) node.Value.Refresh();
                if (node.Children.Count > 0) RefreshNodes(node.Children);
            }
        }

        private void RebuildCalculationDetails()
        {
            CalculationDetails.Clear();
            var newItems = _currentCalculation.GetFormulae();
            if (newItems != null)
            {
                foreach (var item in newItems)
                {
                    CalculationDetails.Add(new OutputItemViewModel(item));
                }
            }
        }
    }
}
