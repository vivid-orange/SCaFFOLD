// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core;

namespace Scaffold.Calculations
{
    public class EmbeddedCalc : ICalculation
    {
        public string CalculationTitle { get; set; } = "";

        public string EntityLabel => "Embedded calc";

        public CalcStatus Status => CalcStatus.None;

        [CalcValueType(CalcValueType.Input, "H", "Column height")]
        public Length ColumnHeight { get; set; } = new Length(4.5, LengthUnit.Meter);

        [CalcValueType(CalcValueType.Output, "H", "Reduced column height")]
        public Length ReducedColumnHeight { get; private set; } = new Length(0, LengthUnit.Meter);

        public void Calculate()
        {
            ReducedColumnHeight = ColumnHeight / 2;
        }
        public IList<IOutputItem> GetFormulae() => new List<IOutputItem>();
    }
}
