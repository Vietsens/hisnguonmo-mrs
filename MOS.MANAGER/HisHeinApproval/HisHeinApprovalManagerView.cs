using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisHeinApproval
{
    public partial class HisHeinApprovalManager : BusinessBase
    {
        
        public List<V_HIS_HEIN_APPROVAL> GetView(HisHeinApprovalViewFilterQuery filter)
        {
            List<V_HIS_HEIN_APPROVAL> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_HEIN_APPROVAL> resultData = null;
                if (valid)
                {
                    resultData = new HisHeinApprovalGet(param).GetView(filter);
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

        
        public V_HIS_HEIN_APPROVAL GetViewById(long data)
        {
            V_HIS_HEIN_APPROVAL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_HEIN_APPROVAL resultData = null;
                if (valid)
                {
                    resultData = new HisHeinApprovalGet(param).GetViewById(data);
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

        
        public V_HIS_HEIN_APPROVAL GetViewById(long data, HisHeinApprovalViewFilterQuery filter)
        {
            V_HIS_HEIN_APPROVAL result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_HEIN_APPROVAL resultData = null;
                if (valid)
                {
                    resultData = new HisHeinApprovalGet(param).GetViewById(data, filter);
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
