using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServDebt
{
    partial class HisSereServDebtGet : BusinessBase
    {
        internal HisSereServDebtGet()
            : base()
        {

        }

        internal HisSereServDebtGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERE_SERV_DEBT> Get(HisSereServDebtFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServDebtDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_DEBT GetById(long id)
        {
            try
            {
                return GetById(id, new HisSereServDebtFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_DEBT GetById(long id, HisSereServDebtFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServDebtDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_DEBT> GetByIds(List<long> ids)
        {
            try
            {
                HisSereServDebtFilterQuery filter = new HisSereServDebtFilterQuery();
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

        internal List<HIS_SERE_SERV_DEBT> GetBySereServIds(List<long> sereServIds)
        {
            try
            {
                HisSereServDebtFilterQuery filter = new HisSereServDebtFilterQuery();
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

        internal List<HIS_SERE_SERV_DEBT> GetNoCancelBySereServIds(List<long> sereServIds)
        {
            try
            {
                HisSereServDebtFilterQuery filter = new HisSereServDebtFilterQuery();
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

        internal List<HIS_SERE_SERV_DEBT> GetNoCancelBySereServId(long sereServId)
        {
            try
            {
                HisSereServDebtFilterQuery filter = new HisSereServDebtFilterQuery();
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

        internal List<HIS_SERE_SERV_DEBT> GetByDebtId(long DebtId)
        {
            try
            {
                HisSereServDebtFilterQuery filter = new HisSereServDebtFilterQuery();
                filter.DEBT_ID = DebtId;
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
