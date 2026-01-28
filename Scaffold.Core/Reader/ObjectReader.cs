using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Scaffold.Reader
{
    public static class ObjectReader
    {
        // 1. Type-Level Cache: Stores the structural definition (Reflection/Expressions)
        private static readonly ConcurrentDictionary<Type, ObjectDefinition> _typeCache
            = new ConcurrentDictionary<Type, ObjectDefinition>();

        // 2. Instance-Level Cache: Stores the actual wrapper lists for specific instances.
        // ConditionalWeakTable ensures we don't cause memory leaks.
        private static readonly ConditionalWeakTable<object, InstanceCache> _instanceCache
            = new ConditionalWeakTable<object, InstanceCache>();

        /// <summary>
        /// Reads properties decorated with [InputCalcValue] from the provided object instance.
        /// </summary>
        public static List<ICalcValue> GetInputs(object instance)
        {
            if (instance == null) return new List<ICalcValue>();

            var instanceData = _instanceCache.GetOrCreateValue(instance);

            if (instanceData.Inputs == null)
            {
                var definition = GetDefinition(instance.GetType());
                instanceData.Inputs = definition.CreateInputs(instance);
            }

            return instanceData.Inputs;
        }

        /// <summary>
        /// Reads properties decorated with [OutputCalcValue] from the provided object instance.
        /// </summary>
        public static List<ICalcValue> GetOutputs(object instance)
        {
            if (instance == null) return new List<ICalcValue>();

            var instanceData = _instanceCache.GetOrCreateValue(instance);

            if (instanceData.Outputs == null)
            {
                var definition = GetDefinition(instance.GetType());
                instanceData.Outputs = definition.CreateOutputs(instance);
            }

            return instanceData.Outputs;
        }

        private static ObjectDefinition GetDefinition(Type type)
        {
            return _typeCache.GetOrAdd(type, t => new ObjectDefinition(t));
        }

        // --- Internal Helper Classes ---

        private class InstanceCache
        {
            public List<ICalcValue> Inputs { get; set; }
            public List<ICalcValue> Outputs { get; set; }
        }

        private class ObjectDefinition
        {
            private readonly List<IPropertyAdapter> _inputAdapters = new List<IPropertyAdapter>();
            private readonly List<IPropertyAdapter> _outputAdapters = new List<IPropertyAdapter>();

            public ObjectDefinition(Type type)
            {
                // Scan all public properties for attributes
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
                _displayName = attr.DisplayName ?? prop.Name;
                _headings = attr.Headings;

                // Compile Getter
                var param = System.Linq.Expressions.Expression.Parameter(typeof(TModel), "m");
                var access = System.Linq.Expressions.Expression.Property(param, prop);
                _getter = System.Linq.Expressions.Expression.Lambda<Func<TModel, TProp>>(access, param).Compile();

                // Compile Setter
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
