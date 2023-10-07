using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceReq
{
    public partial class HisServiceReqManager : BusinessBase
    {
        public List<V_HIS_SERVICE_REQ_7> GetView7(HisServiceReqView7FilterQuery filter)
        {
            List<V_HIS_SERVICE_REQ_7> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_SERVICE_REQ_7> resultData = null;
                if (valid)
                {
                    resultData = new HisServiceReqGet(param).GetView7(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }

            return result;
        }
    }
}
