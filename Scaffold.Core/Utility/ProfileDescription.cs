using System.Reflection;
using VividOrange.Taxonomy.Profiles;

namespace Scaffold.Core.Utility
{
    internal static class ProfileDescription
    {
        internal static T ProfileFromDescription<T>(string description) where T : IProfile
        {
            List<Length> lengths = GetLengths(description);
            Type t = typeof(T);
            ConstructorInfo[] constructors = t.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            var args = new List<object>();
            args.AddRange(lengths.Select(l => (object)l).ToList());
            args.Add(string.Empty); // DisplayName
            args.Add(string.Empty); // Symbol
            object instance = constructors.FirstOrDefault().Invoke(args.ToArray());
            return (T)instance;
        }

        private static List<string> SanitiseString(string str)
        {
            var strings = new List<string>();
            strings.AddRange(str
                .Replace("x", " ").Replace("×", " ")
                .Replace("\u2009", " ").Split(' '));
            strings = strings.Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

            return strings.Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
        }

        private static string GetFallbackUnit(string str)
        {
            List<string> strings = SanitiseString(str);
            for (int i = strings.Count - 1; i >= 0; i--)
            {
                string unit = RemoveDigits(strings[i]);
                if (!string.IsNullOrEmpty(unit))
                {
                    return unit;
                }
            }

            throw new ScaffoldException($"Unable to detect unit in profile description {str}.");
        }

        private static (List<double>, List<LengthUnit>) SplitValueAndUnit(string str)
        {
            LengthUnit fallbackUnit = Length.ParseUnit(GetFallbackUnit(str));
            List<string> strings = SanitiseString(str);
            var values = new List<double>();
            var units = new List<LengthUnit>();
            for (int i = 0; i < strings.Count; i++)
            {
                double value = 0;
                if (!double.TryParse(GetDigitsOnly(strings[i]), NumberStyles.Any, CultureInfo.CurrentCulture, out value))
                {
                    continue;
                }

                values.Add(value);
                string unit = RemoveDigits(strings[i]);
                if (string.IsNullOrEmpty(unit) && i != strings.Count - 1)
                {
                    if (double.TryParse(strings[i + 1], NumberStyles.Any, CultureInfo.CurrentCulture, out value))
                    {
                        units.Add(fallbackUnit);
                        continue;
                    }
                    else
                    {
                        unit = RemoveDigits(strings[i + 1]);
                    }
                }

                if (string.IsNullOrEmpty(unit))
                {
                    throw new ScaffoldException($"Unknown syntax in profile description {str}.");
                }

                units.Add(Length.ParseUnit(unit));
            }

            return (values, units);
        }

        private static List<Length> GetLengths(string str)
        {
            (List<double> values, List<LengthUnit> units) splitStrings = SplitValueAndUnit(str);
            var lengths = new List<Length>();
            for (int i = 0; i < splitStrings.values.Count; i++)
            {
                lengths.Add(new Length(splitStrings.values[i], splitStrings.units[i]));
            }

            return lengths;
        }

        private static string RemoveDigits(string str)
        {
            return new String(str.Where(c => (c < '0' || c > '9') &&
                c != Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                ).ToArray());
        }

        private static string GetDigitsOnly(string str)
        {
            return new String(str.Where(c => char.IsDigit(c) ||
                c == Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                ).ToArray());
        }
    }
}
