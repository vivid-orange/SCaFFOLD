using System.Text;
using System.Text.RegularExpressions;

namespace Scaffold.Reader
{
    public static class ParameterNaming
    {
        public static string SplitPascalCaseToString(string input, bool lowercaseRest = true)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            MatchCollection matches = Regex.Matches(input, @"([A-Z]+(?=[A-Z][a-z])|[A-Z][a-z]+|[0-9]+|[A-Z]+)");
            string[] words = matches.Cast<Match>().Select(m => m.Value).ToArray();
            if (lowercaseRest && words.Length > 1)
            {
                // Keep first word as is, lowercase the rest
                for (int i = 1; i < words.Length; i++)
                {
                    words[i] = words[i].ToLower();
                }
            }

            return string.Join(" ", words);
        }

        // Dictionary contains the "Ideal" acronym. 
        // Logic handles transforming the casing to match the input.
        private static readonly Dictionary<string, string> _wordBank = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Material", "Mat" },
            { "Control", "Ctrl" },
            { "Time", "t" },
            { "Length", "L" },
            { "Width", "W" },
            { "Breadth", "B" },
            { "Height", "H" },
            { "RelativeHumidity", "RH" },
            { "Thickness", "t" },
        };

        private static readonly char[] _hardConsonants = { 'k', 't', 'd', 'p', 'b', 'g', 'x', 'z' };
        private static readonly char[] _softConsonants = { 'n', 's', 'l', 'r', 'm', 'f', 'v', 'h', 'c' };

        public static string CreateThreeLetterAcronym(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            // Check if the word (or a match) exists in our bank
            if (_wordBank.TryGetValue(input, out string acronym))
            {
                return acronym;
            }

            if (input.Length < 4)
            {
                return input;
            }

            return IsMultiWord(input) ? GenerateMultiWord(input) : GenerateSkeleton(input);
        }

        private static bool IsMultiWord(string input) => input.Skip(1).Any(char.IsUpper);

        private static string GenerateMultiWord(string input)
        {
            string[] words = Regex.Split(input, @"(?<!^)(?=[A-Z])").Where(s => !string.IsNullOrEmpty(s)).ToArray();
            string prefix = $"{words[0][0]}{words[1][0]}";

            if (words.Length >= 3)
            {
                return prefix + words[2][0];
            }

            string searchSpace = words[1].Substring(1);
            char third = searchSpace.FirstOrDefault(c => _hardConsonants.Contains(char.ToLower(c)));

            if (third == '\0')
            {
                third = searchSpace.FirstOrDefault(c => _softConsonants.Contains(char.ToLower(c)));
            }

            return prefix + char.ToLower(third != '\0' ? third : words[1][1]);
        }

        private static string GenerateSkeleton(string input)
        {
            StringBuilder sb = new StringBuilder().Append(input[0]);
            string remainder = input.Substring(1).Replace("ck", "k", StringComparison.OrdinalIgnoreCase);

            foreach (char c in remainder)
            {
                if (sb.Length >= 3)
                {
                    break;
                }

                if (!"aeiouAEIOU".Contains(c))
                {
                    sb.Append(char.ToLower(c));
                }
            }

            return sb.Length < 3 ? input.Substring(0, 3) : sb.ToString();
        }
    }
}
