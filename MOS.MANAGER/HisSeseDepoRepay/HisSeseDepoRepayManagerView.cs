using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSeseDepoRepay
{
    public partial class HisSeseDepoRepayManager : BusinessBase
    {
        
        public List<V_HIS_SESE_DEPO_REPAY> GetView(HisSeseDepoRepayViewFilterQuery filter)
        {
            List<V_HIS_SESE_DEPO_REPAY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SESE_DEPO_REPAY> resultData = null;
                if (valid)
                {
                    resultData = new HisSeseDepoRepayGet(param).GetView(filter);
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

        
        public V_HIS_SESE_DEPO_REPAY GetViewById(long data)
        {
            V_HIS_SESE_DEPO_REPAY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_SESE_DEPO_REPAY resultData = null;
                if (valid)
                {
                    resultData = new HisSeseDepoRepayGet(param).GetViewById(data);
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

        
        public V_HIS_SESE_DEPO_REPAY GetViewById(long data, HisSeseDepoRepayViewFilterQuery filter)
        {
            V_HIS_SESE_DEPO_REPAY result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_SESE_DEPO_REPAY resultData = null;
                if (valid)
                {
                    resultData = new HisSeseDepoRepayGet(param).GetViewById(data, filter);
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

        
        public List<V_HIS_SESE_DEPO_REPAY> GetViewByRepayId(long repayId)
        {
            List<V_HIS_SESE_DEPO_REPAY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(repayId);
                List<V_HIS_SESE_DEPO_REPAY> resultData = null;
                if (valid)
                {
                    resultData = new HisSeseDepoRepayGet(param).GetViewByRepayId(repayId);
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
