using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServDeposit
{
    partial class HisSereServDepositGet : BusinessBase
    {
        internal HisSereServDepositGet()
            : base()
        {

        }

        internal HisSereServDepositGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERE_SERV_DEPOSIT> Get(HisSereServDepositFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServDepositDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_DEPOSIT GetById(long id)
        {
            try
            {
                return GetById(id, new HisSereServDepositFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_DEPOSIT GetById(long id, HisSereServDepositFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServDepositDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_DEPOSIT> GetByIds(List<long> ids)
        {
            try
            {
                HisSereServDepositFilterQuery filter = new HisSereServDepositFilterQuery();
                filter.IDs = ids;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_DEPOSIT> GetBySereServIds(List<long> sereServIds)
        {
            try
            {
                HisSereServDepositFilterQuery filter = new HisSereServDepositFilterQuery();
                filter.SERE_SERV_IDs = sereServIds;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_DEPOSIT> GetNoCancelBySereServIds(List<long> sereServIds)
        {
            try
            {
                HisSereServDepositFilterQuery filter = new HisSereServDepositFilterQuery();
                filter.SERE_SERV_IDs = sereServIds;
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

        internal List<HIS_SERE_SERV_DEPOSIT> GetNoCancelBySereServId(long sereServId)
        {
            try
            {
                HisSereServDepositFilterQuery filter = new HisSereServDepositFilterQuery();
                filter.SERE_SERV_ID = sereServId;
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

        internal List<HIS_SERE_SERV_DEPOSIT> GetByDepositId(long depositId)
        {
            try
            {
                HisSereServDepositFilterQuery filter = new HisSereServDepositFilterQuery();
                filter.DEPOSIT_ID = depositId;
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
