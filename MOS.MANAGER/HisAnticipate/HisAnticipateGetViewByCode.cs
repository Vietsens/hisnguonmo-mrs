using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipate
{
    partial class HisAnticipateGet : BusinessBase
    {
        internal V_HIS_ANTICIPATE GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisAnticipateViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_ANTICIPATE GetViewByCode(string code, HisAnticipateViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
