using System.Reflection;

namespace Scaffold.Reader.Utility;

internal static class PropertyAdapterFactory
{
    public static IPropertyAdapter Create(Type modelType, PropertyInfo prop, CalcParameterAttribute attr)
    {
        return Create(modelType, prop, attr.Symbol, attr.EntityLabel ?? prop.Name, attr.Headings);
    }

    public static IPropertyAdapter Create(
        Type modelType, PropertyInfo prop, string symbol, string entityLabel, string[]? headings)
    {
        Type adapterType = typeof(PropertyAdapter<,>).MakeGenericType(modelType, prop.PropertyType);
        return (IPropertyAdapter)Activator.CreateInstance(adapterType, prop, symbol, entityLabel, headings);
    }
}
