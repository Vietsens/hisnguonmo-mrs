using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestBltyReq
{
    public partial class HisExpMestBltyReqDAO : EntityBase
    {
        public List<V_HIS_EXP_MEST_BLTY_REQ_1> GetView1(HisExpMestBltyReqSO search, CommonParam param)
        {
            List<V_HIS_EXP_MEST_BLTY_REQ_1> result = new List<V_HIS_EXP_MEST_BLTY_REQ_1>();
            try
            {
                result = GetWorker.GetView1(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_EXP_MEST_BLTY_REQ_1 GetView1ById(long id, HisExpMestBltyReqSO search)
        {
            V_HIS_EXP_MEST_BLTY_REQ_1 result = null;

            try
            {
                result = GetWorker.GetView1ById(id, search);
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
