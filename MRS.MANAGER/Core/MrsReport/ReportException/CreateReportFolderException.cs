using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.ReportException
{
    class CreateReportFolderException : Exception
    {
        internal CreateReportFolderException() { }
        internal CreateReportFolderException(string message) : base(message) { }
    }
}
