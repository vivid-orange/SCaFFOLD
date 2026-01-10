// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core.CalcValues;

namespace Scaffold.Core.Geometry
{
    public class InteractiveGeometryDoubleArrays : IInteractiveGeometryItem
    {
        ICalcListOfDoubleArrays _quantity;

        double[] _position = [0, 0, 0];
        int[] _constraints = [1, 1, 0];
        public double[] Position
        {
            get
            {
                _position[0] = _quantity.Value[0][0];
                _position[1] = _quantity.Value[0][1];
                return _position;
            }
            set
            {
                _quantity.Value[0][0] = value[0];
                _quantity.Value[1][0] = value[1];

            }
        }
        public int[] Constraints => _constraints;

        public InteractiveGeometryDoubleArrays(ICalcListOfDoubleArrays listofdoublearrays)
        {
            _quantity = listofdoublearrays;
        }
    }
}
