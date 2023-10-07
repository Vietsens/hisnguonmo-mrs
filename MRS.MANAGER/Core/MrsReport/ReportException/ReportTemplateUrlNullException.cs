using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.ReportException
{
    class ReportTemplateUrlNullException : Exception
    {
        internal ReportTemplateUrlNullException() { }
        internal ReportTemplateUrlNullException(string message) : base(message) { }
    }
}
