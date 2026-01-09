using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace Scaffold.Core.CalcValues
{
    public interface ISteelProfile : IComplex
    {
        ISIQuantity<Area> Area { get; }
    }
}
