using Scaffold.Core.Abstract;
using System.Collections.Generic;
using System;

namespace Scaffold.Core.CalcValues;

public class CalcSelectionList : CalcValue<string>, ISelectionList
{
    public List<string> Selections { get; private set; }

    public int SelectedItemIndex { get; set; }

    public override string Value
    {
        get { return Selections[SelectedItemIndex]; }
        set { TryParse(value); }
    }

    public CalcSelectionList(string name, int selectedItemIndex, IEnumerable<string> values)
        : base(name, string.Empty, string.Empty)
    {
        Selections = values.ToList();
    }
    public CalcSelectionList(string name, string selectedItem, IEnumerable<string> values)
    : base(name, string.Empty, string.Empty)
    {
        Selections = values.ToList();
        TryParse(selectedItem);
    }

    public override bool TryParse(string strValue)
    {
        int i = Selections.IndexOf(strValue);

        if (i == -1) { return false; }
        else { SelectedItemIndex = i; return true; }

    }

    public override string ToString() => string.Join(", ", Selections);
}
