using UnitsNet;
using Scaffold.Core.Enums;
using Scaffold.Core.CalcValues;
using System.Globalization;

namespace Scaffold.Core.CalcValues;

public class SIQuantity<T> : ISIQuantity<T> where T : UnitsNet.IQuantity
{
    private T _quantity;
    private string _displayName;
    private string _symbol;

    public SIQuantity(string name, string symbol, T quantity)
    {
        _quantity = quantity;
        _displayName = name;
        _symbol = symbol;
    }

    public string DisplayName { get { return _displayName; } }

    public CalcStatus Status { get; }

    public string TypeName
    {
        get => _displayName;
    }

    public string Symbol
    {
        get => _symbol;
    }

    public T Quantity
    {
        get => _quantity;
        set => _quantity = value;
    }

    public UnitsNet.IQuantity GenericQuantity
    {
        get => _quantity;
    }


    string IQuantity.Unit
    {
        get
        {
            return UnitAbbreviationsCache.Default.GetDefaultAbbreviation(_quantity.Unit.GetType(), Convert.ToInt32(_quantity.Unit));
        }
    }

    public double Value
    {
        get => (Double)_quantity.Value;
        set => _quantity = (T)UnitsNet.Quantity.From(value, _quantity.Unit);
    }

    public string GetValueAsString()
        => _quantity.Value.ToString(CultureInfo.InvariantCulture);

    bool ICalcValue.TryParse(string strValue)
    {
        _quantity = (T)UnitsNet.Quantity.From(double.TryParse(strValue, out var convertedValue)
            ? convertedValue
            : double.NaN, _quantity.Unit);
        return true;
    }
}
