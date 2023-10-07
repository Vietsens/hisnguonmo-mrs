using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockExty
{
    partial class HisMediStockExtyGet : BusinessBase
    {
        internal HisMediStockExtyGet()
            : base()
        {

        }

        internal HisMediStockExtyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDI_STOCK_EXTY> Get(HisMediStockExtyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediStockExtyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_STOCK_EXTY GetById(long id)
        {
            try
            {
                return GetById(id, new HisMediStockExtyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_STOCK_EXTY GetById(long id, HisMediStockExtyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediStockExtyDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDI_STOCK_EXTY> GetByMediStockId(long mediStockId)
        {
            try
            {
                HisMediStockExtyFilterQuery filter = new HisMediStockExtyFilterQuery();
                filter.MEDI_STOCK_ID = mediStockId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDI_STOCK_EXTY> GetByExpMestTypeId(long expMestTypeId)
        {
            try
            {
                HisMediStockExtyFilterQuery filter = new HisMediStockExtyFilterQuery();
                filter.EXP_MEST_TYPE_ID = expMestTypeId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_STOCK_EXTY GetByMediStockIdAndExpMestTypeId(long mediStockId, long expMestTypeId)
        {
            try
            {
                HisMediStockExtyFilterQuery filter = new HisMediStockExtyFilterQuery();
                filter.EXP_MEST_TYPE_ID = expMestTypeId;
                filter.MEDI_STOCK_ID = mediStockId;
                return this.Get(filter).SingleOrDefault();
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
