using SAR.EFMODEL.DataModels;
using SAR.MANAGER.Core.SarRetyFofi;
using Inventec.Core;
using System;

namespace SAR.MANAGER.Core.Check
{
    class SarRetyFofiCheckVerifyId
    {
        internal static bool Verify(CommonParam param, long id)
        {
            bool result = true;
            try
            {
                SAR_RETY_FOFI raw = new SarRetyFofiBO().Get<SAR_RETY_FOFI>(id);
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

        internal static bool Verify(CommonParam param, long id, ref SAR_RETY_FOFI raw)
        {
            bool result = true;
            try
            {
                raw = new SarRetyFofiBO().Get<SAR_RETY_FOFI>(id);
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
