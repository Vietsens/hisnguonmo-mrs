using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatmentLogging
{
    public partial class HisTreatmentLoggingManager : BusinessBase
    {
        
        public List<V_HIS_TREATMENT_LOGGING> GetView(HisTreatmentLoggingViewFilterQuery filter)
        {
            List<V_HIS_TREATMENT_LOGGING> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_TREATMENT_LOGGING> resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentLoggingGet(param).GetView(filter);
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

        
        public V_HIS_TREATMENT_LOGGING GetViewById(long data)
        {
            V_HIS_TREATMENT_LOGGING result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_TREATMENT_LOGGING resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentLoggingGet(param).GetViewById(data);
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

        
        public V_HIS_TREATMENT_LOGGING GetViewById(long data, HisTreatmentLoggingViewFilterQuery filter)
        {
            V_HIS_TREATMENT_LOGGING result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_TREATMENT_LOGGING resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentLoggingGet(param).GetViewById(data, filter);
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
