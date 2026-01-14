using System;
using System.Collections.Generic;
using System.ComponentModel;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;

namespace Scaffold.Core.Models
{
    /// <summary>
    /// A generic wrapper that delegates property access to compiled functions.
    /// </summary>
    public class DelegateCalcValue<T> : ICalcValue
    {
        private readonly Func<T> _getter;
        private readonly Action<T> _setter;

        public string Symbol { get; }
        public string TypeName { get; }
        public List<string> Headings { get; }

        // ICalculationStatus implementation
        public string DisplayName { get; } // Maps to ICalculationStatus.TypeName usually, but here acting as Name
        string ICalculationStatus.TypeName => DisplayName;
        public CalcStatus Status { get; set; } = CalcStatus.None;

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
            DisplayName = displayName ?? typeof(T).Name;
            Headings = headings != null ? new List<string>(headings) : new List<string>();
            TypeName = typeof(T).Name;
        }

        public string GetValueAsString()
        {
            var val = _getter();

            // Handle specific formatting for arrays if needed, or default toString
            if (val is List<double[]> list)
            {
                // Simple formatter for the list type mentioned in context
                return $"List<double[]> ({list.Count} items)";
            }

            return val?.ToString() ?? string.Empty;
        }

        public bool TryParse(string strValue)
        {
            if (_setter == null) return false;



            // WE NEED TO FIND A MORE GENERIC WAY TO HANDLE CONVERSION FROM STRING / DOUBLE ETC TO UNDERLYING TYPE
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

        // Accessor for the raw value (useful for UI binding if we cast)
        public T Value
        {
            get => _getter();
            set => _setter?.Invoke(value);
        }
    }
}
