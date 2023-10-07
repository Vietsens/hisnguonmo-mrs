using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockExty
{
    public partial class HisMediStockExtyManager : BusinessBase
    {
        public HisMediStockExtyManager()
            : base()
        {

        }

        public HisMediStockExtyManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_MEDI_STOCK_EXTY> Get(HisMediStockExtyFilterQuery filter)
        {
            List<HIS_MEDI_STOCK_EXTY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisMediStockExtyGet(param).Get(filter);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public List<HIS_MEDI_STOCK_EXTY> GetByMediStockId(long mediStockId)
        {
            List<HIS_MEDI_STOCK_EXTY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                if (valid)
                {
                    result = new HisMediStockExtyGet(param).GetByMediStockId(mediStockId);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public List<HIS_MEDI_STOCK_EXTY> GetByExpMestTypeId(long expMestTypeId)
        {
            List<HIS_MEDI_STOCK_EXTY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                if (valid)
                {
                    result = new HisMediStockExtyGet(param).GetByExpMestTypeId(expMestTypeId);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public HIS_MEDI_STOCK_EXTY GetByMediStockIdAndExpMestTypeId(long mediStockId, long expMestTypeId)
        {
            HIS_MEDI_STOCK_EXTY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                if (valid)
                {
                    result = new HisMediStockExtyGet(param).GetByMediStockIdAndExpMestTypeId(mediStockId, expMestTypeId);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
