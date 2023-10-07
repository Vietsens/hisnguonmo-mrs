using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDepositReq
{
    partial class HisDepositReqGet : BusinessBase
    {
        internal V_HIS_DEPOSIT_REQ GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisDepositReqViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_DEPOSIT_REQ GetViewByCode(string code, HisDepositReqViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepositReqDAO.GetViewByCode(code, filter.Query());
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
