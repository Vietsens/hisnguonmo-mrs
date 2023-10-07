using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEmergencyWtime
{
    public partial class HisEmergencyWtimeDAO : EntityBase
    {
        public HIS_EMERGENCY_WTIME GetByCode(string code, HisEmergencyWtimeSO search)
        {
            HIS_EMERGENCY_WTIME result = null;

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

        public Dictionary<string, HIS_EMERGENCY_WTIME> GetDicByCode(HisEmergencyWtimeSO search, CommonParam param)
        {
            Dictionary<string, HIS_EMERGENCY_WTIME> result = new Dictionary<string, HIS_EMERGENCY_WTIME>();
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
