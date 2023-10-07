using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDeathWithin
{
    public partial class HisDeathWithinDAO : EntityBase
    {
        private HisDeathWithinGet GetWorker
        {
            get
            {
                return (HisDeathWithinGet)Worker.Get<HisDeathWithinGet>();
            }
        }
        public List<HIS_DEATH_WITHIN> Get(HisDeathWithinSO search, CommonParam param)
        {
            List<HIS_DEATH_WITHIN> result = new List<HIS_DEATH_WITHIN>();
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

        public HIS_DEATH_WITHIN GetById(long id, HisDeathWithinSO search)
        {
            HIS_DEATH_WITHIN result = null;
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
