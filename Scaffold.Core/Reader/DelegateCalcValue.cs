using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Scaffold.Reader;

internal class DelegateCalcValue<T> : ICalcValue<T>
{
    private readonly Func<T> _getter;
    private readonly Action<T> _setter;

    public string Symbol { get; }
    public string EntityLabel { get; }
    public List<string> Headings { get; }

    public CalcStatus Status { get; set; } = CalcStatus.None;

    // --- Type Flags ---
    public bool IsICalculation { get; }
    public bool IsCollection { get; }
    public bool IsComplexValue { get; }

    public DelegateCalcValue(
        Func<T> getter,
        Action<T> setter,
        string symbol,
        string displayName,
        IEnumerable<string> headings)
    {
        _getter = getter;
        _setter = setter;
        Symbol = symbol;
        EntityLabel = displayName ?? typeof(T).Name;
        Headings = headings != null ? new List<string>(headings) : new List<string>();

        // 1. Check for ICalculation
        IsICalculation = typeof(ICalculation).IsAssignableFrom(typeof(T));

        // 2. Check for ICollection (excluding strings)
        IsCollection = typeof(ICollection).IsAssignableFrom(typeof(T)) && typeof(T) != typeof(string);

        // 3. Check for Complex Value
        // True if the type T has any public properties tagged with [CalcParameterAttribute]
        // We cache this check per type T to avoid reflecting every constructor call
        IsComplexValue = CheckIfComplex(typeof(T));
    }

    private static bool CheckIfComplex(Type type)
    {
        // Simple string/value types are not complex in this context
        if (type == typeof(string) || type.IsValueType)
        {
            // Edge case: Structs with attributes are complex
            if (type.IsPrimitive)
            {
                return false;
            }
        }

        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   .Any(p => Attribute.IsDefined(p, typeof(CalcParameterAttribute)));
    }

    // --- Object Reader Integration ---

    /// <summary>
    /// Retrieves child input values if this is a Complex Value or ICalculation.
    /// </summary>
    public List<ICalcValue> GetChildInputs()
    {
        object val = Value;
        if (val == null)
        {
            return new List<ICalcValue>();
        }

        // Use the CalculationReader to scan the current value instance
        return CalculationReader.GetInputs(val);
    }

    /// <summary>
    /// Retrieves child output values if this is a Complex Value or ICalculation.
    /// </summary>
    public List<ICalcValue> GetChildOutputs()
    {
        object val = Value;
        if (val == null)
        {
            return new List<ICalcValue>();
        }

        return CalculationReader.GetOutputs(val);
    }

    // --- IFormattable ---

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        T val = _getter();

        if (IsCollection && val is ICollection collection)
            return $"{typeof(T).Name} ({collection.Count} items)";

        if (val is IFormattable f)
            return f.ToString(format, formatProvider);

        return val?.ToString() ?? string.Empty;
    }

    public override string ToString() => ToString(null, CultureInfo.InvariantCulture);

    // --- TryParse ---

    public bool TryParse(string strValue)
    {
        if (_setter == null)
            return false;

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

    public T Value
    {
        get => _getter();
        set => _setter?.Invoke(value);
    }

    public object ValueAsObject
    {
        get => (object)_getter();
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
