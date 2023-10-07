using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStock
{
    public partial class HisMediStockManager : BusinessBase
    {
        
        public List<V_HIS_MEDI_STOCK> GetView(HisMediStockViewFilterQuery filter)
        {
            List<V_HIS_MEDI_STOCK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEDI_STOCK> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockGet(param).GetView(filter);
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

        
        public V_HIS_MEDI_STOCK GetViewByCode(string data)
        {
            V_HIS_MEDI_STOCK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDI_STOCK resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockGet(param).GetViewByCode(data);
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

        
        public V_HIS_MEDI_STOCK GetViewByCode(string data, HisMediStockViewFilterQuery filter)
        {
            V_HIS_MEDI_STOCK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDI_STOCK resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_MEDI_STOCK GetViewById(long data)
        {
            V_HIS_MEDI_STOCK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEDI_STOCK resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockGet(param).GetViewById(data);
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

        
        public V_HIS_MEDI_STOCK GetViewById(long data, HisMediStockViewFilterQuery filter)
        {
            V_HIS_MEDI_STOCK result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEDI_STOCK resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_MEDI_STOCK> GetViewActive()
        {
            List<V_HIS_MEDI_STOCK> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<V_HIS_MEDI_STOCK> resultData = null;
                if (valid)
                {
                    resultData = new HisMediStockGet(param).GetViewActive();
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
