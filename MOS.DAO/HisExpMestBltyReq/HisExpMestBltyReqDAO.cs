using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestBltyReq
{
    public partial class HisExpMestBltyReqDAO : EntityBase
    {
        private HisExpMestBltyReqGet GetWorker
        {
            get
            {
                return (HisExpMestBltyReqGet)Worker.Get<HisExpMestBltyReqGet>();
            }
        }
        public List<HIS_EXP_MEST_BLTY_REQ> Get(HisExpMestBltyReqSO search, CommonParam param)
        {
            List<HIS_EXP_MEST_BLTY_REQ> result = new List<HIS_EXP_MEST_BLTY_REQ>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_EXP_MEST_BLTY_REQ GetById(long id, HisExpMestBltyReqSO search)
        {
            HIS_EXP_MEST_BLTY_REQ result = null;
            try
            {
                result = GetWorker.GetById(id, search);
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
