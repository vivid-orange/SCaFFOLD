using VividOrange.Materials.StandardMaterials.En;

namespace Scaffold.Calculations.Eurocode.Concrete
{
    public class ConcreteMaterialProperties : Calculation
    {
        [InputParameter("Grd", "Grade")]
        public EnConcreteGrade ConcreteGrade { get; set; } = EnConcreteGrade.C30_37;

        [OutputParameter("C", "Concrete")]
        public EnConcreteMaterial Material => new(ConcreteGrade, NationalAnnex);

        [OutputParameter("f_{ck}", "Characteristic cylinder strength")]
        public Pressure fck =>
            new(double.Parse(Material.Grade.ToString().Split('C', '_')[1]), _unit);

        [OutputParameter("f_{ck,cube}", "Characteristic cube strength")]
        public Pressure fckcube =>
            new(double.Parse(Material.Grade.ToString().Split('_')[1]), _unit);

        [OutputParameter("f_{cm}", "Mean cylinder strength")]
        public Pressure fcm => fck + new Pressure(8, _unit);

        [OutputParameter("f_{ctm}", "Mean tensile strength")]
        public Pressure fctm => new(fck.As(_unit) <= 50
                    ? 0.3 * Math.Pow(fck.As(_unit), 2d / 3d)
                    : 2.12 * Math.Log(1 + (fcm.As(_unit) / 10)),
                    _unit);

        [OutputParameter("f_{ctk;0.05}", "Tensile strength 5% fractile")]
        public Pressure fctk005 => 0.7 * fctm;

        [OutputParameter("f_{ctk;0.95}", "Tensile strength 95% fractile")]
        public Pressure fctk095 => 1.3 * fctm;

        [OutputParameter("E_{cm}", "Secant modulus of elasticity")]
        public Pressure Ecm =>
            new(22 * Math.Pow(fcm.As(_unit) / 10, 0.3), PressureUnit.Gigapascal);

        [OutputParameter("ε_{c1}", "Nominal peak strain")]
        public Ratio Epsilonc1 =>
            new(Math.Min(2.8, 0.7 * Math.Pow(fcm.As(_unit), 0.31)), RatioUnit.PartPerThousand);

        [OutputParameter("ε_{cu1}", "Nominal ultimate strain")]
        public Ratio Epsiloncu1 => new(fck.As(_unit) >= 50
                    ? 2.8 + (27.0 * Math.Pow((98 - fcm.As(_unit)) / 100, 4))
                    : 3.5,
                    RatioUnit.PartPerThousand);

        [OutputParameter("ε_{c2}", "Simplified parabola-rectangle peak strain")]
        public Ratio Epsilonc2 => new(fck.As(_unit) >= 50
                    ? 2.0 + (0.085 * Math.Pow(fck.As(_unit) - 50, 0.53))
                    : 2.0,
                    RatioUnit.PartPerThousand);

        [OutputParameter("ε_{cu2}", "Simplified ultimate strain")]
        public Ratio Epsiloncu2 => new(fck.As(_unit) >= 50
                    ? 2.6 + (35.0 * Math.Pow((90 - fck.As(_unit)) / 100, 4))
                    : 3.5,
                    RatioUnit.PartPerThousand);

        [OutputParameter(@"\textit{n}", "Exponent")]
        public double n => fck.As(_unit) >= 50
                    ? 1.4 + (23.4 * Math.Pow((90 - fck.As(_unit)) / 100, 4))
                    : 2.0;

        [OutputParameter("ε_{c3}", "Simplified bi-linear peak strain")]
        public Ratio Epsilonc3 => new(fck.As(_unit) >= 50
                    ? 1.75 + (0.55 * ((fck.As(_unit) - 50) / 40))
                    : 1.75,
            RatioUnit.PartPerThousand);

        [OutputParameter("ε_{cu3}", "Simplified ultimate strain")]
        public Ratio Epsiloncu3 => new(fck.As(_unit) >= 50
                    ? 2.6 + (35.0 * Math.Pow((90 - fck.As(_unit)) / 100, 4))
                    : 3.5,
            RatioUnit.PartPerThousand);

        private static PressureUnit _unit = PressureUnit.Megapascal;

        public ConcreteMaterialProperties()
        {
            Calculate();
        }
    }
}
