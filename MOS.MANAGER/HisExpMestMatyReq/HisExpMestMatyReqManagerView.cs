using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMatyReq
{
    public partial class HisExpMestMatyReqManager : BusinessBase
    {
        
        public List<V_HIS_EXP_MEST_MATY_REQ> GetView(HisExpMestMatyReqViewFilterQuery filter)
        {
            List<V_HIS_EXP_MEST_MATY_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXP_MEST_MATY_REQ> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMatyReqGet(param).GetView(filter);
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

        
        public V_HIS_EXP_MEST_MATY_REQ GetViewById(long data)
        {
            V_HIS_EXP_MEST_MATY_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXP_MEST_MATY_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMatyReqGet(param).GetViewById(data);
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

        
        public V_HIS_EXP_MEST_MATY_REQ GetViewById(long data, HisExpMestMatyReqViewFilterQuery filter)
        {
            V_HIS_EXP_MEST_MATY_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXP_MEST_MATY_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMatyReqGet(param).GetViewById(data, filter);
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
