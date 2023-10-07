using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDepositReq
{
    public partial class HisDepositReqManager : BusinessBase
    {
        
        public List<V_HIS_DEPOSIT_REQ> GetView(HisDepositReqViewFilterQuery filter)
        {
            List<V_HIS_DEPOSIT_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_DEPOSIT_REQ> resultData = null;
                if (valid)
                {
                    resultData = new HisDepositReqGet(param).GetView(filter);
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

        
        public V_HIS_DEPOSIT_REQ GetViewByCode(string data)
        {
            V_HIS_DEPOSIT_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_DEPOSIT_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisDepositReqGet(param).GetViewByCode(data);
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

        
        public V_HIS_DEPOSIT_REQ GetViewByCode(string data, HisDepositReqViewFilterQuery filter)
        {
            V_HIS_DEPOSIT_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_DEPOSIT_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisDepositReqGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_DEPOSIT_REQ GetViewById(long data)
        {
            V_HIS_DEPOSIT_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_DEPOSIT_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisDepositReqGet(param).GetViewById(data);
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

        
        public V_HIS_DEPOSIT_REQ GetViewById(long data, HisDepositReqViewFilterQuery filter)
        {
            V_HIS_DEPOSIT_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_DEPOSIT_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisDepositReqGet(param).GetViewById(data, filter);
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
