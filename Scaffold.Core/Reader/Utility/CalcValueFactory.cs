using System.Collections;

namespace Scaffold.Reader.Utility
{
    public static class CalcValueFactory
    {
        public static ICalcValue CreateWrapperGeneric<T>(IList collection, int index)
        {
            Func<T> getter = () => (T)collection[index];
            Action<T> setter = (val) => collection[index] = val;
            string name = $"[{index}]";

            return new DelegateCalcValue<T>(getter, setter, string.Empty, name, null);
        }
    }
}
