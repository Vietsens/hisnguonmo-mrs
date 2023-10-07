using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBillFund
{
    public partial class HisBillFundManager : BusinessBase
    {
        
        public List<V_HIS_BILL_FUND> GetView(HisBillFundViewFilterQuery filter)
        {
            List<V_HIS_BILL_FUND> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_BILL_FUND> resultData = null;
                if (valid)
                {
                    resultData = new HisBillFundGet(param).GetView(filter);
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

        
        public V_HIS_BILL_FUND GetViewById(long data)
        {
            V_HIS_BILL_FUND result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_BILL_FUND resultData = null;
                if (valid)
                {
                    resultData = new HisBillFundGet(param).GetViewById(data);
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

        
        public V_HIS_BILL_FUND GetViewById(long data, HisBillFundViewFilterQuery filter)
        {
            V_HIS_BILL_FUND result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_BILL_FUND resultData = null;
                if (valid)
                {
                    resultData = new HisBillFundGet(param).GetViewById(data, filter);
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
