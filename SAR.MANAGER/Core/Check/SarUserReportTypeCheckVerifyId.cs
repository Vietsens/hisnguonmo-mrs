using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Core.SarUserReportType;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.Check
{
    class SarUserReportTypeCheckVerifyId
    {
        internal static bool Verify(CommonParam param, long id)
        {
            bool result = true;
            try
            {
                SAR_USER_REPORT_TYPE raw = new SarUserReportTypeBO().Get<SAR_USER_REPORT_TYPE>(id);
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

        internal static bool Verify(CommonParam param, long id, ref SAR_USER_REPORT_TYPE raw)
        {
            bool result = true;
            try
            {
                raw = new SarUserReportTypeBO().Get<SAR_USER_REPORT_TYPE>(id);
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
