using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientTypeAlter
{
    public partial class HisPatientTypeAlterManager : BusinessBase
    {
        public HisPatientTypeAlterManager()
            : base()
        {

        }

        public HisPatientTypeAlterManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_PATIENT_TYPE_ALTER> Get(HisPatientTypeAlterFilterQuery filter)
        {
            List<HIS_PATIENT_TYPE_ALTER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PATIENT_TYPE_ALTER> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).Get(filter);
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

        
        public HIS_PATIENT_TYPE_ALTER GetById(long data)
        {
            HIS_PATIENT_TYPE_ALTER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PATIENT_TYPE_ALTER resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetById(data);
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

        
        public HIS_PATIENT_TYPE_ALTER GetById(long data, HisPatientTypeAlterFilterQuery filter)
        {
            HIS_PATIENT_TYPE_ALTER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PATIENT_TYPE_ALTER resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetById(data, filter);
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

        
        public HIS_PATIENT_TYPE_ALTER GetLastByTreatmentId(long treatmentId)
        {
            HIS_PATIENT_TYPE_ALTER result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                HIS_PATIENT_TYPE_ALTER resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetLastByTreatmentId(treatmentId);
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

        
        public List<HIS_PATIENT_TYPE_ALTER> GetDistinct(long treatmentId)
        {
            List<HIS_PATIENT_TYPE_ALTER> result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<HIS_PATIENT_TYPE_ALTER> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetDistinct(treatmentId);
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

        
        public List<HIS_PATIENT_TYPE_ALTER> GetByTreatmentIds(List<long> filter)
        {
            List<HIS_PATIENT_TYPE_ALTER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PATIENT_TYPE_ALTER> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_PATIENT_TYPE_ALTER>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisPatientTypeAlterGet(param).GetByTreatmentIds(Ids));
                    }
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

        
        public HIS_PATIENT_TYPE_ALTER GetApplied(long data)
        {
            HIS_PATIENT_TYPE_ALTER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PATIENT_TYPE_ALTER resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetApplied(data);
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

        
        public HIS_PATIENT_TYPE_ALTER GetApplied(long treatmentId, long? instructionTime)
        {
            HIS_PATIENT_TYPE_ALTER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(treatmentId);
                HIS_PATIENT_TYPE_ALTER resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetApplied(treatmentId, instructionTime);
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

        
        public List<HIS_PATIENT_TYPE_ALTER> GetByPatientTypeIdAndTreatmentId(long patientTypeId, long treatmentId)
        {
            List<HIS_PATIENT_TYPE_ALTER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(treatmentId);
                valid = valid && IsNotNull(patientTypeId);
                List<HIS_PATIENT_TYPE_ALTER> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetByPatientTypeIdAndTreatmentId(patientTypeId, treatmentId);
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
