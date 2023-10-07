using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodMedi
{
    public partial class HisMestPeriodMediManager : BusinessBase
    {
        
        public List<V_HIS_MEST_PERIOD_MEDI> GetView(HisMestPeriodMediViewFilterQuery filter)
        {
            List<V_HIS_MEST_PERIOD_MEDI> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEST_PERIOD_MEDI> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMediGet(param).GetView(filter);
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

        
        public V_HIS_MEST_PERIOD_MEDI GetViewById(long data)
        {
            V_HIS_MEST_PERIOD_MEDI result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEST_PERIOD_MEDI resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMediGet(param).GetViewById(data);
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

        
        public V_HIS_MEST_PERIOD_MEDI GetViewById(long data, HisMestPeriodMediViewFilterQuery filter)
        {
            V_HIS_MEST_PERIOD_MEDI result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEST_PERIOD_MEDI resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMediGet(param).GetViewById(data, filter);
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
