using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCaFFOLD_Desktop
{
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
}
