using System.Globalization;
using Scaffold.Core.Abstract;
using Scaffold.Core.Enums;
using Scaffold.Core.CalcValues;

namespace Scaffold.Core.CalcValues;

public sealed class CalcValueDouble(double value) : IQuantity
{
    public string Unit { get; }
    public string DisplayName { get; }
    public double Value { get; set; } = value;
    public string TypeName { get; set; }
    public string Symbol { get; set; }
    public CalcStatus Status { get; set; }

    public string GetValueAsString()
    {
        return Value.ToString();
    }

    public bool TryParse(string strValue)
    {
        if (double.TryParse(strValue, out double result))
        {
            Value = result;
            return true;
        }
        else
        {
            return false;
        }
    }
}
