using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.ReportException
{
    class TemplateFileNotFoundException : Exception 
    {
        internal TemplateFileNotFoundException() { }
        internal TemplateFileNotFoundException(string message) : base(message) { }
    }
}
