using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Core.SarReportTemplate;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.Check
{
    class SarReportTemplateCheckVerifyId
    {
        internal static bool Verify(CommonParam param, long id)
        {
            bool result = true;
            try
            {
                SAR_REPORT_TEMPLATE raw = new SarReportTemplateBO().Get<SAR_REPORT_TEMPLATE>(id);
                if (raw == null)
                {
                    result = false;
                    SAR.MANAGER.Base.BugUtil.SetBugCode(param, LibraryBug.Bug.Enum.Common__KXDDDuLieuCanXuLy);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
                param.HasException = true;
            }
            return result;
        }

        internal static bool Verify(CommonParam param, long id, ref SAR_REPORT_TEMPLATE raw)
        {
            bool result = true;
            try
            {
                raw = new SarReportTemplateBO().Get<SAR_REPORT_TEMPLATE>(id);
                if (raw == null)
                {
                    result = false;
                    SAR.MANAGER.Base.BugUtil.SetBugCode(param, LibraryBug.Bug.Enum.Common__KXDDDuLieuCanXuLy);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
                param.HasException = true;
            }
            return result;
        }
    }
}
