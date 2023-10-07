using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRegisterReq
{
    public partial class HisRegisterReqManager : BusinessBase
    {
        
        public List<V_HIS_REGISTER_REQ> GetView(HisRegisterReqViewFilterQuery filter)
        {
            List<V_HIS_REGISTER_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_REGISTER_REQ> resultData = null;
                if (valid)
                {
                    resultData = new HisRegisterReqGet(param).GetView(filter);
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

        
        public V_HIS_REGISTER_REQ GetViewById(long data)
        {
            V_HIS_REGISTER_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_REGISTER_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisRegisterReqGet(param).GetViewById(data);
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

        
        public V_HIS_REGISTER_REQ GetViewById(long data, HisRegisterReqViewFilterQuery filter)
        {
            V_HIS_REGISTER_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_REGISTER_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisRegisterReqGet(param).GetViewById(data, filter);
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
