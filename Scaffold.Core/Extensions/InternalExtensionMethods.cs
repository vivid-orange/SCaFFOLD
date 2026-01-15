using System.Reflection;
using System.Text.RegularExpressions;

namespace Scaffold.Core.Extensions;

internal static class InternalExtensionMethods
{
    internal static bool IsAcceptedPrimitive(this Type type)
        => type == typeof(double)
           || type == typeof(int) || type == typeof(long) || type == typeof(short)
           || type == typeof(float) || type == typeof(decimal) || type == typeof(bool);

    internal static string SplitPascalCaseToString(this string pascalCaseStr)
    {
        var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

        string modelWithSpaces = r.Replace(pascalCaseStr, " ");
        return modelWithSpaces;
    }

    internal static void CopyTo<T>(this T from, T to)
    {
        Type t = typeof(T);
        PropertyInfo[] props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo p in props)
        {
            if (!p.CanRead || !p.CanWrite) continue;

            object val = p.GetGetMethod().Invoke(from, null);
            object defaultVal = p.PropertyType.IsValueType ? Activator.CreateInstance(p.PropertyType) : null;
            if (null != val && !val.Equals(defaultVal))
            {
                p.GetSetMethod().Invoke(to, new[] { val });
            }
        }
    }
}
