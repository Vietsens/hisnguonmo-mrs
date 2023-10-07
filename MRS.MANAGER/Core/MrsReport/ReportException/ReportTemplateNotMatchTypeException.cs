using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.ReportException
{
    class ReportTemplateNotMatchTypeException : Exception 
    {
        internal ReportTemplateNotMatchTypeException() { }
        internal ReportTemplateNotMatchTypeException(string message) : base(message) { }
    }
}
