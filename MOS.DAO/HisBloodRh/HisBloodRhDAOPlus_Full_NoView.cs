using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBloodRh
{
    public partial class HisBloodRhDAO : EntityBase
    {
        public HIS_BLOOD_RH GetByCode(string code, HisBloodRhSO search)
        {
            HIS_BLOOD_RH result = null;

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

        public Dictionary<string, HIS_BLOOD_RH> GetDicByCode(HisBloodRhSO search, CommonParam param)
        {
            Dictionary<string, HIS_BLOOD_RH> result = new Dictionary<string, HIS_BLOOD_RH>();
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
