using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestMatyReq
{
    public partial class HisExpMestMatyReqDAO : EntityBase
    {
        private HisExpMestMatyReqGet GetWorker
        {
            get
            {
                return (HisExpMestMatyReqGet)Worker.Get<HisExpMestMatyReqGet>();
            }
        }
        public List<HIS_EXP_MEST_MATY_REQ> Get(HisExpMestMatyReqSO search, CommonParam param)
        {
            List<HIS_EXP_MEST_MATY_REQ> result = new List<HIS_EXP_MEST_MATY_REQ>();
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

        public HIS_EXP_MEST_MATY_REQ GetById(long id, HisExpMestMatyReqSO search)
        {
            HIS_EXP_MEST_MATY_REQ result = null;
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
