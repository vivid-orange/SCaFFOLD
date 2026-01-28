using System.Collections.ObjectModel;

namespace Scaffold.Desktop;

public class ArrayRowViewModel : ViewModelBase
{
    public ObservableCollection<ArrayCellViewModel> Cells { get; } = [];

    public ArrayRowViewModel(double[] rowData, Action onValueChanged)
    {
        for (int i = 0; i < rowData.Length; i++)
        {
            Cells.Add(new ArrayCellViewModel(rowData, i, onValueChanged));
        }
    }
}
