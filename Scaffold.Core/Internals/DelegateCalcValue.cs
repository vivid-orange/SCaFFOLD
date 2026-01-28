using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Scaffold.Core
{
    public class DelegateCalcValue<T> : ICalcValue
    {
        private readonly Func<T> _getter;
        private readonly Action<T> _setter;

        public string Symbol { get; }
        public string EntityLabel { get; }
        public List<string> Headings { get; }

        public CalcStatus Status { get; set; } = CalcStatus.None;

        // --- New Flags ---
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
            // True if the type T has any public properties tagged with [InputCalcValue] or [OutputCalcValue]
            // We cache this check per type T to avoid reflecting every constructor call
            IsComplexValue = CheckIfComplex(typeof(T));
        }

        private static bool CheckIfComplex(Type type)
        {
            // Simple string/value types are not complex in this context
            if (type == typeof(string) || type.IsValueType)
            {
                // Edge case: Structs with attributes are complex
                if (type.IsPrimitive) return false;
            }

            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                       .Any(p => Attribute.IsDefined(p, typeof(CalcValueTypeAttribute)));
        }

        // --- Object Reader Integration ---

        /// <summary>
        /// Retrieves child input values if this is a Complex Value or ICalculation.
        /// </summary>
        public List<ICalcValue> GetChildInputs()
        {
            object val = Value;
            if (val == null) return new List<ICalcValue>();

            // Use the ObjectReader to scan the current value instance
            return ObjectReader.GetInputs(val);
        }

        /// <summary>
        /// Retrieves child output values if this is a Complex Value or ICalculation.
        /// </summary>
        public List<ICalcValue> GetChildOutputs()
        {
            object val = Value;
            if (val == null) return new List<ICalcValue>();

            return ObjectReader.GetOutputs(val);
        }

        // --- Existing Implementation ---

        public string ValueAsString()
        {
            var val = _getter();

            if (val is List<double[]> list)
            {
                return $"List<double[]> ({list.Count} items)";
            }
            if (IsCollection && val is ICollection collection)
            {
                return $"{typeof(T).Name} ({collection.Count} items)";
            }

            return val?.ToString() ?? string.Empty;
        }

        public bool TryParse(string strValue)
        {
            if (_setter == null) return false;

            if (Value is IQuantity)
            {
                try
                {
                    IQuantity quantity = UnitsNet.Quantity.Parse(CultureInfo.InvariantCulture, ((IQuantity)Value).QuantityInfo.ValueType, strValue);
                    Value = (T)quantity;
                    return true;
                }
                catch { }

                if (double.TryParse(strValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                {
                    Value = (T)UnitsNet.Quantity.From(val, ((IQuantity)Value).Unit);
                    return true;
                }
                return false;
            }

            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null && converter.CanConvertFrom(typeof(string)))
                {
                    T result = (T)converter.ConvertFrom(strValue);
                    _setter(result);
                    return true;
                }
            }
            catch
            {
                // Conversion failed
            }
            return false;
        }

        public T Value
        {
            get => _getter();
            set => _setter?.Invoke(value);
        }
    }
}
