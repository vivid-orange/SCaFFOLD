using System.Collections.ObjectModel;
using System.Linq;

namespace SCaFFOLD_Desktop
{
    public class CalcNodeViewModel : ViewModelBase
    {
        // For Group Nodes
        public string Name { get; }
        public ObservableCollection<CalcNodeViewModel> Children { get; } = new ObservableCollection<CalcNodeViewModel>();

        // For Leaf Nodes
        public CalcValueViewModel Value { get; }

        // Flags
        public bool IsLeaf => Value != null;
        public bool IsExpanded { get; set; } = true; // Default groups to open

        // Constructor for Group
        public CalcNodeViewModel(string name)
        {
            Name = name;
            Value = null;
        }

        // Constructor for Leaf
        public CalcNodeViewModel(CalcValueViewModel value)
        {
            Name = value.DisplayName; // Or Symbol, depending on preference
            Value = value;
        }

        /// <summary>
        /// Helper to find or create a child group node
        /// </summary>
        public CalcNodeViewModel GetOrCreateChild(string name)
        {
            var child = Children.FirstOrDefault(c => c.Name == name && !c.IsLeaf);
            if (child == null)
            {
                child = new CalcNodeViewModel(name);
                Children.Add(child);
            }
            return child;
        }
    }
}
