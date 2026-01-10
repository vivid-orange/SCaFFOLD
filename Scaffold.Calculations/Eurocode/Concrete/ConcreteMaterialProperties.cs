using System;
using System.Collections.Generic;
using MagmaWorks.Taxonomy.Materials.StandardMaterials.En;
using MagmaWorks.Taxonomy.Standards.Eurocode;
using Scaffold.Calculations.CalculationUtility;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcObjects.Materials.StandardMaterials.En;
using Scaffold.Core.CalcQuantities;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Calculations.Eurocode.Concrete
{
    public class ConcreteMaterialProperties : ICalculation
    {
        public string TypeName { get; set; }
        public string InstanceName { get; set; } = "Concrete Material Properties";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue("Grd", "Grade")]
        public CalcSelectionList ConcreteGrade { get; set; }
            = new CalcSelectionList("Concrete Grade", "C30/37", EnumSelectionListParser.ConcreteGrades);

        [OutputCalcValue]
        public CalcEnConcreteMaterial Material =>
            new CalcEnConcreteMaterial(ConcreteGrade.GetEnum<EnConcreteGrade>("/", "_"),
                                       NationalAnnex.RecommendedValues, "Concrete", "C");

        [OutputCalcValue]
        public CalcStress fck
            => new CalcStress(GetStrength(), "Characteristic cylinder strength", "f_{ck}");

        [OutputCalcValue]
        public CalcStress fckcube => new CalcStress(
            new Pressure(double.Parse(Material.Grade.ToString().Split('_')[1]), _unit),
            "Characteristic cube strength", "f_{ck,cube}");

        [OutputCalcValue]
        public CalcStress fcm => new CalcStress(GetStrength() + new Pressure(8, _unit),
                "Mean cylinder strength", "f_{cm}");

        [OutputCalcValue]
        public CalcStress fctm => new CalcStress(fck <= 50
                    ? 0.3 * Math.Pow(fck, 2d / 3d)
                    : 2.12 * Math.Log(1 + fcm / 10),
            PressureUnit.Megapascal, "Mean tensile strength", "f_{ctm}");

        [OutputCalcValue]
        public CalcStress fctk005 => new CalcStress(0.7 * fctm,
            PressureUnit.Megapascal, "Tensile strength 5% fractile", "f_{ctk;0.05}");

        [OutputCalcValue]
        public CalcStress fctk095 => new CalcStress(1.3 * fctm,
            PressureUnit.Megapascal, "Tensile strength 95% fractile", "f_{ctk;0.95}");

        [OutputCalcValue]
        public CalcStress Ecm => new CalcStress(22 * Math.Pow(fcm / 10, 0.3),
            PressureUnit.Gigapascal, "Secant modulus of elasticity", "E_{cm}");

        [OutputCalcValue]
        public CalcStrain Epsilonc1 => new CalcStrain(Math.Min(2.8, 0.7 * Math.Pow(fcm, 0.31)),
            RatioUnit.PartPerThousand, "Nominal peak strain", "ε_{c1}");

        [OutputCalcValue]
        public CalcStrain Epsiloncu1 => new CalcStrain(fck >= 50
                    ? 2.8 + 27.0 * Math.Pow((98 - fcm) / 100, 4)
                    : 3.5,
            RatioUnit.PartPerThousand, "Nominal ultimate strain", "ε_{cu1}");

        [OutputCalcValue]
        public CalcStrain Epsilonc2 => new CalcStrain(fck >= 50
                    ? 2.0 + 0.085 * Math.Pow(fck - 50, 0.53)
                    : 2.0,
            RatioUnit.PartPerThousand, "Simplified parabola-rectangle peak strain", "ε_{c2}");

        [OutputCalcValue]
        public CalcStrain Epsiloncu2 => new CalcStrain(fck >= 50
                    ? 2.6 + 35.0 * Math.Pow((90 - fck) / 100, 4)
                    : 3.5,
            RatioUnit.PartPerThousand, "Simplified ultimate strain", "ε_{cu2}");

        [OutputCalcValue]
        public CalcDouble n => new CalcDouble(fck >= 50
                    ? 1.4 + 23.4 * Math.Pow((90 - fck) / 100, 4)
                    : 2.0,
             "Exponent", @"\textit{n}");

        [OutputCalcValue]
        public CalcStrain Epsilonc3 => new CalcStrain(fck >= 50
                    ? 1.75 + 0.55 * ((fck - 50) / 40)
                    : 1.75,
            RatioUnit.PartPerThousand, "Simplified bi-linear peak strain", "ε_{c3}");

        [OutputCalcValue]
        public CalcStrain Epsiloncu3 => new CalcStrain(fck >= 50
                    ? 2.6 + 35.0 * Math.Pow((90 - fck) / 100, 4)
                    : 3.5,
            RatioUnit.PartPerThousand, "Simplified ultimate strain", "ε_{cu3}");

        private static PressureUnit _unit = PressureUnit.Megapascal;

        public ConcreteMaterialProperties()
        {
            Calculate();
        }

        public ConcreteMaterialProperties(EnConcreteGrade grade)
        {
            ConcreteGrade.Value = grade.ToString().Replace("_", "/");
            Calculate();
        }

        public List<IOutputItem> GetFormulae()
        {
            return new List<IOutputItem>();
        }

        public void Calculate() { }

        private Pressure GetStrength() => new Pressure(
            double.Parse(Material.Grade.ToString().Split('C', '_')[1]), _unit);
        public List<ICalcValue> GetInputs() => throw new NotImplementedException();
        public List<ICalcValue> GetOutputs() => throw new NotImplementedException();
    }
}
