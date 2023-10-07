using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockMety
{
    public partial class HisMediStockMetyManager : BusinessBase
    {
        
        public List<V_HIS_MEDI_STOCK_METY> GetView(HisMediStockMetyViewFilterQuery filter)
        {
            List<V_HIS_MEDI_STOCK_METY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEDI_STOCK_METY> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMetyGet(param).GetView(filter);
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

        
        public V_HIS_MEDI_STOCK_METY GetViewById(long data)
        {
            V_HIS_MEDI_STOCK_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDI_STOCK_METY resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMetyGet(param).GetViewById(data);
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

        
        public V_HIS_MEDI_STOCK_METY GetViewById(long data, HisMediStockMetyViewFilterQuery filter)
        {
            V_HIS_MEDI_STOCK_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDI_STOCK_METY resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMetyGet(param).GetViewById(data, filter);
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
