using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Scaffold.Core;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Geometry;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Services;

namespace SCaFFOLD_Desktop
{
    public class CalculationViewModel : ViewModelBase
    {
        private ICalculation _currentCalculation;
        private readonly Stack<ICalculation> _navigationStack = new Stack<ICalculation>();

        public ObservableCollection<ICalculation> Breadcrumbs { get; } = [];

        // CHANGED: Now collections of Nodes (Tree Roots)
        public ObservableCollection<CalcNodeViewModel> Inputs { get; } = [];
        public ObservableCollection<CalcNodeViewModel> Outputs { get; } = [];

        public ObservableCollection<OutputItemViewModel> CalculationDetails { get; } = [];

        public string CurrentTitle => _currentCalculation?.TypeName;
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
            Geometry = null;

            if (_currentCalculation == null) return;

            // 1. Inputs - Build Tree
            var rawInputs = CalculationReader.GetInputs(_currentCalculation);
            var inputVMs = rawInputs.Select(i => new CalcValueViewModel(i, OnCalculationUpdate)).ToList();
            BuildTree(Inputs, inputVMs);

            // 2. Outputs - Build Tree
            var rawOutputs = CalculationReader.GetOutputs(_currentCalculation);
            var outputVMs = rawOutputs.Select(o => new CalcValueViewModel(o, null)).ToList();
            BuildTree(Outputs, outputVMs);

            // 3. Details
            RebuildCalculationDetails();

            // 4. Geometry
            if (_currentCalculation is IInteractiveGeometry interactiveCalc)
            {
                Geometry = new InteractiveGeometryViewModel(interactiveCalc, OnCalculationUpdate);
            }
        }

        // NEW: Tree Building Logic
        private void BuildTree(ObservableCollection<CalcNodeViewModel> roots, List<CalcValueViewModel> items)
        {
            roots.Clear();

            foreach (var item in items)
            {
                // Access Headings from the Model (via ICalcValue interface update)
                var headings = item.Model.Headings;

                ObservableCollection<CalcNodeViewModel> currentLevel = roots;

                // Traverse/Create Groups
                if (headings != null)
                {
                    foreach (var heading in headings)
                    {
                        var groupNode = currentLevel.FirstOrDefault(n => n.Name == heading && !n.IsLeaf);
                        if (groupNode == null)
                        {
                            groupNode = new CalcNodeViewModel(heading);
                            currentLevel.Add(groupNode);
                        }
                        currentLevel = groupNode.Children;
                    }
                }

                // Add Leaf
                currentLevel.Add(new CalcNodeViewModel(item));
            }
        }

        private void OnCalculationUpdate()
        {
            _currentCalculation.Calculate();

            // Refresh Inputs (Leaf Nodes only)
            RefreshLeaves(Inputs);

            // Refresh Outputs (Rebuild Tree as values/structure might change)
            // Ideally we just refresh values, but if structure is dynamic, we rebuild.
            Outputs.Clear();
            var rawOutputs = CalculationReader.GetOutputs(_currentCalculation);
            var outputVMs = rawOutputs.Select(o => new CalcValueViewModel(o, null)).ToList();
            BuildTree(Outputs, outputVMs);

            RebuildCalculationDetails();
            Geometry?.Refresh();
        }

        // Recursive helper to refresh leaf values
        private void RefreshLeaves(ObservableCollection<CalcNodeViewModel> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.IsLeaf)
                {
                    node.Value.Refresh();
                }
                else
                {
                    RefreshLeaves(node.Children);
                }
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
