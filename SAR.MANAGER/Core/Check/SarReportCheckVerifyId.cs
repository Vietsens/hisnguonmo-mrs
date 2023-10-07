using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Core.SarReport;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.Check
{
    class SarReportCheckVerifyId
    {
        internal static bool Verify(CommonParam param, long id)
        {
            bool result = true;
            try
            {
                SAR_REPORT raw = new SarReportBO().Get<SAR_REPORT>(id);
                if (raw == null)
                {
                    result = false;
                    Inventec.Common.Logging.LogSystem.Warn("Khong lay duoc sar_report 2." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => raw), raw));
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

        internal static bool Verify(CommonParam param, long id, ref SAR_REPORT raw)
        {
            bool result = true;
            try
            {
                raw = new SarReportBO().Get<SAR_REPORT>(id);
                if (raw == null)
                {
                    result = false;
                    Inventec.Common.Logging.LogSystem.Warn("Khong lay duoc sar_report 3.");
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
