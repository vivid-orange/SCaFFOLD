using Scaffold.Core;
using SCaFFOLD_Desktop;
using System.Collections.ObjectModel;
using Scaffold.Core.Interfaces;
using System.Linq;
using System.Windows.Media;

namespace SCaFFOLD_Desktop
{
    // New helper class for the View
    public class ExpressionRowViewModel : ViewModelBase
    {
        public ObservableCollection<SCaFFOLD_Desktop.ExpressionViewModel> Items { get; }
            = [];
    }

    public class OutputItemViewModel : ViewModelBase
    {
        private readonly IOutputItem _model;

        public OutputItemViewModel(IOutputItem model)
        {
            _model = model;

            if (_model.Expressions != null)
            {
                foreach (var expr in _model.Expressions)
                {
                    var vm = new SCaFFOLD_Desktop.ExpressionViewModel(expr);

                    // Logic: If item is NOT inline, OR if we have no rows yet, start a new row.
                    // Otherwise, add to the existing row (inline flow).
                    if (!vm.IsInLine || Rows.Count == 0)
                    {
                        var newRow = new ExpressionRowViewModel();
                        newRow.Items.Add(vm);
                        Rows.Add(newRow);
                    }
                    else
                    {
                        Rows.Last().Items.Add(vm);
                    }
                }
            }
        }

        public string Conclusion => _model.Conclusion;

        // We now expose a list of ROWS, not a flat list of expressions
        public ObservableCollection<ExpressionRowViewModel> Rows { get; }
            = [];
    }
}
