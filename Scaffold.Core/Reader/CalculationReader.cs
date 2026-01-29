using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Scaffold.Reader;

public static class CalculationReader
{
    // 1. Type-Level Cache: Stores the structural definition (Reflection/Expressions)
    // Calculated once per Class Type.
    private static readonly ConcurrentDictionary<Type, CalculationDefinition> _typeCache
        = new ConcurrentDictionary<Type, CalculationDefinition>();

    // 2. Instance-Level Cache: Stores the actual wrapper lists for specific instances.
    // ConditionalWeakTable ensures we don't cause memory leaks; if the ICalculation 
    // is garbage collected, these cached lists go with it.
    private static readonly ConditionalWeakTable<ICalculation, InstanceCache> _instanceCache
        = new ConditionalWeakTable<ICalculation, InstanceCache>();

    private static readonly HashSet<string> _scaffoldCoreProperties = GetCoreProperties();

    private static HashSet<string> GetCoreProperties()
    {
        Type[] interfacesToExclude =
        {
            typeof(Calculation),
            typeof(ICalcParameter),
            typeof(ICalcValue)
        };

        return interfacesToExclude
            .SelectMany(i => i.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            .Select(p => p.Name)
            .ToHashSet();
    }

    public static List<ICalcValue> GetInputs(ICalculation calculation)
    {
        if (calculation == null)
        {
            return new List<ICalcValue>();
        }

        // Get or create the cache container for this specific instance
        InstanceCache instanceData = _instanceCache.GetOrCreateValue(calculation);

        // If we haven't created the Input wrappers for this instance yet, do so now
        if (instanceData.Inputs == null)
        {
            CalculationDefinition definition = GetDefinition(calculation.GetType());
            instanceData.Inputs = definition.CreateInputs(calculation);
        }

        return instanceData.Inputs;
    }

    public static List<ICalcValue> GetOutputs(ICalculation calculation)
    {
        if (calculation == null)
        {
            return new List<ICalcValue>();
        }

        InstanceCache instanceData = _instanceCache.GetOrCreateValue(calculation);

        if (instanceData.Outputs == null)
        {
            CalculationDefinition definition = GetDefinition(calculation.GetType());
            instanceData.Outputs = definition.CreateOutputs(calculation);
        }

        return instanceData.Outputs;
    }

    private static CalculationDefinition GetDefinition(Type type)
    {
        return _typeCache.GetOrAdd(type, t => new CalculationDefinition(t));
    }

    // --- Internal Helper Classes ---

    // Holds the cached lists for a specific ICalculation instance
    private class InstanceCache
    {
        public List<ICalcValue> Inputs { get; set; }
        public List<ICalcValue> Outputs { get; set; }
    }

    // Holds the structural map for a Calculation Type (e.g., BeamCalc)
    private class CalculationDefinition
    {
        private readonly List<IPropertyAdapter> _inputAdapters = new List<IPropertyAdapter>();
        private readonly List<IPropertyAdapter> _outputAdapters = new List<IPropertyAdapter>();

        public CalculationDefinition(Type type)
        {
            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (_scaffoldCoreProperties.Contains(prop.Name))
                {
                    continue; // Skip: This is an core property (CalculationTitle, Status, etc.)
                }

                CalcParameterAttribute? attr = prop.GetCustomAttribute<CalcParameterAttribute>()
                                               ?? CreateAttributes(prop);
                if (attr is null)
                {
                    continue;
                }

                IPropertyAdapter adapter = CreateAdapter(type, prop, attr);
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

        private static CalcParameterAttribute CreateAttributes(PropertyInfo prop)
        {
            return new CalcParameterAttribute(GetParameterType(prop))
            {
                Symbol = ParameterNaming.CreateThreeLetterAcronym(prop.Name),
                EntityLabel = ParameterNaming.SplitPascalCaseToString(prop.Name)
            };
        }

        private static CalcParameterType GetParameterType(PropertyInfo property)
        {
            MethodInfo? setter = property.GetSetMethod(nonPublic: true);

            // If there is no setter at all, or the setter is not public, it's an output
            if (setter == null || !setter.IsPublic)
            {
                return CalcParameterType.Output;
            }

            return CalcParameterType.Input;
        }

        public List<ICalcValue> CreateInputs(object instance)
            => _inputAdapters.Select(a => a.Create(instance)).ToList();

        public List<ICalcValue> CreateOutputs(object instance)
            => _outputAdapters.Select(a => a.Create(instance)).ToList();

        private IPropertyAdapter CreateAdapter(Type modelType, PropertyInfo prop, CalcParameterAttribute attr)
        {
            Type adapterType = typeof(PropertyAdapter<,>).MakeGenericType(modelType, prop.PropertyType);
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

        public PropertyAdapter(PropertyInfo prop, CalcParameterAttribute attr)
        {
            _symbol = attr.Symbol;
            _displayName = attr.EntityLabel ?? prop.Name;
            _headings = attr.Headings;

            // Compile Getter
            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(typeof(TModel), "m");
            MemberExpression access = System.Linq.Expressions.Expression.Property(param, prop);
            _getter = System.Linq.Expressions.Expression.Lambda<Func<TModel, TProp>>(access, param).Compile();

            // Compile Setter
            if (prop.CanWrite && prop.GetSetMethod() != null)
            {
                ParameterExpression valueParam = System.Linq.Expressions.Expression.Parameter(typeof(TProp), "v");
                MethodCallExpression assign = System.Linq.Expressions.Expression.Call(param, prop.GetSetMethod(), valueParam);
                _setter = System.Linq.Expressions.Expression.Lambda<Action<TModel, TProp>>(assign, param, valueParam).Compile();
            }
        }

        public ICalcValue Create(object instance)
        {
            TModel model = (TModel)instance;

            // Create closures around the specific instance
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
