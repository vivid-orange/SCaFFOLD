using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Core
{
    public interface IExpression
    {
        public bool IncludeInSummary { get; }
        public bool IsInLine { get; }
    }
}
