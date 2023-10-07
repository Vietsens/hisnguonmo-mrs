using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestBltyReq
{
    public partial class HisExpMestBltyReqManager : BusinessBase
    {
        
        public List<V_HIS_EXP_MEST_BLTY_REQ> GetView(HisExpMestBltyReqViewFilterQuery filter)
        {
            List<V_HIS_EXP_MEST_BLTY_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXP_MEST_BLTY_REQ> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBltyReqGet(param).GetView(filter);
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

        
        public V_HIS_EXP_MEST_BLTY_REQ GetViewById(long data)
        {
            V_HIS_EXP_MEST_BLTY_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXP_MEST_BLTY_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBltyReqGet(param).GetViewById(data);
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

        
        public V_HIS_EXP_MEST_BLTY_REQ GetViewById(long data, HisExpMestBltyReqViewFilterQuery filter)
        {
            V_HIS_EXP_MEST_BLTY_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXP_MEST_BLTY_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestBltyReqGet(param).GetViewById(data, filter);
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
