using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodMety
{
    class HisMestPeriodMetyGet : GetBase
    {
        internal HisMestPeriodMetyGet()
            : base()
        {

        }

        internal HisMestPeriodMetyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEST_PERIOD_METY> Get(HisMestPeriodMetyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMetyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEST_PERIOD_METY> GetView(HisMestPeriodMetyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMetyDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PERIOD_METY GetById(long id)
        {
            try
            {
                return GetById(id, new HisMestPeriodMetyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PERIOD_METY GetById(long id, HisMestPeriodMetyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMetyDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_MEST_PERIOD_METY GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMestPeriodMetyViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEST_PERIOD_METY GetViewById(long id, HisMestPeriodMetyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMetyDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEST_PERIOD_METY> GetByMediStockPeriodId(long id)
        {
            try
            {
                HisMestPeriodMetyFilterQuery filter = new HisMestPeriodMetyFilterQuery();
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

        internal List<HIS_MEST_PERIOD_METY> GetByMedicineTypeId(long id)
        {
            try
            {
                HisMestPeriodMetyFilterQuery filter = new HisMestPeriodMetyFilterQuery();
                filter.MEDICINE_TYPE_ID = id;
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
