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

        int[] _constraints = [1, 1, 0];

        public int[] Constraints => _constraints;

        public double PositionX
        {
            get
            {
                return _quantity[0];
            }
            set
            {
                _quantity[0] = value;
            }
        }

        public double PositionY
        {
            get
            {
                return _quantity[1];
            }
            set
            {
                _quantity[1] = value;
            }
        }

        string _symbol = "";
        public string Symbol => _symbol;

        public string Summary => "";

        public InteractiveGeometryDoubleArrays(string symbol, double[] doubleArray)
        {
            _quantity = doubleArray;
            _symbol = symbol;
        }
    }
}
