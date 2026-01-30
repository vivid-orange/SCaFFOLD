namespace Scaffold.Calculations
{
    public class TestCalculation : Calculation
    {
        [InputParameter(@"D", "Multiplier")]
        public double Multiplier { get; set; } = 0;

        [InputParameter(@"F", "Force")]
        public Force Force { get; set; } = new Force(10, ForceUnit.Kilonewton);

        [OutputParameter(@"R", "Result")]
        public double Result { get; private set; } = 0;

        public override void Calculate()
        {
            Result = Force.Value * Multiplier;
        }
    }
}
