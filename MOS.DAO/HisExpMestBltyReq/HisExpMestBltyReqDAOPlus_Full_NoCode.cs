using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestBltyReq
{
    public partial class HisExpMestBltyReqDAO : EntityBase
    {
        public List<V_HIS_EXP_MEST_BLTY_REQ> GetView(HisExpMestBltyReqSO search, CommonParam param)
        {
            List<V_HIS_EXP_MEST_BLTY_REQ> result = new List<V_HIS_EXP_MEST_BLTY_REQ>();
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

        public V_HIS_EXP_MEST_BLTY_REQ GetViewById(long id, HisExpMestBltyReqSO search)
        {
            V_HIS_EXP_MEST_BLTY_REQ result = null;

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
