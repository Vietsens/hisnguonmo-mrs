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
        internal HisSeseDepoRepayGet()
            : base()
        {

        }

        internal HisSeseDepoRepayGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SESE_DEPO_REPAY> Get(HisSeseDepoRepayFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSeseDepoRepayDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SESE_DEPO_REPAY GetById(long id)
        {
            try
            {
                return GetById(id, new HisSeseDepoRepayFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SESE_DEPO_REPAY GetById(long id, HisSeseDepoRepayFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSeseDepoRepayDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SESE_DEPO_REPAY> GetBySereServDepositIds(List<long> sereServDepositIds)
        {
            try
            {
                HisSeseDepoRepayFilterQuery filter = new HisSeseDepoRepayFilterQuery();
                filter.SERE_SERV_DEPOSIT_IDs = sereServDepositIds;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SESE_DEPO_REPAY> GetNoCancelBySereServDepositIds(List<long> sereServDepositIds)
        {
            try
            {
                HisSeseDepoRepayFilterQuery filter = new HisSeseDepoRepayFilterQuery();
                filter.SERE_SERV_DEPOSIT_IDs = sereServDepositIds;
                filter.IS_CANCEL = false;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SESE_DEPO_REPAY> GetNoCancelBySereServDepositId(long sereServDepositId)
        {
            try
            {
                HisSeseDepoRepayFilterQuery filter = new HisSeseDepoRepayFilterQuery();
                filter.SERE_SERV_DEPOSIT_ID = sereServDepositId;
                filter.IS_CANCEL = false;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SESE_DEPO_REPAY> GetByRepayId(long repayId)
        {
            try
            {
                HisSeseDepoRepayFilterQuery filter = new HisSeseDepoRepayFilterQuery();
                filter.REPAY_ID = repayId;
                return this.Get(filter);
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
