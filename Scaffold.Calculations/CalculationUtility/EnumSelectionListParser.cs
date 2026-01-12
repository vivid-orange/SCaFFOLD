using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VividOrange.Taxonomy.Materials.StandardMaterials.En;
using VividOrange.Taxonomy.Sections;
using VividOrange.Taxonomy.Sections.Reinforcement;
using VividOrange.Taxonomy.Standards.Eurocode;
using Scaffold.Core.CalcValues;

namespace Scaffold.Calculations.CalculationUtility
{
    internal static class EnumSelectionListParser
    {
        internal static readonly List<string> BarDiameters = Enum.GetValues(typeof(BarDiameter)).Cast<BarDiameter>()
        .Select(v => v.ToString().Replace("D", string.Empty)).ToList();
        internal static readonly List<string> ConcreteGrades = Enum.GetValues(typeof(EnConcreteGrade)).Cast<EnConcreteGrade>()
            .Select(v => v.ToString().Replace("_", "/")).ToList();
        internal static readonly List<string> NationalAnnexes = Enum.GetValues(typeof(NationalAnnex)).Cast<NationalAnnex>()
            .Select(v => SplitCamelCase(v.ToString())).ToList();
        internal static readonly List<string> RebarGrades = Enum.GetValues(typeof(EnRebarGrade)).Cast<EnRebarGrade>()
            .Select(v => v.ToString()).ToList();
        internal static readonly List<string> SectionFaces = Enum.GetValues(typeof(SectionFace)).Cast<SectionFace>()
            .Select(v => v.ToString()).ToList();
        internal static readonly List<string> SteelGrades = Enum.GetValues(typeof(EnSteelGrade)).Cast<EnSteelGrade>()
            .Select(v => v.ToString()).ToList();

        internal static T GetEnum<T>(this CalcSelectionList list,
            string replaceOld = "", string replaceNew = "",
            string prefix = "", string suffix = "") where T : Enum
        {
            string value = string.IsNullOrEmpty(replaceOld)
                ? list.Value
                : list.Value.Replace(replaceOld, replaceNew);
            return (T)Enum.Parse(typeof(T), StringWithoutSpaces(prefix + value + suffix), true);
        }

        private static string SplitCamelCase(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return Regex.Replace(str, @"(\p{Lu})", " $1");
        }

        private static string StringWithoutSpaces(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return str.Replace(" ", string.Empty);
        }
    }
}
