using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestBltyReq
{
    partial class HisExpMestBltyReqGet : BusinessBase
    {
        internal List<V_HIS_EXP_MEST_BLTY_REQ_1> GetView1(HisExpMestBltyReqView1FilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestBltyReqDAO.GetView1(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
