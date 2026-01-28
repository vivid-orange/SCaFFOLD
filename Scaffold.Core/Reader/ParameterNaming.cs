using System.Text;
using System.Text.RegularExpressions;

namespace Scaffold.Reader
{
    public static class ParameterNaming
    {
        // Dictionary contains the "Ideal" acronym. 
        // Logic handles transforming the casing to match the input.
        private static readonly Dictionary<string, string> _wordBank = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Material", "Mat" },
            { "Control", "Ctrl" }
        };

        private static readonly char[] _hardConsonants = { 'k', 't', 'd', 'p', 'b', 'g', 'x', 'z' };
        private static readonly char[] _softConsonants = { 'n', 's', 'l', 'r', 'm', 'f', 'v', 'h', 'c' };

        public static string CreateTla(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            // Check if the word (or a match) exists in our bank
            if (_wordBank.TryGetValue(input, out string acronym))
            {
                return ApplyCasing(input, acronym);
            }

            if (input.Length < 4)
            {
                return input;
            }

            return IsMultiWord(input) ? GenerateMultiWord(input) : GenerateSkeleton(input);
        }

        private static string ApplyCasing(string original, string acronym)
        {
            if (original.All(c => !char.IsLetter(c) || char.IsUpper(c)))
            {
                return acronym.ToUpper();
            }

            if (char.IsLower(original[0]))
            {
                return acronym.ToLower();
            }

            // Default to TitleCase (e.g., Mat, Ctrl)
            return char.ToUpper(acronym[0]) + acronym.Substring(1).ToLower();
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
            var sb = new StringBuilder().Append(input[0]);
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
