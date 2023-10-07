using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMetyReq
{
    public partial class HisExpMestMetyReqManager : BusinessBase
    {
        
        public List<V_HIS_EXP_MEST_METY_REQ_1> GetView1(HisExpMestMetyReqView1FilterQuery filter)
        {
            List<V_HIS_EXP_MEST_METY_REQ_1> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_EXP_MEST_METY_REQ_1> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMetyReqGet(param).GetView1(filter);
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

        
        public List<V_HIS_EXP_MEST_METY_REQ_1> GetView1ByExpMestId(long data)
        {
            List<V_HIS_EXP_MEST_METY_REQ_1> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_METY_REQ_1> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMetyReqGet(param).GetView1ByExpMestId(data);
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

        
        public List<V_HIS_EXP_MEST_METY_REQ_1> GetView1ByExpMestIds(List<long> data)
        {
            List<V_HIS_EXP_MEST_METY_REQ_1> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<V_HIS_EXP_MEST_METY_REQ_1> resultData = null;
                if (valid)
                {
                    resultData = new HisExpMestMetyReqGet(param).GetView1ByExpMestIds(data);
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
