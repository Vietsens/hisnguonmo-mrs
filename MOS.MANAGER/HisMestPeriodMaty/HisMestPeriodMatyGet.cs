using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodMaty
{
    class HisMestPeriodMatyGet : GetBase
    {
        internal HisMestPeriodMatyGet()
            : base()
        {

        }

        internal HisMestPeriodMatyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEST_PERIOD_MATY> Get(HisMestPeriodMatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMatyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEST_PERIOD_MATY> GetView(HisMestPeriodMatyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMatyDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PERIOD_MATY GetById(long id)
        {
            try
            {
                return GetById(id, new HisMestPeriodMatyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_PERIOD_MATY GetById(long id, HisMestPeriodMatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMatyDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_MEST_PERIOD_MATY GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMestPeriodMatyViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEST_PERIOD_MATY GetViewById(long id, HisMestPeriodMatyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPeriodMatyDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEST_PERIOD_MATY> GetByMaterialTypeId(long id)
        {
            try
            {
                HisMestPeriodMatyFilterQuery filter = new HisMestPeriodMatyFilterQuery();
                filter.MATERIAL_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEST_PERIOD_MATY> GetByMediStockPeriodId(long id)
        {
            try
            {
                HisMestPeriodMatyFilterQuery filter = new HisMestPeriodMatyFilterQuery();
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
    }
}
