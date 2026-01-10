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

        public double PositionX
        {
            get
            {
                return _position[0];
            }
            set
            {
                _position[0] = value;
            }
        }

        public double PositionY
        {
            get
            {
                return _position[1];
            }
            set
            {
                _position[1] = value;
            }
        }

        public int[] Constraints => _constraints;

        public string Symbol => _quantity.Symbol;

        public string Summary => _quantity.GetValueAsString();

        public InteractiveGeometryQuantityOnX(ICalcSIQuantity quantity)
        {
            _quantity = quantity;
        }
    }
}
