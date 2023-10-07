using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMestReason
{
    public partial class HisExpMestReasonDAO : EntityBase
    {
        private HisExpMestReasonGet GetWorker
        {
            get
            {
                return (HisExpMestReasonGet)Worker.Get<HisExpMestReasonGet>();
            }
        }
        public List<HIS_EXP_MEST_REASON> Get(HisExpMestReasonSO search, CommonParam param)
        {
            List<HIS_EXP_MEST_REASON> result = new List<HIS_EXP_MEST_REASON>();
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

        public HIS_EXP_MEST_REASON GetById(long id, HisExpMestReasonSO search)
        {
            HIS_EXP_MEST_REASON result = null;
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
