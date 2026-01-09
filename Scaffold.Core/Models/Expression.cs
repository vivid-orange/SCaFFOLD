using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Core
{
    public abstract class Expression : IExpression
    {
        public bool IncludeInSummary { get; } = true;

        public bool IsInLine { get; protected set; } = false;
    }
}
