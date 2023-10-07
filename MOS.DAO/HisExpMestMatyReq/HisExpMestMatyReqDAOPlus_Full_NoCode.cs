using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestMatyReq
{
    public partial class HisExpMestMatyReqDAO : EntityBase
    {
        public List<V_HIS_EXP_MEST_MATY_REQ> GetView(HisExpMestMatyReqSO search, CommonParam param)
        {
            List<V_HIS_EXP_MEST_MATY_REQ> result = new List<V_HIS_EXP_MEST_MATY_REQ>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_EXP_MEST_MATY_REQ GetViewById(long id, HisExpMestMatyReqSO search)
        {
            V_HIS_EXP_MEST_MATY_REQ result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
