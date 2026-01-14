using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Interfaces;
using Scaffold.Core.Models;
using CalcValueType = Scaffold.Core.Attributes.CalcValueType;

namespace Scaffold.Core.Services
{
    public static class CalculationReader
    {
        // Cache to store the definition for each Calculation Type
        private static readonly ConcurrentDictionary<Type, CalculationDefinition> _cache
            = new ConcurrentDictionary<Type, CalculationDefinition>();

        public static List<ICalcValue> GetInputs(ICalculation calculation)
        {
            if (calculation == null) return new List<ICalcValue>();
            var definition = GetDefinition(calculation.GetType());
            return definition.CreateInputs(calculation);
        }

        public static List<ICalcValue> GetOutputs(ICalculation calculation)
        {
            if (calculation == null) return new List<ICalcValue>();
            var definition = GetDefinition(calculation.GetType());
            return definition.CreateOutputs(calculation);
        }

        private static CalculationDefinition GetDefinition(Type type)
        {
            return _cache.GetOrAdd(type, t => new CalculationDefinition(t));
        }

        // --- Internal Helper Classes to map the Type ---

        private class CalculationDefinition
        {
            private readonly List<IPropertyAdapter> _inputAdapters = new List<IPropertyAdapter>();
            private readonly List<IPropertyAdapter> _outputAdapters = new List<IPropertyAdapter>();

            public CalculationDefinition(Type type)
            {
                foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var attr = prop.GetCustomAttribute<CalcValueTypeAttribute>();
                    if (attr == null) continue;

                    var adapter = CreateAdapter(type, prop, attr);

                    if (attr.Type == CalcValueType.Input)
                        _inputAdapters.Add(adapter);
                    else
                        _outputAdapters.Add(adapter);
                }
            }

            public List<ICalcValue> CreateInputs(object instance)
                => _inputAdapters.Select(a => a.Create(instance)).ToList();

            public List<ICalcValue> CreateOutputs(object instance)
                => _outputAdapters.Select(a => a.Create(instance)).ToList();

            private IPropertyAdapter CreateAdapter(Type modelType, PropertyInfo prop, CalcValueTypeAttribute attr)
            {
                // Create generic PropertyAdapter<TModel, TProp> via reflection
                var adapterType = typeof(PropertyAdapter<,>).MakeGenericType(modelType, prop.PropertyType);
                return (IPropertyAdapter)Activator.CreateInstance(adapterType, prop, attr);
            }
        }

        private interface IPropertyAdapter
        {
            ICalcValue Create(object instance);
        }

        private class PropertyAdapter<TModel, TProp> : IPropertyAdapter
        {
            private readonly Func<TModel, TProp> _getter;
            private readonly Action<TModel, TProp> _setter;
            private readonly string _symbol;
            private readonly string _displayName;
            private readonly string[] _headings;

            public PropertyAdapter(PropertyInfo prop, CalcValueTypeAttribute attr)
            {
                _symbol = attr.Symbol;
                _displayName = attr.DisplayName ?? prop.Name; // Default to Prop Name if null
                _headings = attr.Headings;

                // 1. Compile Getter: (TModel m) => m.Prop
                var param = System.Linq.Expressions.Expression.Parameter(typeof(TModel), "m");
                var access = System.Linq.Expressions.Expression.Property(param, prop);
                _getter = System.Linq.Expressions.Expression.Lambda<Func<TModel, TProp>>(access, param).Compile();

                // 2. Compile Setter: (TModel m, TProp v) => m.Prop = v
                if (prop.CanWrite && prop.GetSetMethod() != null)
                {
                    var valueParam = System.Linq.Expressions.Expression.Parameter(typeof(TProp), "v");
                    var assign = System.Linq.Expressions.Expression.Call(param, prop.GetSetMethod(), valueParam);
                    _setter = System.Linq.Expressions.Expression.Lambda<Action<TModel, TProp>>(assign, param, valueParam).Compile();
                }
            }

            public ICalcValue Create(object instance)
            {
                TModel model = (TModel)instance;

                // Create closures around the instance
                Func<TProp> boundGetter = () => _getter(model);
                Action<TProp> boundSetter = _setter == null ? null : (v) => _setter(model, v);

                return new DelegateCalcValue<TProp>(
                    boundGetter,
                    boundSetter,
                    _symbol,
                    _displayName,
                    _headings);
            }
        }
    }
}
