using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockImty
{
    partial class HisMediStockImtyGet : BusinessBase
    {
        internal HisMediStockImtyGet()
            : base()
        {

        }

        internal HisMediStockImtyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDI_STOCK_IMTY> Get(HisMediStockImtyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediStockImtyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_STOCK_IMTY GetById(long id)
        {
            try
            {
                return GetById(id, new HisMediStockImtyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_STOCK_IMTY GetById(long id, HisMediStockImtyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediStockImtyDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDI_STOCK_IMTY> GetByMediStockId(long mediStockId)
        {
            try
            {
                HisMediStockImtyFilterQuery filter = new HisMediStockImtyFilterQuery();
                filter.MEDI_STOCK_ID = mediStockId;
                return Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_STOCK_IMTY GetByMediStockIdAndImpMestTypeId(long mediStockId, long impMestTypeId)
        {
            try
            {
                HisMediStockImtyFilterQuery filter = new HisMediStockImtyFilterQuery();
                filter.IMP_MEST_TYPE_ID = impMestTypeId;
                filter.MEDI_STOCK_ID = mediStockId;
                return Get(filter).SingleOrDefault();
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
