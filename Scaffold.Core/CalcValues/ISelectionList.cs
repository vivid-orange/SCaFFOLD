using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Core.CalcValues
{
    public interface ISelectionList : ICalcValue
    {
        List<string> Selections { get; }

        int SelectedItemIndex { get; set; }

        string Value { get; }

    }
}
