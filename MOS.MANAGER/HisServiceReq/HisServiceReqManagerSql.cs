using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServiceReq
{
    public partial class HisServiceReqManager : BusinessBase
    {
        public List<D_HIS_SERVICE_REQ_2> GetDHisServiceReq2(DHisServiceReq2Filter filter)
        {
            List<D_HIS_SERVICE_REQ_2> result = null;

            try
            {
                result = new HisServiceReqGet(param).GetDHisServiceReq2(filter);
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
