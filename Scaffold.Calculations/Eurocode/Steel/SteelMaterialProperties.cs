using VividOrange.Materials;
using VividOrange.Materials.StandardMaterials.En;

namespace Scaffold.Calculations.Eurocode.Steel
{
    public class SteelMaterialProperties : Calculation
    {
        public EnSteelGrade Grade { get; set; } = EnSteelGrade.S355;

        [InputParameter("t", "Nominal thickness of the element")]
        public Length Thickness { get; set; } = new(40, LengthUnit.Millimeter);

        [OutputParameter("S", "Steel Material")]
        public EnSteelMaterial Material => new(Grade, NationalAnnex);

        [OutputParameter("E", "Modulus of Elasticity")]
        public Pressure E => new(210000, _unit);

        [OutputParameter(@"\nu", "Poisson's ratio")]
        public double nu => 0.3;

        [OutputParameter("G", "Shear Modulus")]
        public Pressure G => E / (2 * (1 + nu));

        [OutputParameter(@"\alpha_T", "Coefficient of Linear Thermal Expansion")]
        public CoefficientOfThermalExpansion alpha =>
            new((12 * 10) ^ -6, CoefficientOfThermalExpansionUnit.PerKelvin);

        [OutputParameter("f_y", "Yield Strength")]
        public Pressure fy => _analysisMaterial.YieldStrength;

        [OutputParameter("f_u", "Ultimate Tensile Strength")]
        public Pressure fu => _analysisMaterial.UltimateStrength;

        [OutputParameter("ε_y", "Yield Strain")]
        public Ratio Epsilony => _analysisMaterial.YieldStrain;

        [OutputParameter("ε_u", "Failure Tension Strain")]
        public Ratio Epsilonu => _analysisMaterial.FailureStrain;

        [OutputParameter("ε", "Material Parameter")]
        public double Epsilon => Math.Sqrt(235 / fy.As(_unit));

        private IBiLinearMaterial _analysisMaterial => EnSteelFactory.CreateBiLinear(Material, Thickness);
        private static PressureUnit _unit = PressureUnit.NewtonPerSquareMillimeter;

        public SteelMaterialProperties()
        {
            Calculate();
        }
    }
}
