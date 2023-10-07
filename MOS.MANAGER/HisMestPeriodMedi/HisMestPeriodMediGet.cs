using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodMedi
{
    class HisMestPeriodMediGet : GetBase
    {
        internal HisMestPeriodMediGet()
            : base()
        {

        }

        internal HisMestPeriodMediGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEST_PERIOD_MEDI> Get(HisMestPeriodMediFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMediDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEST_PERIOD_MEDI> GetView(HisMestPeriodMediViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMediDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PERIOD_MEDI GetById(long id)
        {
            try
            {
                return GetById(id, new HisMestPeriodMediFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PERIOD_MEDI GetById(long id, HisMestPeriodMediFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMediDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_MEST_PERIOD_MEDI GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMestPeriodMediViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEST_PERIOD_MEDI GetViewById(long id, HisMestPeriodMediViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMediDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEST_PERIOD_MEDI> GetByMediStockPeriodId(long id)
        {
            try
            {
                HisMestPeriodMediFilterQuery filter = new HisMestPeriodMediFilterQuery();
                filter.MEDI_STOCK_PERIOD_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEST_PERIOD_MEDI> GetByMedicineId(long id)
        {
            try
            {
                HisMestPeriodMediFilterQuery filter = new HisMestPeriodMediFilterQuery();
                filter.MEDICINE_ID = id;
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
