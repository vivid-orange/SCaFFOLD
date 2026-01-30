using System.Reflection;

namespace Scaffold.Reader.Utility;

/// <summary>
/// Holds the structural map for a Calculation Type (e.g., BeamCalc)
/// </summary>
internal class CalculationDefinition
{
    private readonly List<IPropertyAdapter> _inputAdapters = new List<IPropertyAdapter>();
    private readonly List<IPropertyAdapter> _outputAdapters = new List<IPropertyAdapter>();

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

            if (attr.EntityLabel is null or "")
            {
                attr.EntityLabel = ParameterNaming.SplitPascalCaseToString(prop.Name);
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
