using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockImty
{
    public partial class HisMediStockImtyManager : BusinessBase
    {
        public List<V_HIS_MEDI_STOCK_IMTY> GetView(HisMediStockImtyViewFilterQuery filter)
        {
            List<V_HIS_MEDI_STOCK_IMTY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisMediStockImtyGet(param).GetView(filter);
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
