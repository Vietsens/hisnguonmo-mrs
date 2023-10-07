using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMetyReq
{
    public partial class HisExpMestMetyReqManager : BusinessBase
    {
        
        public List<V_HIS_EXP_MEST_METY_REQ> GetView(HisExpMestMetyReqViewFilterQuery filter)
        {
            List<V_HIS_EXP_MEST_METY_REQ> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXP_MEST_METY_REQ> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMetyReqGet(param).GetView(filter);
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

        
        public V_HIS_EXP_MEST_METY_REQ GetViewById(long data)
        {
            V_HIS_EXP_MEST_METY_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_EXP_MEST_METY_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMetyReqGet(param).GetViewById(data);
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

        
        public V_HIS_EXP_MEST_METY_REQ GetViewById(long data, HisExpMestMetyReqViewFilterQuery filter)
        {
            V_HIS_EXP_MEST_METY_REQ result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_EXP_MEST_METY_REQ resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMetyReqGet(param).GetViewById(data, filter);
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
