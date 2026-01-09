using System.ComponentModel;
using System.Reflection;
using Scaffold.Core.CalcValues;

namespace Scaffold.Core.Internals;

internal sealed class InternalCalcValue : ICalcValue
{
    private ICalculation Calculation { get; }
    private Type ValueType { get; }
    private string MemberName { get; }

    public string DisplayName { get; set; }
    public string Symbol { get; set; }
    public string UnitName => Unit?.QuantityInfo.Name ?? "";
    public CalcStatus Status { get; set; }

    internal InternalCalcValue(ICalculation calculation, Type valueType, string memberName)
    {
        Calculation = calculation;
        ValueType = valueType;
        MemberName = memberName;
    }

    internal object Value
    {
        get
        {
            PropertyInfo member = Calculation.GetType().GetProperty(MemberName);
            return member?.GetValue(Calculation);
        }
    }

    internal UnitsNet.IQuantity Unit { get; set; }

    private object Convert(string input)
    {
        TypeConverter converter = TypeDescriptor.GetConverter(ValueType);
        return converter.ConvertFromString(input);
    }

    public bool SetValue(string strValue)
    {
        object newValue = Convert(strValue);
        PropertyInfo member = Calculation.GetType().GetProperty(MemberName);
        if (member != null)
        {
            member?.SetValue(Calculation, newValue);
        }

        return member != null;
    }

    public string GetValue() => Value.ToString();
}
