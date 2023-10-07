using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientProgram
{
    public partial class HisPatientProgramManager : BusinessBase
    {
        public HisPatientProgramManager()
            : base()
        {

        }

        public HisPatientProgramManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_PATIENT_PROGRAM> Get(HisPatientProgramFilterQuery filter)
        {
            List<HIS_PATIENT_PROGRAM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PATIENT_PROGRAM> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientProgramGet(param).Get(filter);
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

        
        public HIS_PATIENT_PROGRAM GetById(long data)
        {
            HIS_PATIENT_PROGRAM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PATIENT_PROGRAM resultData = null;
                if (valid)
                {
                    resultData = new HisPatientProgramGet(param).GetById(data);
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

        
        public HIS_PATIENT_PROGRAM GetById(long data, HisPatientProgramFilterQuery filter)
        {
            HIS_PATIENT_PROGRAM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PATIENT_PROGRAM resultData = null;
                if (valid)
                {
                    resultData = new HisPatientProgramGet(param).GetById(data, filter);
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

        
        public HIS_PATIENT_PROGRAM GetByCode(string data)
        {
            HIS_PATIENT_PROGRAM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PATIENT_PROGRAM resultData = null;
                if (valid)
                {
                    resultData = new HisPatientProgramGet(param).GetByCode(data);
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

        
        public HIS_PATIENT_PROGRAM GetByCode(string data, HisPatientProgramFilterQuery filter)
        {
            HIS_PATIENT_PROGRAM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PATIENT_PROGRAM resultData = null;
                if (valid)
                {
                    resultData = new HisPatientProgramGet(param).GetByCode(data, filter);
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

        
        public List<HIS_PATIENT_PROGRAM> GetByProgramId(long data)
        {
            List<HIS_PATIENT_PROGRAM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_PATIENT_PROGRAM> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientProgramGet(param).GetByProgramId(data);
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

        
        public List<HIS_PATIENT_PROGRAM> GetByPatientId(long data)
        {
            List<HIS_PATIENT_PROGRAM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_PATIENT_PROGRAM> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientProgramGet(param).GetByPatientId(data);
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

        
        public List<HIS_PATIENT_PROGRAM> GetByPatientIdAndProgramId(long patientId, long programId)
        {
            List<HIS_PATIENT_PROGRAM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(patientId);
                valid = valid && IsNotNull(programId);
                List<HIS_PATIENT_PROGRAM> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientProgramGet(param).GetByPatientIdAndProgramId(patientId, programId);
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
