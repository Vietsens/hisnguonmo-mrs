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
        internal HIS_DEPOSIT_REQ GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisDepositReqFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DEPOSIT_REQ GetByCode(string code, HisDepositReqFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepositReqDAO.GetByCode(code, filter.Query());
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
