using System.Collections.Generic;
using System;

namespace Scaffold.Core;

public class CalcSelectionList : ICalcSelectionList
{
    public List<string> Selections { get; private set; }

    public int SelectedItemIndex { get; set; }

    public string Value
    {
        get { return Selections[SelectedItemIndex]; }
        set { TryParse(value); }
    }

    public CalcSelectionList(int selectedItemIndex, IEnumerable<string> values)
    {
        SelectedItemIndex = selectedItemIndex;
        Selections = values.ToList();
    }
    //public CalcSelectionList(string name, string selectedItem, IEnumerable<string> values)
    //: base(name, string.Empty, string.Empty)
    //{
    //    Selections = values.ToList();
    //    TryParse(selectedItem);
    //}

    public bool TryParse(string strValue)
    {
        int i = Selections.IndexOf(strValue);

        if (i == -1) { return false; }
        else { SelectedItemIndex = i; return true; }

    }

    public string ValueAsString() => string.Join(", ", Selections);

    public override string ToString() => string.Join(", ", Selections);
}
