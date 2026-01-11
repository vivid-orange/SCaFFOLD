// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Interfaces;
using UnitsNet;
using UnitsNet.Units;

namespace Scaffold.Calculations.Eurocode.Concrete.PunchingShear
{
    public class EffectiveDepths : CalculationBase, ICalcValue
    {
        [InputCalcValue]
        public CalcSIQuantity<Length> Height { get; } = new ("Slab depth", "h", new Length(250, LengthUnit.Millimeter));
        [InputCalcValue]
        public CalcSIQuantity<Length> OffsetY { get;} = new ("Offset to effective depth y dir", "d_{y,offset}", new Length(45, LengthUnit.Millimeter));
        [InputCalcValue]
        public CalcSIQuantity<Length> OffsetZ { get; } = new ("Offset to effective depth z dir", "d_{z,offset}", new Length(65, LengthUnit.Millimeter));
        [OutputCalcValue]
        public CalcSIQuantity<Length> EffectiveDepthY { get; }  = new("Effective depth in Y direction", "d_y", new Length(0, LengthUnit.Millimeter));
        [OutputCalcValue]
        public CalcSIQuantity<Length> EffectiveDepthZ { get; } = new("Effective depth in Z direction", "d_z", new Length(0, LengthUnit.Millimeter));
        [OutputCalcValue]
        public CalcSIQuantity<Length> D_average { get; } = new ("Average effective depth", "d_{average}", new Length(205, LengthUnit.Millimeter));

        public string Symbol => "";

        public EffectiveDepths()
        {
            Calculate();
        }
        public override void Calculate()
        {
            EffectiveDepthY.Quantity = Height.Quantity - OffsetY.Quantity;
            EffectiveDepthZ.Quantity = Height.Quantity - OffsetZ.Quantity;
            D_average.Quantity = (EffectiveDepthY.Quantity + EffectiveDepthZ.Quantity) / 2.0;
        }
        public override List<IOutputItem> GetFormulae()
        {
            var returnList = new List<IOutputItem>();

            var out1 = new LatexItem(D_average.Symbol + @" = \frac{" + EffectiveDepthY.Symbol + "+" + EffectiveDepthZ.Symbol + "}{2} = " + D_average.Value );

            returnList.Add(new OutputItem("", "", "OK", out1));

            return returnList;
        }

        public bool TryParse(string strValue) { return false; }
        public string GetValueAsString() { return D_average.GetValueAsString(); }
    }
}
