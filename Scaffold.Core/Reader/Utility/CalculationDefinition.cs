using System.Reflection;

namespace Scaffold.Reader.Utility;

/// <summary>
/// Holds the structural map for a Calculation Type (e.g., BeamCalc)
/// </summary>
internal class CalculationDefinition : ITypeDefinition
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
                continue; // Skip: This is a core property (CalculationTitle, Status, etc.)
            }

            if (prop.GetCustomAttribute<CalcIgnoreAttribute>() != null)
            {
                continue;
            }

            CalcParameterAttribute? attr = prop.GetCustomAttribute<CalcParameterAttribute>();

            CalcParameterType paramType;
            string symbol;
            string entityLabel;
            string[]? headings;

            if (attr != null)
            {
                paramType = attr.Type;
                symbol = attr.Symbol;
                entityLabel = attr.EntityLabel;
                headings = attr.Headings;
            }
            else
            {
                paramType = GetParameterType(prop);
                symbol = ParameterNaming.CreateThreeLetterAcronym(prop.Name);
                entityLabel = ParameterNaming.SplitPascalCaseToString(prop.Name);
                headings = null;
            }

            if (string.IsNullOrEmpty(entityLabel))
            {
                entityLabel = ParameterNaming.SplitPascalCaseToString(prop.Name);
            }

            IPropertyAdapter adapter = PropertyAdapterFactory.Create(type, prop, symbol, entityLabel, headings);
            if (paramType == CalcParameterType.Input)
            {
                _inputAdapters.Add(adapter);
            }
            else
            {
                _outputAdapters.Add(adapter);
            }
        }
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
}
