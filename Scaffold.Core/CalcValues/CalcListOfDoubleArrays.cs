using Scaffold.Core.Abstract;
using Scaffold.Core.Enums;
using Scaffold.Core.CalcValues;
using System.Text;

// TODO: output is not currently formatting.
namespace Scaffold.Core.CalcValues;

public class CalcListOfDoubleArrays : ICalcListOfDoubleArrays
{
    public List<double[]> Value { get; private set; }   // TODO: Default value from type param.
    public void ResetArray() => Value.Clear();

    public string TypeName { get; } = "";

    public string InstanceName { get; private set; } = "";

    public CalcListOfDoubleArrays(string name, string symbol, List<double[]> multiDimensionalArray)
    {
        Value = multiDimensionalArray ?? [];
        InstanceName = name;
        Symbol = symbol;
    }


    //public override void SetValue(string strValue)
    //{
    //    var valueList = new List<double[]>();

    //    if (!strValue.StartsWith("{{") || !strValue.EndsWith("}}"))
    //    {
    //        valueList.Add(new[] { double.NaN });
    //        return;
    //    }

    //    var parts = strValue.Split(new string[] { "}{" }, StringSplitOptions.None);
    //    parts[0] = parts.First().Remove(0, 2);
    //    parts[parts.Count() - 1] = parts.Last().Remove(parts.Last().Length - 2, 2);

    //    foreach (var part in parts)
    //    {
    //        var entries = part.Split(',');
    //        var lineEntries = new double[entries.Length];
    //        for (var j = 0; j < entries.Length; j++)
    //        {
    //            var entry = entries[j];
    //            double.TryParse(entry, out var result);
    //            lineEntries[j] = result;
    //        }
    //        valueList.Add(lineEntries);
    //    }

    //    Value = valueList;
    //}

    public bool TryParse(string value)
    {
        return false;
    }

    public string GetValueAsString()
    {
        var innerStr = new StringBuilder();
        foreach (var item in Value)
        {
            innerStr.Append('{');

            foreach (var item2 in item)
            {
                innerStr.Append($"{item2},");
            }

            innerStr.Append('}');
        }

        var final = new StringBuilder();
        final.Append('{').Append(innerStr).Append('}');

        return final.ToString();
    }

    public string Symbol { get; private set; }
    public CalcStatus Status { get;  }

}
