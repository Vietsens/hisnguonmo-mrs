using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDeathCause
{
    public partial class HisDeathCauseDAO : EntityBase
    {
        private HisDeathCauseGet GetWorker
        {
            get
            {
                return (HisDeathCauseGet)Worker.Get<HisDeathCauseGet>();
            }
        }
        public List<HIS_DEATH_CAUSE> Get(HisDeathCauseSO search, CommonParam param)
        {
            List<HIS_DEATH_CAUSE> result = new List<HIS_DEATH_CAUSE>();
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

        public HIS_DEATH_CAUSE GetById(long id, HisDeathCauseSO search)
        {
            HIS_DEATH_CAUSE result = null;
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
