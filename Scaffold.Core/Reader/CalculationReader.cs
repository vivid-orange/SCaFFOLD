using System.Collections;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Scaffold.Reader.Utility;

namespace Scaffold.Reader;

public static class CalculationReader
{
    // Type-Level Cache: Stores the structural definition (Reflection/Expressions)
    // Calculated once per Class Type.
    private static readonly ConcurrentDictionary<Type, ITypeDefinition> _typeCache = new();

    // Value Kind Cache: Classifies types as Standard, Complex, Collection, or Calculation.
    private static readonly ConcurrentDictionary<Type, CalcValueKind> _kindCache = new();

    // Instance-Level Cache: Stores the actual wrapper lists for specific instances.
    // ConditionalWeakTable ensures we don't cause memory leaks; if the instance
    // is garbage collected, these cached lists go with it.
    private static readonly ConditionalWeakTable<object, InstanceCache> _instanceCache = [];

    public static List<ICalcValue> GetInputs(ICalculation calculation) => GetInputs((object)calculation);

    public static List<ICalcValue> GetOutputs(ICalculation calculation) => GetOutputs((object)calculation);

    public static List<ICalcValue> GetInputs(object instance)
    {
        if (instance == null)
        {
            return new List<ICalcValue>();
        }

        InstanceCache instanceData = _instanceCache.GetOrCreateValue(instance);

        if (instanceData.Inputs == null)
        {
            ITypeDefinition definition = GetDefinition(instance.GetType());
            instanceData.Inputs = definition.CreateInputs(instance);
        }

        return instanceData.Inputs;
    }

    public static List<ICalcValue> GetOutputs(object instance)
    {
        if (instance == null)
        {
            return new List<ICalcValue>();
        }

        InstanceCache instanceData = _instanceCache.GetOrCreateValue(instance);

        if (instanceData.Outputs == null)
        {
            ITypeDefinition definition = GetDefinition(instance.GetType());
            instanceData.Outputs = definition.CreateOutputs(instance);
        }

        return instanceData.Outputs;
    }

    private static ITypeDefinition GetDefinition(Type type)
    {
        return _typeCache.GetOrAdd(type, t =>
            typeof(ICalculation).IsAssignableFrom(t)
                ? new CalculationDefinition(t)
                : new ObjectDefinition(t));
    }

    public static CalcValueKind GetValueKind(Type type)
    {
        return _kindCache.GetOrAdd(type, t =>
        {
            if (typeof(ICalculation).IsAssignableFrom(t))
                return CalcValueKind.Calculation;

            if (typeof(ICollection).IsAssignableFrom(t) && t != typeof(string))
                return CalcValueKind.Collection;

            if (HasCalcParameterProperties(t))
                return CalcValueKind.Complex;

            return CalcValueKind.Standard;
        });
    }

    public static CalcValueKind GetValueKind(ICalcValue value)
    {
        ArgumentNullException.ThrowIfNull(value);
        
        Type valueType = value.GetType();
        
        // Extract generic T from DelegateCalcValue<T>
        if (valueType.IsGenericType && valueType.GetGenericTypeDefinition().Name == "DelegateCalcValue`1")
        {
            Type genericArg = valueType.GetGenericArguments()[0];
            return GetValueKind(genericArg);
        }

        // Fallback to the value's own type
        return GetValueKind(valueType);
    }

    private static bool HasCalcParameterProperties(Type type)
    {
        if (type.IsPrimitive)
            return false;

        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   .Any(p => Attribute.IsDefined(p, typeof(CalcParameterAttribute)));
    }

    /// <summary>
    /// Type definition for plain objects - only reads explicit [CalcParameter] attributes.
    /// </summary>
    private class ObjectDefinition : ITypeDefinition
    {
        private readonly List<IPropertyAdapter> _inputAdapters = new();
        private readonly List<IPropertyAdapter> _outputAdapters = new();

        public ObjectDefinition(Type type)
        {
            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                CalcParameterAttribute attr = prop.GetCustomAttribute<CalcParameterAttribute>();
                if (attr == null)
                {
                    continue;
                }

                IPropertyAdapter adapter = PropertyAdapterFactory.Create(type, prop, attr);

                if (attr.Type == CalcParameterType.Input)
                {
                    _inputAdapters.Add(adapter);
                }
                else
                {
                    _outputAdapters.Add(adapter);
                }
            }
        }

        public List<ICalcValue> CreateInputs(object instance)
            => _inputAdapters.Select(a => a.Create(instance)).ToList();

        public List<ICalcValue> CreateOutputs(object instance)
            => _outputAdapters.Select(a => a.Create(instance)).ToList();
    }
}
