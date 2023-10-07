using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodMate
{
    class HisMestPeriodMateGet : GetBase
    {
        internal HisMestPeriodMateGet()
            : base()
        {

        }

        internal HisMestPeriodMateGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEST_PERIOD_MATE> Get(HisMestPeriodMateFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMateDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEST_PERIOD_MATE> GetView(HisMestPeriodMateViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMateDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PERIOD_MATE GetById(long id)
        {
            try
            {
                return GetById(id, new HisMestPeriodMateFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PERIOD_MATE GetById(long id, HisMestPeriodMateFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMateDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_MEST_PERIOD_MATE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMestPeriodMateViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEST_PERIOD_MATE GetViewById(long id, HisMestPeriodMateViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMateDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEST_PERIOD_MATE> GetByMediStockPeriodId(long id)
        {
            try
            {
                HisMestPeriodMateFilterQuery filter = new HisMestPeriodMateFilterQuery();
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

        internal List<HIS_MEST_PERIOD_MATE> GetByMaterialId(long id)
        {
            try
            {
                HisMestPeriodMateFilterQuery filter = new HisMestPeriodMateFilterQuery();
                filter.MATERIAL_ID = id;
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
