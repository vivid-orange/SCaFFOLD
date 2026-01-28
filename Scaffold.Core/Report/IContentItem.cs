using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Report
{
    public interface IContentItem
    {
        public bool IncludeInSummary { get; }
        public bool IsInLine { get; }
    }
}
