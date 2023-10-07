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
        internal List<V_HIS_DEPOSIT_REQ> GetView(HisDepositReqViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepositReqDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_DEPOSIT_REQ GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisDepositReqViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_DEPOSIT_REQ GetViewById(long id, HisDepositReqViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDepositReqDAO.GetViewById(id, filter.Query());
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
