using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockImty
{
    public partial class HisMediStockImtyManager : BusinessBase
    {
        public HisMediStockImtyManager()
            : base()
        {

        }

        public HisMediStockImtyManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_MEDI_STOCK_IMTY> Get(HisMediStockImtyFilterQuery filter)
        {
            List<HIS_MEDI_STOCK_IMTY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisMediStockImtyGet(param).Get(filter);
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

        public List<HIS_MEDI_STOCK_IMTY> GetByMediStockId(long mediStockId)
        {
            List<HIS_MEDI_STOCK_IMTY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                if (valid)
                {
                    result = new HisMediStockImtyGet(param).GetByMediStockId(mediStockId);
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

        public HIS_MEDI_STOCK_IMTY GetByMediStockIdAndExpMestTypeId(long mediStockId, long impMestTypeId)
        {
            HIS_MEDI_STOCK_IMTY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                if (valid)
                {
                    result = new HisMediStockImtyGet(param).GetByMediStockIdAndImpMestTypeId(mediStockId, impMestTypeId);
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
