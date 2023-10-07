using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDeathCause
{
    public partial class HisDeathCauseDAO : EntityBase
    {
        public HIS_DEATH_CAUSE GetByCode(string code, HisDeathCauseSO search)
        {
            HIS_DEATH_CAUSE result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public Dictionary<string, HIS_DEATH_CAUSE> GetDicByCode(HisDeathCauseSO search, CommonParam param)
        {
            Dictionary<string, HIS_DEATH_CAUSE> result = new Dictionary<string, HIS_DEATH_CAUSE>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
