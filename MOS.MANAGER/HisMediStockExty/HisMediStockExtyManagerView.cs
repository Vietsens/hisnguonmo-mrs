using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockExty
{
    public partial class HisMediStockExtyManager : BusinessBase
    {
        public List<V_HIS_MEDI_STOCK_EXTY> GetView(HisMediStockExtyViewFilterQuery filter)
        {
            List<V_HIS_MEDI_STOCK_EXTY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisMediStockExtyGet(param).GetView(filter);
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
