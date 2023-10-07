using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockPeriod
{
    public partial class HisMediStockPeriodManager : BusinessBase
    {
        
        public List<V_HIS_MEDI_STOCK_PERIOD> GetView(HisMediStockPeriodViewFilterQuery filter)
        {
            List<V_HIS_MEDI_STOCK_PERIOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEDI_STOCK_PERIOD> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockPeriodGet(param).GetView(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public V_HIS_MEDI_STOCK_PERIOD GetViewById(long data)
        {
            V_HIS_MEDI_STOCK_PERIOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDI_STOCK_PERIOD resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockPeriodGet(param).GetViewById(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public V_HIS_MEDI_STOCK_PERIOD GetViewById(long data, HisMediStockPeriodViewFilterQuery filter)
        {
            V_HIS_MEDI_STOCK_PERIOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDI_STOCK_PERIOD resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockPeriodGet(param).GetViewById(data, filter);
                }
                result = resultData;
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
