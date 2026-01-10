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
        double[] _quantity;

        double[] _position = [0, 0, 0];
        int[] _constraints = [1, 1, 0];
        public double[] Position
        {
            get
            {
                return _quantity;

            }
            set
            {

            }
        }
        public int[] Constraints => _constraints;

        public InteractiveGeometryDoubleArrays(double[] doubleArray)
        {
            _quantity = doubleArray;
        }
    }
}
