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
    public class InteractiveGeometryQuantityOnY : IInteractiveGeometryItem
    {
        ICalcSIQuantity _quantity;
        bool _centred = false;
        double _xCoordinate = 0;

        int[] _constraints = [0, 1, 0];

        public double PositionX
        {
            get
            {
                return _xCoordinate;
            }
            set
            {
            
            }
        }

        public double PositionY
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
        public int[] Constraints => _constraints;

        public string Symbol => _quantity.Symbol;

        public string Summary => _quantity.GetValueAsString();
        public InteractiveGeometryQuantityOnY(double xCoordinate, ICalcSIQuantity quantity)
        {
            _quantity = quantity;
            _xCoordinate = xCoordinate;
        }

        public InteractiveGeometryQuantityOnY(double xCoordinate, ICalcSIQuantity quantity, bool centred)
        {
            _quantity = quantity;
            _xCoordinate = xCoordinate;
            _centred = centred;
        }
    }
}
