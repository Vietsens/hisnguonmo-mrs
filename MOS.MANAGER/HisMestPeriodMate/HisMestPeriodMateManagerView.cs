using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPeriodMate
{
    public partial class HisMestPeriodMateManager : BusinessBase
    {
        
        public List<V_HIS_MEST_PERIOD_MATE> GetView(HisMestPeriodMateViewFilterQuery filter)
        {
            List<V_HIS_MEST_PERIOD_MATE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEST_PERIOD_MATE> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMateGet(param).GetView(filter);
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

        
        public V_HIS_MEST_PERIOD_MATE GetViewById(long data)
        {
            V_HIS_MEST_PERIOD_MATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEST_PERIOD_MATE resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMateGet(param).GetViewById(data);
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

        
        public V_HIS_MEST_PERIOD_MATE GetViewById(long data, HisMestPeriodMateViewFilterQuery filter)
        {
            V_HIS_MEST_PERIOD_MATE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEST_PERIOD_MATE resultData = null;
                if (valid)
                {
                    resultData = new HisMestPeriodMateGet(param).GetViewById(data, filter);
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
