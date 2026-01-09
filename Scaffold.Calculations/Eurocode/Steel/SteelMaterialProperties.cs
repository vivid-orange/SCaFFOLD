using System;
using System.Collections.Generic;
using MagmaWorks.Taxonomy.Materials;
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

namespace Scaffold.Calculations.Eurocode.Steel
{
    public class SteelMaterialProperties : ICalculation
    {
        public string DisplayName { get; set; }
        public string CalculationName { get; set; } = "Steel Material Properties";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue("Grd", "Grade")]
        public CalcSelectionList SteelGrade { get; set; }
            = new CalcSelectionList("Steel Grade", "S355", EnumSelectionListParser.SteelGrades);

        [InputCalcValue]
        public CalcLength Thickness { get; set; }
            = new CalcLength(40, LengthUnit.Millimeter, "Nominal thickness of the element", "t");

        [OutputCalcValue]
        public CalcEnSteelMaterial Material =>
            new CalcEnSteelMaterial(SteelGrade.GetEnum<EnSteelGrade>(),
                                    NationalAnnex.RecommendedValues, "Steel", "S");

        [OutputCalcValue]
        public CalcStress E
            => new CalcStress(new Pressure(210000, _unit), "Modulus of Elasticity", "E");

        [OutputCalcValue]
        public CalcDouble nu => new CalcDouble(0.3, "Poisson's ratio", @"\nu");

        [OutputCalcValue]
        public CalcStress G => new CalcStress(E / (2 * (1 + nu)), "Shear Modulus", "G");

        [OutputCalcValue]
        public CalcQuantityWrapper<CoefficientOfThermalExpansion> alpha => new CalcQuantityWrapper<CoefficientOfThermalExpansion>(
            new CoefficientOfThermalExpansion(12 * 10 ^ -6, CoefficientOfThermalExpansionUnit.PerKelvin),
            "Coefficient of Linear Thermal Expansion", @"\alpha_T");

        [OutputCalcValue]
        public CalcStress fy => new CalcStress(_analysisMaterial.YieldStrength, "Yield Strength", "f_y");

        [OutputCalcValue]
        public CalcStress fu => new CalcStress(_analysisMaterial.UltimateStrength, "Ultimate Tensile Strength", "f_u");

        [OutputCalcValue]
        public CalcStrain Epsilony => new CalcStrain(_analysisMaterial.YieldStrain, "Yield Strain", "ε_y");

        [OutputCalcValue]
        public CalcStrain Epsilonu => new CalcStrain(_analysisMaterial.FailureStrain, "Failure Tension Strain", "ε_u");

        [OutputCalcValue]
        public CalcDouble Epsilon => new CalcDouble(Math.Sqrt(235 / fy.Quantity.As(_unit)),
            "Material Parameter", "ε");

        private IBiLinearMaterial _analysisMaterial => EnSteelFactory.CreateBiLinear(Material, Thickness);
        private static PressureUnit _unit = PressureUnit.NewtonPerSquareMillimeter;

        public SteelMaterialProperties()
        {
            Calculate();
        }

        public SteelMaterialProperties(EnSteelGrade grade)
        {
            SteelGrade.Value = grade.ToString();
            Calculate();
        }

        public List<IOutputItem> GetFormulae()
        {
            return new List<IOutputItem>();
        }

        public void Calculate() { }

        public List<ICalcValue> GetInputs() => throw new NotImplementedException();
        public List<ICalcValue> GetOutputs() => throw new NotImplementedException();
    }
}
