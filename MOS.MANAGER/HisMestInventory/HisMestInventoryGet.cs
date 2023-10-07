using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.MANAGER.HisMestInventory
{
    partial class HisMestInventoryGet : BusinessBase
    {
        internal HisMestInventoryGet()
            : base()
        {

        }

        internal HisMestInventoryGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEST_INVENTORY> Get(HisMestInventoryFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestInventoryDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_INVENTORY GetById(long id)
        {
            try
            {
                return GetById(id, new HisMestInventoryFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_INVENTORY GetById(long id, HisMestInventoryFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestInventoryDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEST_INVENTORY GetByMediStockPeriodId(long mediStockPeriodId)
        {
            try
            {
                HisMestInventoryFilterQuery filter = new HisMestInventoryFilterQuery();
                filter.MEDI_STOCK_PERIOD_ID = mediStockPeriodId;
                var rs = Get(filter);
                if (IsNotNullOrEmpty(rs))
                {
                    return rs.FirstOrDefault();
                }
                return null;
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
