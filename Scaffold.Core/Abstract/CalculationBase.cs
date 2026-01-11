using Scaffold.Core.Attributes;
using Scaffold.Core.Enums;
using Scaffold.Core.CalcValues;
using Scaffold.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Core.Abstract
{
    public abstract class CalculationBase : ICalculation
    {
        public string TypeName
        { get
            {
                return GetType().Name;
            }
        }
        public virtual string InstanceName { get; set; }

        public CalcStatus Status { get; }

        public abstract List<IOutputItem> GetFormulae();

        public List<ICalcValue> GetInputs()
        {
            return GetProperties<InputCalcValueAttribute>();
        }

        public List<ICalcValue> GetOutputs()
        {
            return GetProperties<OutputCalcValueAttribute>();
        }

        public abstract void Calculate();


        private List<ICalcValue> GetProperties<TAttribute>() where TAttribute : Attribute
        {
            var matchingValues = new List<ICalcValue>();

            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //var fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttribute<TAttribute>();
                if (attr == null) continue;

                if (typeof(ICalcValue).IsAssignableFrom(prop.PropertyType))
                {
                    var value = prop.GetValue(this);

                    if (value is ICalcValue castedValue)
                    {
                        matchingValues.Add(castedValue);
                    }
                }
            }

            return matchingValues;
        }
    }
}
