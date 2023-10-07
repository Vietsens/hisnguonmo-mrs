using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockMaty
{
    public partial class HisMediStockMatyManager : BusinessBase
    {
        
        public List<V_HIS_MEDI_STOCK_MATY> GetView(HisMediStockMatyViewFilterQuery filter)
        {
            List<V_HIS_MEDI_STOCK_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEDI_STOCK_MATY> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMatyGet(param).GetView(filter);
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

        
        public V_HIS_MEDI_STOCK_MATY GetViewById(long data)
        {
            V_HIS_MEDI_STOCK_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDI_STOCK_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMatyGet(param).GetViewById(data);
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

        
        public V_HIS_MEDI_STOCK_MATY GetViewById(long data, HisMediStockMatyViewFilterQuery filter)
        {
            V_HIS_MEDI_STOCK_MATY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDI_STOCK_MATY resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockMatyGet(param).GetViewById(data, filter);
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
