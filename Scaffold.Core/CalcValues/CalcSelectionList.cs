using Scaffold.Core.Abstract;

namespace Scaffold.Core.CalcValues;

public class CalcSelectionList : CalcValue<string>
{
    public List<string> SelectionList { get; private set; }

    public CalcSelectionList(string name, string selectedValue, IEnumerable<string> values)
        : base(selectedValue, name, string.Empty, string.Empty)
    {
        SelectionList = values.ToList();
        string selectedItem = SelectionList.FirstOrDefault(x => x == selectedValue);
        Value = selectedItem == null ? SelectionList[0] : selectedValue;
    }

    public override bool SetValue(string strValue)
    {
        string exists = SelectionList.FirstOrDefault(x => x == strValue);
        if (exists == null)
        {
            return false;
        }

        Value = strValue;
        return true;
    }

    public override string ToString() => string.Join(", ", SelectionList);
}
