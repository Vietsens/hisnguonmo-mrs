using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBillFund
{
    partial class HisBillFundGet : BusinessBase
    {
        internal List<V_HIS_BILL_FUND> GetView(HisBillFundViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBillFundDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BILL_FUND GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisBillFundViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BILL_FUND GetViewById(long id, HisBillFundViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBillFundDAO.GetViewById(id, filter.Query());
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
