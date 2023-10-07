using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSeseDepoRepay
{
    partial class HisSeseDepoRepayGet : BusinessBase
    {
        internal List<V_HIS_SESE_DEPO_REPAY> GetView(HisSeseDepoRepayViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSeseDepoRepayDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SESE_DEPO_REPAY GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisSeseDepoRepayViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SESE_DEPO_REPAY GetViewById(long id, HisSeseDepoRepayViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSeseDepoRepayDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SESE_DEPO_REPAY> GetViewByRepayId(long repayId)
        {
            try
            {
                HisSeseDepoRepayViewFilterQuery filter = new HisSeseDepoRepayViewFilterQuery();
                filter.REPAY_ID = repayId;
                return this.GetView(filter);
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
