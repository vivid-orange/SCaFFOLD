using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Scaffold.Reader;

/// <summary>
/// Base implementation for wrapping property values with delegate accessors.
/// Implements the appropriate marker interfaces based on the type T.
/// </summary>
public abstract class DelegateCalcValue : ICalcValue
{
    protected readonly Func<object?> _getter;
    protected readonly Action<object?>? _setter;

    public string Symbol { get; }
    public string EntityLabel { get; }
    public List<string> Headings { get; }
    public CalcStatus Status { get; set; } = CalcStatus.None;

    protected DelegateCalcValue(
        Func<object?> getter,
        Action<object?>? setter,
        string symbol,
        string displayName,
        IEnumerable<string>? headings)
    {
        _getter = getter;
        _setter = setter;
        Symbol = symbol;
        EntityLabel = displayName ?? "Value";
        Headings = headings != null ? new List<string>(headings) : new List<string>();
    }

    public abstract bool TryParse(string strValue);

    public virtual string ToString(string? format, IFormatProvider? formatProvider)
    {
        object? val = _getter();

        if (val is ICollection collection && val is not string)
        {
            return $"{val.GetType().Name} ({collection.Count} items)";
        }

        if (val is IFormattable f)
        {
            return f.ToString(format, formatProvider);
        }

        return val?.ToString() ?? string.Empty;
    }

    public override string ToString() => ToString(null, CultureInfo.InvariantCulture);
}

/// <summary>
/// Generic wrapper for standard/leaf values (primitives, strings, etc.)
/// This is the generic fallback for types that don't fit other categories.
/// </summary>
public class DelegateCalcValue<T> : DelegateCalcValue, ICalcValue
{
    private readonly Func<T> _typedGetter;
    private readonly Action<T>? _typedSetter;

    public DelegateCalcValue(
        Func<T> getter,
        Action<T>? setter,
        string symbol,
        string displayName,
        IEnumerable<string>? headings)
        : base(() => getter(), setter != null ? (v => setter((T)v)) : null, symbol, displayName, headings)
    {
        _typedGetter = getter;
        _typedSetter = setter;
    }

    public T Value
    {
        get => _typedGetter();
        set => _typedSetter?.Invoke(value);
    }

    public override bool TryParse(string strValue)
    {
        if (_typedSetter == null)
        {
            return false;
        }

        // IQuantity path (UnitsNet)
        if (Value is IQuantity currentQuantity)
        {
            try
            {
                IQuantity parsed = UnitsNet.Quantity.Parse(
                    CultureInfo.InvariantCulture,
                    currentQuantity.QuantityInfo.ValueType,
                    strValue);
                Value = (T)parsed;
                return true;
            }
            catch (Exception ex) when (ex is FormatException or UnitsNet.UnitsNetException) { }

            // Bare number — preserve current unit
            if (double.TryParse(strValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double numVal))
            {
                Value = (T)UnitsNet.Quantity.From(numVal, currentQuantity.Unit);
                return true;
            }

            return false;
        }

        // IParsable<T> path (cached static check)
        if (ParsableHelper<T>.IsParsable)
        {
            if (ParsableHelper<T>.TryParse(strValue, CultureInfo.InvariantCulture, out T? result))
            {
                Value = result!;
                return true;
            }
            return false;
        }

        // TypeConverter fallback
        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        if (converter.CanConvertFrom(typeof(string)))
        {
            try
            {
                object? converted = converter.ConvertFromString(null, CultureInfo.InvariantCulture, strValue);
                if (converted is T typedResult)
                {
                    Value = typedResult;
                    return true;
                }
            }
            catch (Exception) { /* converter threw — not parsable */ }
        }

        return false;
    }

    // --- IParsable<T> Helper ---

    private static class ParsableHelper<TParsable>
    {
        public static readonly bool IsParsable;
        private static readonly TryParseDelegate? _tryParse;

        private delegate bool TryParseDelegate(string? s, IFormatProvider? provider, out TParsable? result);

        static ParsableHelper()
        {
            Type? iface = typeof(TParsable).GetInterface($"System.IParsable`1[{typeof(TParsable).FullName}]");
            if (iface == null) { IsParsable = false; return; }

            MethodInfo? method = typeof(TParsable).GetMethod("TryParse",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(string), typeof(IFormatProvider), typeof(TParsable).MakeByRefType() },
                null);

            if (method != null)
            {
                _tryParse = (TryParseDelegate)Delegate.CreateDelegate(typeof(TryParseDelegate), method);
                IsParsable = true;
            }
        }

        public static bool TryParse(string? s, IFormatProvider? provider, out TParsable? result)
        {
            if (_tryParse != null)
            {
                return _tryParse(s, provider, out result);
            }

            result = default;
            return false;
        }
    }
}

/// <summary>
/// Wrapper for UnitsNet quantity values (Length, Force, Area, etc.)
/// Implements IQuantityValue which inherits from IDoubleValue.
/// </summary>
public class QuantityCalcValue<T> : DelegateCalcValue<T>, IQuantityValue where T : IQuantity
{
    private T _lastValue;

    public QuantityCalcValue(
        Func<T> getter,
        Action<T>? setter,
        string symbol,
        string displayName,
        IEnumerable<string>? headings)
        : base(getter, setter, symbol, displayName, headings)
    {
        _lastValue = getter();
    }

    /// <summary>
    /// Gets the unit of the underlying quantity.
    /// </summary>
    public Enum Unit
    {
        get
        {
            T val = Value;
            return val?.Unit as Enum ?? _lastValue.Unit as Enum;
        }
    }
}

/// <summary>
/// Wrapper for complex objects with [CalcParameter]-decorated child properties.
/// Marker interface only - child traversal handled by CalculationReader.
/// </summary>
public class ComplexCalcValue<T> : DelegateCalcValue<T>, IComplexValue
{
    public ComplexCalcValue(
        Func<T> getter,
        Action<T>? setter,
        string symbol,
        string displayName,
        IEnumerable<string>? headings)
        : base(getter, setter, symbol, displayName, headings)
    {
    }
}

/// <summary>
/// Wrapper for collection values (List, Array, etc. excluding string).
/// </summary>
public class CollectionCalcValue<T> : DelegateCalcValue<T>, ICollectionValue where T : ICollection
{
    public CollectionCalcValue(
        Func<T> getter,
        Action<T>? setter,
        string symbol,
        string displayName,
        IEnumerable<string>? headings)
        : base(getter, setter, symbol, displayName, headings)
    {
    }

    public int Count => Value?.Count ?? 0;
}

/// <summary>
/// Wrapper for ICalculation values.
/// Marker interface only - child traversal handled by CalculationReader.
/// </summary>
public class CalculationCalcValue<T> : DelegateCalcValue<T>, ICalculationValue where T : ICalculation
{
    public CalculationCalcValue(
        Func<T> getter,
        Action<T>? setter,
        string symbol,
        string displayName,
        IEnumerable<string>? headings)
        : base(getter, setter, symbol, displayName, headings)
    {
    }
}

/// <summary>
/// Wrapper for int primitive values.
/// </summary>
public class IntCalcValue : DelegateCalcValue<int>, IIntValue
{
    public IntCalcValue(
        Func<int> getter,
        Action<int>? setter,
        string symbol,
        string displayName,
        IEnumerable<string>? headings)
        : base(getter, setter, symbol, displayName, headings)
    {
    }
}

/// <summary>
/// Wrapper for double primitive values.
/// </summary>
public class DoubleCalcValue : DelegateCalcValue<double>, IDoubleValue
{
    public DoubleCalcValue(
        Func<double> getter,
        Action<double>? setter,
        string symbol,
        string displayName,
        IEnumerable<string>? headings)
        : base(getter, setter, symbol, displayName, headings)
    {
    }
}

/// <summary>
/// Wrapper for string primitive values.
/// </summary>
public class StringCalcValue : DelegateCalcValue<string>, IStringValue
{
    public StringCalcValue(
        Func<string> getter,
        Action<string>? setter,
        string symbol,
        string displayName,
        IEnumerable<string>? headings)
        : base(getter, setter, symbol, displayName, headings)
    {
    }
}

/// <summary>
/// Wrapper for bool primitive values.
/// </summary>
public class BoolCalcValue : DelegateCalcValue<bool>, IBoolValue
{
    public BoolCalcValue(
        Func<bool> getter,
        Action<bool>? setter,
        string symbol,
        string displayName,
        IEnumerable<string>? headings)
        : base(getter, setter, symbol, displayName, headings)
    {
    }
}
