using System;
using System.Collections.Generic;
using System.Linq;
using VividOrange.Taxonomy.Profiles;

namespace Scaffold.Calculations.CalculationUtility
{
    internal enum CatalogueType
    {
        IPE,
        HE,
        HEA,
        HEAA,
        HEB,
        HEM,
        HL,
        HD,
        HP,
        UBP,
        UB,
        UC,
        IPN,
        J,
        UPE,
        PFC,
        UPN,
        CH,
    }

    internal static class CatalogueProfileSelectionList
    {
        internal static List<string> GetCatalogueProfiles(CatalogueType type)
        {
            switch (type)
            {
                case CatalogueType.IPE: return IPEs;
                case CatalogueType.HE: return HEs;
                case CatalogueType.HEA: return HEAs;
                case CatalogueType.HEAA: return HEAAs;
                case CatalogueType.HEB: return HEBs;
                case CatalogueType.HEM: return HEMs;
                case CatalogueType.HL: return HLs;
                case CatalogueType.HD: return HDs;
                case CatalogueType.HP: return HPs;
                case CatalogueType.UBP: return UBPs;
                case CatalogueType.UB: return UBs;
                case CatalogueType.UC: return UCs;
                case CatalogueType.IPN: return IPNs;
                case CatalogueType.J: return Js;
                case CatalogueType.UPE: return UPEs;
                case CatalogueType.PFC: return PFCs;
                case CatalogueType.UPN: return UPNs;
                case CatalogueType.CH: return CHs;
                default: throw new NotImplementedException();
            }
        }

        internal static readonly List<string> Catalogues = Enum.GetValues(typeof(CatalogueType)).Cast<CatalogueType>()
        .Select(p => p.ToString()).ToList();

        internal static readonly List<string> IPEs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("IPE")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> HEs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("HE")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> HEAs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("HE") && p.ToString().Contains("A") && !p.ToString().Contains("AA"))
            .Select(p => p.ToString()).ToList();

        internal static readonly List<string> HEAAs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("HE") && p.ToString().Contains("AA")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> HEBs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("HE") && p.ToString().Contains("B")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> HEMs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("HE") && p.ToString().Contains("M")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> HLs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("HL")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> HDs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("HD")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> HPs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("HP")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> UBPs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("UBP")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> UBs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("UB") && !p.ToString().Contains("UBP")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> UCs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("UC")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> IPNs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("IPN")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> Js = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("J")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> UPEs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("UPE")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> PFCs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("PFC")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> UPNs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("UPN")).Select(p => p.ToString()).ToList();

        internal static readonly List<string> CHs = Enum.GetValues(typeof(European)).Cast<European>()
        .Where(p => p.ToString().StartsWith("CH")).Select(p => p.ToString()).ToList();
    }
}
