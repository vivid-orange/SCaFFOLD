using Scaffold.Core.Abstract;
using Scaffold.Core.Enums;
using Scaffold.Core.CalcValues;
using System.Reflection.Metadata.Ecma335;
using System.ComponentModel;

namespace Scaffold.Core.CalcValues;

public class SelectionList : ISelectionList
{
    private readonly List<string> _selectionList;

    public SelectionList(string name, int selectedItemIndex, IEnumerable<string> values)
    {
        _selectionList = values.ToList();
        TypeName = name;
        SelectedItemIndex = selectedItemIndex;
    }

    public string DisplayName { get; } = "";

    public int SelectedItemIndex { get; set; }


    public IReadOnlyList<string> Selections => _selectionList;
    public string Value
    {
        get
        {
            return _selectionList[SelectedItemIndex];
        }
    }

    public string TypeName { get; }

    public string Symbol => "";

    public CalcStatus Status => CalcStatus.None;

    public string GetValue()
    {
        return Value;
    }

    public bool SetValue(string strValue)
    {
        return false;
        // need to add logic here. should this use index or match string value???
    }
}
