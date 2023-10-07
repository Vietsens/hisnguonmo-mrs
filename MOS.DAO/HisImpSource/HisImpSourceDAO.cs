using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpSource
{
    public partial class HisImpSourceDAO : EntityBase
    {
        private HisImpSourceGet GetWorker
        {
            get
            {
                return (HisImpSourceGet)Worker.Get<HisImpSourceGet>();
            }
        }
        public List<HIS_IMP_SOURCE> Get(HisImpSourceSO search, CommonParam param)
        {
            List<HIS_IMP_SOURCE> result = new List<HIS_IMP_SOURCE>();
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

        public HIS_IMP_SOURCE GetById(long id, HisImpSourceSO search)
        {
            HIS_IMP_SOURCE result = null;
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
