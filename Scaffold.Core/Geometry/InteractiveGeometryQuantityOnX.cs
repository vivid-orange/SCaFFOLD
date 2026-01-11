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
        bool _centred = false;
        double _yCoordinate = 0;

        double[] _position = [0, 0, 0];
        int[] _constraints = [1, 0, 0];

        public double PositionX
        {
            get
            {
                if (_centred)
                { return _quantity.Value / 2; }
                else
                { return _quantity.Value; }
            }
            set
            {
                if (_centred)
                { _quantity.Value = value * 2; }
                else
                { _quantity.Value = value; }
            }
        }

        public double PositionY
        {
            get
            {
                return _yCoordinate;
            }
            set
            {
                
            }
        }

        public int[] Constraints => _constraints;

        public string Symbol => _quantity.Symbol;

        public string Summary => _quantity.GetValueAsString();

        public InteractiveGeometryQuantityOnX(ICalcSIQuantity quantity, double ydir)
        {
            _quantity = quantity;
            _yCoordinate = ydir;
        }

        public InteractiveGeometryQuantityOnX(ICalcSIQuantity quantity, double ydir, bool centred)
        {
            _quantity = quantity;
            _yCoordinate = ydir;
            _centred = centred;
        }
    }
}
