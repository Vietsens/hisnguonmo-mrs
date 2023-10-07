using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMest
{
    public partial class HisImpMestDAO : EntityBase
    {
        private HisImpMestGet GetWorker
        {
            get
            {
                return (HisImpMestGet)Worker.Get<HisImpMestGet>();
            }
        }
        public List<HIS_IMP_MEST> Get(HisImpMestSO search, CommonParam param)
        {
            List<HIS_IMP_MEST> result = new List<HIS_IMP_MEST>();
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

        public HIS_IMP_MEST GetById(long id, HisImpMestSO search)
        {
            HIS_IMP_MEST result = null;
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
