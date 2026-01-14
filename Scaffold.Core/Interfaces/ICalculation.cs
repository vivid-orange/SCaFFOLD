using System.Collections.Generic;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Geometry;

namespace Scaffold.Core.Interfaces
{
    public interface ICalculation : ICalculationStatus
    {
        /// <summary>
        /// The name of the instance e.g. 'Column C3'
        /// </summary>
        string InstanceName { get; set; }

        /// <summary>
        /// Returns the calculation steps/report.
        /// </summary>
        List<IOutputItem> GetFormulae();

        /// <summary>
        /// Performs the calculation logic.
        /// </summary>
        void Calculate();

        // Note: GetInputs() and GetOutputs() removed. 
        // Use CalculationReader.GetInputs(calc) and CalculationReader.GetOutputs(calc) instead.
    }
}
