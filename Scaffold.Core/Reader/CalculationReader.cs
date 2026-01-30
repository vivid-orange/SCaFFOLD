using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Scaffold.Reader.Utility;

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
    private static readonly ConditionalWeakTable<ICalculation, InstanceCache> _instanceCache = [];



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








}
