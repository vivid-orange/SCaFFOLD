namespace Scaffold;

public interface ICalcValue : ICalcParameter, IFormattable
{
    bool TryParse(string strValue);
    CalcStatus Status { get; }
    List<string> Headings { get; }
}

/// <summary>
/// Marker interface for int primitive values.
/// </summary>
public interface IIntValue : ICalcValue
{
}

/// <summary>
/// Marker interface for double primitive values.
/// </summary>
public interface IDoubleValue : ICalcValue
{
}

/// <summary>
/// Marker interface for string primitive values.
/// </summary>
public interface IStringValue : ICalcValue
{
}

/// <summary>
/// Marker interface for bool primitive values.
/// </summary>
public interface IBoolValue : ICalcValue
{
}

/// <summary>
/// Marker interface for UnitsNet quantity values (Length, Force, Area, etc.).
/// Inherits from IDoubleValue as quantities are backed by double values.
/// </summary>
public interface IQuantityValue : IDoubleValue, IStringValue
{
    /// <summary>
    /// Gets the unit of the quantity.
    /// </summary>
    Enum Unit { get; }
}

/// <summary>
/// Marker interface for complex objects with [CalcParameter]-decorated child properties.
/// Child traversal is provided by the reader via GetInputs/GetOutputs methods.
/// </summary>
public interface IComplexValue : ICalcValue
{
}

/// <summary>
/// Marker interface for values wrapping an ICalculation.
/// Child traversal is provided by the reader via GetInputs/GetOutputs methods.
/// </summary>
public interface ICalculationValue : ICalcValue
{
}

/// <summary>
/// Marker interface for collection types (List, Array, etc. but excluding string).
/// </summary>
public interface ICollectionValue : ICalcValue
{
    int Count { get; }
}
