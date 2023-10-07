using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPatientType
{
    public partial class HisMestPatientTypeManager : BusinessBase
    {
        
        public List<V_HIS_MEST_PATIENT_TYPE> GetView(HisMestPatientTypeViewFilterQuery filter)
        {
            List<V_HIS_MEST_PATIENT_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_MEST_PATIENT_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPatientTypeGet(param).GetView(filter);
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

        
        public V_HIS_MEST_PATIENT_TYPE GetViewById(long data)
        {
            V_HIS_MEST_PATIENT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_MEST_PATIENT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMestPatientTypeGet(param).GetViewById(data);
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

        
        public V_HIS_MEST_PATIENT_TYPE GetViewById(long data, HisMestPatientTypeViewFilterQuery filter)
        {
            V_HIS_MEST_PATIENT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_MEST_PATIENT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMestPatientTypeGet(param).GetViewById(data, filter);
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
