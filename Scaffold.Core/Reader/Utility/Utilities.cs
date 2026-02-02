// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Reader.Utility
{
    public static class Utilities
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
