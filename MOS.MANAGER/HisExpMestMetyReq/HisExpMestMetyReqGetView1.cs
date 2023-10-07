using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMetyReq
{
    partial class HisExpMestMetyReqGet : BusinessBase
    {
        internal List<V_HIS_EXP_MEST_METY_REQ_1> GetView1(HisExpMestMetyReqView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMetyReqDAO.GetView1(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXP_MEST_METY_REQ_1> GetView1ByExpMestIds(List<long> expMestIds)
        {
            if (IsNotNullOrEmpty(expMestIds))
            {
                HisExpMestMetyReqView1FilterQuery filter = new HisExpMestMetyReqView1FilterQuery();
                filter.EXP_MEST_IDs = expMestIds;
                return this.GetView1(filter);
            }
            return null;
        }

        internal List<V_HIS_EXP_MEST_METY_REQ_1> GetView1ByExpMestId(long expMestId)
        {
            HisExpMestMetyReqView1FilterQuery filter = new HisExpMestMetyReqView1FilterQuery();
            filter.EXP_MEST_ID = expMestId;
            return this.GetView1(filter);
        }
    }
}
