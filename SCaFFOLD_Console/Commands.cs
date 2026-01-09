using Scaffold.Core.Abstract;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Interfaces;

namespace SCaFFOLD_Console
{
    internal static class Commands
    {
        internal static void DisplayInputs(ICalculation calc, string s)
        {
            IReadOnlyList<ICalcValue> values = reader.GetInputs(calc);
            Console.WriteLine("INPUTS");
            Console.WriteLine("Idx Name           Value");
            for (int i = 0; i < values.Count; i++)
            {
                ICalcValue input = values[i];
                Console.Write(i.ToString().PadRight(4));
                Console.Write(input.Symbol.PadRight(15));
                Console.WriteLine(input.GetValue());
            }
        }
        internal static void DisplayResults(ICalculation calc, string s)
        {
            calc.Calculate();
            IReadOnlyList<ICalcValue> values = reader.GetOutputs(calc);
            Console.WriteLine("CALCULATED VALUES");
            Console.WriteLine("Idx Name           Value");
            for (int i = 0; i < values.Count; i++)
            {
                ICalcValue input = values[i];
                Console.Write(i.ToString().PadRight(4));
                Console.Write(input.Symbol.PadRight(15));
                Console.WriteLine(input.GetValue());
            }
        }

        internal static void ChangeInput(ICalculation calc, string s)
        {
            string[] words = s.Split(' ');
            IReadOnlyList<ICalcValue> values = reader.GetInputs(calc);
            int idx = -1;
            int.TryParse(words[0], out idx);
            if (idx >= 0)
            {
                values[idx].SetValue(words[1]);
                calc.Calculate();
                DisplayResults(calc, "");
            }
        }


        internal static Dictionary<string, Action<ICalculation, string>> commands;

        internal static CalculationReader reader = new CalculationReader();

        internal static Dictionary<string, Action<ICalculation, string>> getCommands()
        {
            if (commands != null)
            { return commands; }
            else
            {
                commands = new Dictionary<string, Action<ICalculation, string>>();
                commands.Add("inputs", (c, s) => DisplayInputs(c, s));
                commands.Add("results", (c, s) => DisplayResults(c, s));
                commands.Add("change", (c, s) => ChangeInput(c, s));
                return commands;
            }
        }

    }
}
