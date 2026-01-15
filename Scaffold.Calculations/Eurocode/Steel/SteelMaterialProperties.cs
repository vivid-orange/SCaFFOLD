using System;
using System.Collections.Generic;
using Scaffold.Core.Attributes;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using UnitsNet;
using UnitsNet.Units;
using VividOrange.Taxonomy.Materials;
using VividOrange.Taxonomy.Materials.StandardMaterials.En;
using VividOrange.Taxonomy.Standards.Eurocode;

namespace Scaffold.Calculations.Eurocode.Steel
{
    public class SteelMaterialProperties : ICalculation
    {
        public string ReferenceName { get; set; }
        public string CalculationName { get; set; } = "Steel Material Properties";
        public CalcStatus Status { get; set; } = CalcStatus.None;

        [InputCalcValue("Grd", "Grade")]
        public EnSteelGrade Grade { get; set; } = EnSteelGrade.S355;

        [InputCalcValue("t", "Nominal thickness of the element")]
        public Length Thickness { get; set; } = new(40, LengthUnit.Millimeter);

        [OutputCalcValue("S", "Steel Material")]
        public EnSteelMaterial Material => new(Grade, NationalAnnex.RecommendedValues);

        [OutputCalcValue("E", "Modulus of Elasticity")]
        public Pressure E => new(210000, _unit);

        [OutputCalcValue(@"\nu", "Poisson's ratio")]
        public double nu => 0.3;

        [OutputCalcValue("G", "Shear Modulus")]
        public Pressure G => E / (2 * (1 + nu));

        [OutputCalcValue(@"\alpha_T", "Coefficient of Linear Thermal Expansion")]
        public CoefficientOfThermalExpansion alpha =>
            new(12 * 10 ^ -6, CoefficientOfThermalExpansionUnit.PerKelvin);

        [OutputCalcValue("f_y", "Yield Strength")]
        public Pressure fy => _analysisMaterial.YieldStrength;

        [OutputCalcValue("f_u", "Ultimate Tensile Strength")]
        public Pressure fu => _analysisMaterial.UltimateStrength;

        [OutputCalcValue("ε_y", "Yield Strain")]
        public Ratio Epsilony => _analysisMaterial.YieldStrain;

        [OutputCalcValue("ε_u", "Failure Tension Strain")]
        public Ratio Epsilonu => _analysisMaterial.FailureStrain;

        [OutputCalcValue("ε", "Material Parameter")]
        public double Epsilon => Math.Sqrt(235 / fy.As(_unit));

        private IBiLinearMaterial _analysisMaterial => EnSteelFactory.CreateBiLinear(Material, Thickness);
        private static PressureUnit _unit = PressureUnit.NewtonPerSquareMillimeter;

        public SteelMaterialProperties()
        {
            Calculate();
        }

        public IList<IFormula> GetFormulae()
        {
            return new List<IFormula>();
        }

        public void Calculate() { }
    }
}
