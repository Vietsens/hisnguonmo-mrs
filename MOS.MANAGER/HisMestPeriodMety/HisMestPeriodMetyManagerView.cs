using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodMety
{
    public partial class HisMestPeriodMetyManager : BusinessBase
    {
        
        public List<V_HIS_MEST_PERIOD_METY> GetView(HisMestPeriodMetyViewFilterQuery filter)
        {
            List<V_HIS_MEST_PERIOD_METY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEST_PERIOD_METY> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMetyGet(param).GetView(filter);
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

        
        public V_HIS_MEST_PERIOD_METY GetViewById(long data)
        {
            V_HIS_MEST_PERIOD_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEST_PERIOD_METY resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMetyGet(param).GetViewById(data);
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

        
        public V_HIS_MEST_PERIOD_METY GetViewById(long data, HisMestPeriodMetyViewFilterQuery filter)
        {
            V_HIS_MEST_PERIOD_METY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEST_PERIOD_METY resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMetyGet(param).GetViewById(data, filter);
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
