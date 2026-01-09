//using System.Reflection;
//using Scaffold.Core.Attributes;
//using Scaffold.Core.Extensions;
//using Scaffold.Core.Internals;
//using Scaffold.Core.Models;

//namespace Scaffold.Core.Abstract;

//public class CalculationReader
//{
//    private class CacheItem
//    {
//        public ICalculation Calculation { get; }
//        public Type Type { get; }
//        public List<ICalcValue> Inputs { get; }
//        public List<ICalcValue> Outputs { get; }

//        public CacheItem(ICalculation calculation, Type type)
//        {
//            Type = type;
//            Calculation = calculation;
//            Inputs = new List<ICalcValue>();
//            Outputs = new List<ICalcValue>();
//        }
//    }

//    private List<CacheItem> Cache { get; } = new List<CacheItem>();

//    private static void ReadMetadata(Type type, ICalculation calculation)
//    {
//        if (calculation.CalculationName != null && calculation.ReferenceName != null)
//        {
//            return;
//        }

//        string fallbackValue = type.Name.SplitPascalCaseToString();
//        CalculationMetadataAttribute metadata = type.GetCustomAttribute<CalculationMetadataAttribute>();


//        calculation.CalculationName ??= metadata == null
//                ? fallbackValue
//                : metadata.CalculationName;

//        calculation.ReferenceName ??= metadata == null
//                ? fallbackValue
//                : metadata.ReferenceName;
//    }

//    private static ICalcValue GetCalcValue(PropertyInfo property, CacheItem cacheItem)
//    {
//        if (property.GetValue(cacheItem.Calculation) is not ICalcValue calcValue)
//        {
//            Type propertyType = property.PropertyType;
//            if (propertyType.IsAcceptedPrimitive() == false)
//                return null;

//            calcValue = new InternalCalcValue(cacheItem.Calculation, propertyType, property.Name);
//        }

//        if (string.IsNullOrEmpty(calcValue.DisplayName))
//        {
//            calcValue.DisplayName = property.Name.SplitPascalCaseToString();
//        }

//        return calcValue;
//    }

//    private static void LoadByAttributes(CacheItem cacheItem)
//    {
//        foreach (PropertyInfo property in cacheItem.Type.GetProperties())
//        {
//            CalcValueTypeAttribute baseAttribute = property.GetCustomAttribute<CalcValueTypeAttribute>();
//            if (baseAttribute == null)
//                continue;

//            ICalcValue calcValue = GetCalcValue(property, cacheItem);
//            if (calcValue == null)
//                continue;

//            calcValue.DisplayName = baseAttribute.DisplayName ?? calcValue.DisplayName;
//            calcValue.Symbol = baseAttribute.Symbol ?? calcValue.Symbol;

//            switch (baseAttribute.Type)
//            {
//                case CalcValueType.Input:
//                    cacheItem.Inputs.InsertCalcValue(calcValue);
//                    break;

//                case CalcValueType.Output:
//                    cacheItem.Outputs.InsertCalcValue(calcValue);
//                    break;

//                default:
//                case CalcValueType.Undefined:
//                    continue;
//            }
//        }
//    }

//    private CacheItem GetCachedItem<T>(T calculation) where T : class, ICalculation
//    {
//        CacheItem cachedCalculation = Cache.FirstOrDefault(x => ReferenceEquals(x.Calculation, calculation));
//        if (cachedCalculation != null)
//            return cachedCalculation;

//        var newCacheItem = new CacheItem(calculation, calculation.GetType());

//        bool isFluentConfiguration =
//            newCacheItem.Type
//                .GetInterfaces()
//                .FirstOrDefault(x => x.FullName != null && x.FullName.Contains(typeof(ICalculationConfiguration<>).Name)) != null;

//        if (isFluentConfiguration)
//        {
//            Type builderType = typeof(CalculationConfigurationBuilder<>).MakeGenericType(calculation.GetType());
//            object configurationBuilder = Activator.CreateInstance(builderType, calculation);

//            Type configurationType = typeof(ICalculationConfiguration<>).MakeGenericType(calculation.GetType());
//            configurationType.GetMethod("Configure")?.Invoke(calculation, new object[] { configurationBuilder });

//            if (configurationBuilder is CalculationConfigurationBuilderBase baseBuilder)
//            {
//                newCacheItem.Inputs.AddRange(baseBuilder.Inputs);
//                newCacheItem.Outputs.AddRange(baseBuilder.Outputs);
//            }
//        }
//        else
//        {
//            LoadByAttributes(newCacheItem);
//        }

//        ReadMetadata(newCacheItem.Type, newCacheItem.Calculation);
//        Cache.Add(newCacheItem);

//        return newCacheItem;
//    }

//    /// <summary>
//    /// Gets metadata, loading calculation from/into cache for later manipulation.
//    /// </summary>
//    public CalculationMetadata GetMetadata<T>(T calculation) where T : class, ICalculation
//    {
//        CacheItem cached = GetCachedItem(calculation);
//        return new CalculationMetadata { Title = cached.Calculation.ReferenceName, Type = cached.Calculation.CalculationName };
//    }

//    /// <summary>
//    /// Gets inputs, loading calculation from/into cache for later manipulation.
//    /// </summary>
//    public IReadOnlyList<ICalcValue> GetInputs<T>(T calculation) where T : class, ICalculation
//    {
//        CacheItem cached = GetCachedItem(calculation);
//        return cached.Inputs;
//    }

//    /// <summary>
//    /// Gets outputs, loading calculation from/into cache for later manipulation.
//    /// </summary>
//    public IReadOnlyList<ICalcValue> GetOutputs<T>(T calculation) where T : class, ICalculation
//    {
//        CacheItem cached = GetCachedItem(calculation);
//        return cached.Outputs;
//    }

//    /// <summary>
//    /// Gets formulae, loading calculation from/into cache for later manipulation.
//    /// Wrapper implemented to keep all reading features together.
//    /// This method can be called directly on the calculation also.
//    /// </summary>
//    public IList<IFormula> GetFormulae<T>(T calculation) where T : class, ICalculation
//    {
//        CacheItem cached = GetCachedItem(calculation);
//        return cached.Calculation.GetFormulae();
//    }

//    /// <summary>
//    /// Updates the calculation, loading calculation from/into cache for later manipulation.
//    /// Wrapper implemented to keep all reading features together.
//    /// This method can be called directly on the calculation also.
//    /// </summary>
//    public void Update<T>(T calculation) where T : class, ICalculation
//    {
//        CacheItem cached = GetCachedItem(calculation);
//        cached.Calculation.Calculate();
//    }
//}
