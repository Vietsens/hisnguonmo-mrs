using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisDeathWithin
{
    public partial class HisDeathWithinDAO : EntityBase
    {
        public HIS_DEATH_WITHIN GetByCode(string code, HisDeathWithinSO search)
        {
            HIS_DEATH_WITHIN result = null;

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

        public Dictionary<string, HIS_DEATH_WITHIN> GetDicByCode(HisDeathWithinSO search, CommonParam param)
        {
            Dictionary<string, HIS_DEATH_WITHIN> result = new Dictionary<string, HIS_DEATH_WITHIN>();
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
