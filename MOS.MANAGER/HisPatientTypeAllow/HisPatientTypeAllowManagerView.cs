using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientTypeAllow
{
    public partial class HisPatientTypeAllowManager : BusinessBase
    {
        
        public List<V_HIS_PATIENT_TYPE_ALLOW> GetView(HisPatientTypeAllowViewFilterQuery filter)
        {
            List<V_HIS_PATIENT_TYPE_ALLOW> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_PATIENT_TYPE_ALLOW> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAllowGet(param).GetView(filter);
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
