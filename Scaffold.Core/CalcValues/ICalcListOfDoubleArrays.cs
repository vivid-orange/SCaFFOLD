using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Core.CalcValues
{
    public interface ICalcListOfDoubleArrays : ICalcValue
    {
        List<double[]> Value { get; }
    }
}
