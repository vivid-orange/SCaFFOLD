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
    public  class InteractiveGeometryQuantityOnX : IInteractiveGeometryItem
    {
        ICalcSIQuantity _quantity;

        double[] _position = [0, 0, 0];
        int[] _constraints = [1, 0, 0];
        public double[] Position
        {
            get
            {
                _position[0] = _quantity.Value;
                return _position;
            }
            set
            {
                _quantity.Value = value[0];
                _position[0] = _quantity.Value;
            }
        }
        public int[] Constraints => _constraints;

        public InteractiveGeometryQuantityOnX(ICalcSIQuantity quantity)
        {
            _quantity = quantity;
        }
    }
}
