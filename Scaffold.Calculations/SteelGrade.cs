using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core;
using Scaffold.Core.Abstract;
using Scaffold.Core.Attributes;
using Scaffold.Core.CalcValues;
using Scaffold.Core.Enums;
using Scaffold.Core.Interfaces;
using UnitsNet;

namespace Scaffold.Calculations
{
    public class SteelGrade : CalculationBase, ICalcValue
    {
        public override string InstanceName { get; set; } = "Beam grade";

        public string TypeName => "Steel grade";

        public new CalcStatus Status { get; set; }


        [InputCalcValue]
        public CalcSelectionList SteelGrades { get; } = new("Steel grade", 1, ["S275", "S355", "S460"]);

        [OutputCalcValue]
        public CalcSIQuantity<Pressure> Gradestrength { get; } = new("Grade strength", "p", new Pressure(260, UnitsNet.Units.PressureUnit.NewtonPerSquareMillimeter));
        public string Symbol { get => ""; }

        public SteelGrade()
        {
            Calculate();
        }



        public override List<IOutputItem> GetFormulae()
        {
            return null;
        }

        public override void Calculate()
        {
            switch (SteelGrades.SelectedItemIndex)
            {
                case 0: Gradestrength.Quantity = new Pressure(275, UnitsNet.Units.PressureUnit.NewtonPerSquareMillimeter); break;
                case 1: Gradestrength.Quantity = new Pressure(355, UnitsNet.Units.PressureUnit.NewtonPerSquareMillimeter); break;
                case 2: Gradestrength.Quantity = new Pressure(460, UnitsNet.Units.PressureUnit.NewtonPerSquareMillimeter); break;
            }
        }

        public bool TryParse(string strValue)
        {
            return false;
        }

        public string GetValueAsString()
        {
            return SteelGrades.Value.ToString();
        }
    }
}
