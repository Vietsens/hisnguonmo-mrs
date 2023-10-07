using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestMetyReq
{
    public partial class HisExpMestMetyReqDAO : EntityBase
    {
        private HisExpMestMetyReqGet GetWorker
        {
            get
            {
                return (HisExpMestMetyReqGet)Worker.Get<HisExpMestMetyReqGet>();
            }
        }
        public List<HIS_EXP_MEST_METY_REQ> Get(HisExpMestMetyReqSO search, CommonParam param)
        {
            List<HIS_EXP_MEST_METY_REQ> result = new List<HIS_EXP_MEST_METY_REQ>();
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

        public HIS_EXP_MEST_METY_REQ GetById(long id, HisExpMestMetyReqSO search)
        {
            HIS_EXP_MEST_METY_REQ result = null;
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
