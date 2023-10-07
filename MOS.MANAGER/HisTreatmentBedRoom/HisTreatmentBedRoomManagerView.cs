using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatmentBedRoom
{
    public partial class HisTreatmentBedRoomManager : BusinessBase
    {
        
        public List<V_HIS_TREATMENT_BED_ROOM> GetView(HisTreatmentBedRoomViewFilterQuery filter)
        {
            List<V_HIS_TREATMENT_BED_ROOM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_TREATMENT_BED_ROOM> resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentBedRoomGet(param).GetView(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        
        public V_HIS_TREATMENT_BED_ROOM GetViewById(long data)
        {
            V_HIS_TREATMENT_BED_ROOM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_TREATMENT_BED_ROOM resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentBedRoomGet(param).GetViewById(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
