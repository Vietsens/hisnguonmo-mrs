using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMatyReq
{
    partial class HisExpMestMatyReqGet : BusinessBase
    {
        internal List<V_HIS_EXP_MEST_MATY_REQ> GetView(HisExpMestMatyReqViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMatyReqDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MATY_REQ GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisExpMestMatyReqViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXP_MEST_MATY_REQ GetViewById(long id, HisExpMestMatyReqViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExpMestMatyReqDAO.GetViewById(id, filter.Query());
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
