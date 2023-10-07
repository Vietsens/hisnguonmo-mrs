using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.ReportException
{
    class ReportTypeNotFoundException : Exception
    {
        internal ReportTypeNotFoundException()
        {
        }

        internal ReportTypeNotFoundException(string reportType)
            : base(string.Format("Khong tim thay loai bao cao: {0} ", reportType != null ? "" : reportType))
        {
        }
    }
}
