using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMest
{
    public partial class HisExpMestDAO : EntityBase
    {
        private HisExpMestGet GetWorker
        {
            get
            {
                return (HisExpMestGet)Worker.Get<HisExpMestGet>();
            }
        }
        public List<HIS_EXP_MEST> Get(HisExpMestSO search, CommonParam param)
        {
            List<HIS_EXP_MEST> result = new List<HIS_EXP_MEST>();
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

        public HIS_EXP_MEST GetById(long id, HisExpMestSO search)
        {
            HIS_EXP_MEST result = null;
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
