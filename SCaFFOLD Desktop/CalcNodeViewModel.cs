using System.Collections.ObjectModel;

namespace Scaffold.Desktop;

public class CalcNodeViewModel : ViewModelBase
{
    // 1. Identification
    public string Name { get; }

    // 2. The Data (Null for pure Folder/Heading nodes)
    public CalcValueViewModel Value { get; }

    // 3. The Hierarchy
    public ObservableCollection<CalcNodeViewModel> Children { get; } = new ObservableCollection<CalcNodeViewModel>();

    // --- FLAGS ---
    // Replaces IsLeaf. True if this node wraps an actual Input/Output object.
    public bool IsData => Value != null;

    // True if this is just a heading/folder
    public bool IsGroup => Value == null;

    public bool IsExpanded { get; set; } = false;

    // Constructor for Group/Heading
    public CalcNodeViewModel(string name)
    {
        Name = name;
        Value = null;
    }

    // Constructor for Data Object
    public CalcNodeViewModel(CalcValueViewModel value)
    {
        // Use DisplayName from the value, or fallback to Symbol if needed
        Name = value.DisplayName;
        Value = value;
    }

    // Helper to find existing group nodes (e.g. "Geometry")
    public CalcNodeViewModel GetOrCreateChildGroup(string name)
    {
        CalcNodeViewModel? child = Children.FirstOrDefault(c => c.Name == name && c.IsGroup);
        if (child == null)
        {
            child = new CalcNodeViewModel(name);
            Children.Add(child);
        }
        return child;
    }
}
