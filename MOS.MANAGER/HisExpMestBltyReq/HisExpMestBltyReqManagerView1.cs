using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestBltyReq
{
    public partial class HisExpMestBltyReqManager : BusinessBase
    {
        
        public List<V_HIS_EXP_MEST_BLTY_REQ_1> GetView1(HisExpMestBltyReqView1FilterQuery filter)
        {
            List<V_HIS_EXP_MEST_BLTY_REQ_1> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXP_MEST_BLTY_REQ_1> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBltyReqGet(param).GetView1(filter);
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
