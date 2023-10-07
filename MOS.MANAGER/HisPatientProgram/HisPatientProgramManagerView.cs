using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientProgram
{
    public partial class HisPatientProgramManager : BusinessBase
    {
        
        public List<V_HIS_PATIENT_PROGRAM> GetView(HisPatientProgramViewFilterQuery filter)
        {
            List<V_HIS_PATIENT_PROGRAM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_PATIENT_PROGRAM> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientProgramGet(param).GetView(filter);
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

        
        public V_HIS_PATIENT_PROGRAM GetViewByCode(string data)
        {
            V_HIS_PATIENT_PROGRAM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_PATIENT_PROGRAM resultData = null;
                if (valid)
                {
                    resultData = new HisPatientProgramGet(param).GetViewByCode(data);
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

        
        public V_HIS_PATIENT_PROGRAM GetViewByCode(string data, HisPatientProgramViewFilterQuery filter)
        {
            V_HIS_PATIENT_PROGRAM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_PATIENT_PROGRAM resultData = null;
                if (valid)
                {
                    resultData = new HisPatientProgramGet(param).GetViewByCode(data, filter);
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

        
        public V_HIS_PATIENT_PROGRAM GetViewById(long data)
        {
            V_HIS_PATIENT_PROGRAM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_PATIENT_PROGRAM resultData = null;
                if (valid)
                {
                    resultData = new HisPatientProgramGet(param).GetViewById(data);
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

        
        public V_HIS_PATIENT_PROGRAM GetViewById(long data, HisPatientProgramViewFilterQuery filter)
        {
            V_HIS_PATIENT_PROGRAM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_PATIENT_PROGRAM resultData = null;
                if (valid)
                {
                    resultData = new HisPatientProgramGet(param).GetViewById(data, filter);
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
