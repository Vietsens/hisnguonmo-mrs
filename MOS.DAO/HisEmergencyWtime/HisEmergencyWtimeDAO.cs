using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEmergencyWtime
{
    public partial class HisEmergencyWtimeDAO : EntityBase
    {
        private HisEmergencyWtimeGet GetWorker
        {
            get
            {
                return (HisEmergencyWtimeGet)Worker.Get<HisEmergencyWtimeGet>();
            }
        }
        public List<HIS_EMERGENCY_WTIME> Get(HisEmergencyWtimeSO search, CommonParam param)
        {
            List<HIS_EMERGENCY_WTIME> result = new List<HIS_EMERGENCY_WTIME>();
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

        public HIS_EMERGENCY_WTIME GetById(long id, HisEmergencyWtimeSO search)
        {
            HIS_EMERGENCY_WTIME result = null;
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
