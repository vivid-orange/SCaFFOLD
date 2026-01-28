using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Scaffold.Geometry;
using Scaffold.Reader;
using Scaffold.Report;

namespace Scaffold.Desktop;

public class CalculationViewModel : ViewModelBase
{
    private ICalculation _currentCalculation;
    private readonly Stack<ICalculation> _navigationStack = new Stack<ICalculation>();

    public ObservableCollection<ICalculation> Breadcrumbs { get; } = [];

    // CHANGED: Now collections of Nodes (Tree Roots)
    public ObservableCollection<CalcNodeViewModel> Inputs { get; } = [];
    public ObservableCollection<CalcNodeViewModel> Outputs { get; } = [];
    public ObservableCollection<OutputItemViewModel> CalculationDetails { get; } = [];

    public string CurrentTitle => _currentCalculation?.CalculationTitle;
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
        // Geometry = ... (Reset geometry logic)

        if (_currentCalculation == null)
        {
            return;
        }

        // 1. Inputs - Recursive Build
        List<ICalcValue> rawInputs = CalculationReader.GetInputs(_currentCalculation);
        BuildTreeNodes(Inputs, rawInputs, isInput: true);

        // 2. Outputs - Recursive Build
        List<ICalcValue> rawOutputs = CalculationReader.GetOutputs(_currentCalculation);
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
        foreach (ICalcValue item in values)
        {
            AddRecursive(collection, item, isInput);
        }
    }

    private void AddRecursive(ObservableCollection<CalcNodeViewModel> nodes, ICalcValue model, bool isInput)
    {
        // Create the VM for this value
        var vm = new CalcValueViewModel(model, OnCalculationUpdate);

        // 1. Find Insertion Point (Handle Headings)
        ObservableCollection<CalcNodeViewModel> currentLevel = nodes;
        if (model.Headings != null)
        {
            foreach (string? heading in model.Headings)
            {
                CalcNodeViewModel? group = nodes.FirstOrDefault(n => n.Name == heading && n.IsGroup);

                // If searching inside a previous group, look in its children
                if (group != null)
                {
                    // Found existing group at this level
                    currentLevel = group.Children;
                }
                else
                {
                    // Create new group
                    var newGroup = new CalcNodeViewModel(heading);
                    currentLevel.Add(newGroup);
                    currentLevel = newGroup.Children;
                }
            }
        }

        // 2. Create the Node for this Data Item
        var itemNode = new CalcNodeViewModel(vm);
        currentLevel.Add(itemNode);

        // 3. RECURSION: Check if this item has children (Complex or Calculation)
        if (vm.IsComplex)
        {
            // Drill down: Get children using the new ICalcValue methods
            List<ICalcValue> children = isInput ? model.GetChildInputs() : model.GetChildOutputs();

            foreach (ICalcValue? child in children)
            {
                AddRecursive(itemNode.Children, child, isInput);
            }
        }
        // 4. RECURSION: Handle Collections
        else if (vm.IsCollection)
        {
            // Iterate the collection items
            // Note: CalcValueViewModel.RawValue needs to act as the bridge here
            if (vm.RawValue is IEnumerable collection)
            {
                int index = 0;
                foreach (object? obj in collection)
                {
                    if (obj == null)
                    {
                        continue;
                    }

                    // Create a "Folder" node for the item (e.g. "[0] Beam")
                    string label = $"[{index}] {obj.GetType().Name}";
                    var arrayNode = new CalcNodeViewModel(label);
                    itemNode.Children.Add(arrayNode);

                    // Scan the item for inputs/outputs
                    List<ICalcValue> itemProps = isInput ? ObjectReader.GetInputs(obj) : ObjectReader.GetOutputs(obj);

                    foreach (ICalcValue? prop in itemProps)
                    {
                        AddRecursive(arrayNode.Children, prop, isInput);
                    }
                    index++;
                }
            }
        }
    }

    private void OnCalculationUpdate()
    {
        _currentCalculation.Calculate();

        // Refresh Values (Recursive)
        RefreshNodes(Inputs);

        // Rebuild Outputs (Structure might change)
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
