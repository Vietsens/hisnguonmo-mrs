using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.ReportException
{
    class ReportTemplateNotFoundException : Exception
    {
        internal ReportTemplateNotFoundException()
        {
        }

        internal ReportTemplateNotFoundException(string templateCode)
            : base(string.Format("Khong tim thay mau bao cao: {0} ", templateCode != null ? "" : templateCode))
        {
        }
    }
}
