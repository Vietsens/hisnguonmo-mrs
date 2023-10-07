using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientTypeAlter
{
    public partial class HisPatientTypeAlterManager : BusinessBase
    {
        
        public List<V_HIS_PATIENT_TYPE_ALTER> GetView(HisPatientTypeAlterViewFilterQuery filter)
        {
            List<V_HIS_PATIENT_TYPE_ALTER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_PATIENT_TYPE_ALTER> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetView(filter);
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

        
        public V_HIS_PATIENT_TYPE_ALTER GetViewById(long data)
        {
            V_HIS_PATIENT_TYPE_ALTER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_PATIENT_TYPE_ALTER resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetViewById(data);
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

        
        public V_HIS_PATIENT_TYPE_ALTER GetViewById(long data, HisPatientTypeAlterViewFilterQuery filter)
        {
            V_HIS_PATIENT_TYPE_ALTER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_PATIENT_TYPE_ALTER resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetViewById(data, filter);
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

        
        public V_HIS_PATIENT_TYPE_ALTER GetViewLastByTreatmentId(long treatmentId)
        {
            V_HIS_PATIENT_TYPE_ALTER result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(treatmentId);
                V_HIS_PATIENT_TYPE_ALTER resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetViewLastByTreatmentId(treatmentId);
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

        
        public V_HIS_PATIENT_TYPE_ALTER GetViewLastByTreatmentId(long treatmentId, long? logTime)
        {
            V_HIS_PATIENT_TYPE_ALTER result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(treatmentId);
                V_HIS_PATIENT_TYPE_ALTER resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetViewLastByTreatmentId(treatmentId, logTime);
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

        
        public V_HIS_PATIENT_TYPE_ALTER GetViewApplied(MOS.Filter.HisPatientTypeAlterViewAppliedFilter filter)
        {
            V_HIS_PATIENT_TYPE_ALTER result = null;

            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                V_HIS_PATIENT_TYPE_ALTER resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetViewApplied(filter.TreatmentId, filter.InstructionTime);
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

        
        public List<V_HIS_PATIENT_TYPE_ALTER> GetViewByTreatmentIds(List<long> filter)
        {
            List<V_HIS_PATIENT_TYPE_ALTER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_PATIENT_TYPE_ALTER> resultData = null;
                if (valid)
                {
                    resultData = new List<V_HIS_PATIENT_TYPE_ALTER>();
                    var skip = 0;
                    while (filter.Count - skip > 0)
                    {
                        var Ids = filter.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisPatientTypeAlterGet(param).GetViewByTreatmentIds(Ids));
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

        
        public V_HIS_PATIENT_TYPE_ALTER GetViewApplied(long treatmentId, long? instructionTime)
        {
            V_HIS_PATIENT_TYPE_ALTER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(treatmentId);
                valid = valid && IsNotNull(instructionTime);
                V_HIS_PATIENT_TYPE_ALTER resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetViewApplied(treatmentId, instructionTime);
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

        
        public List<V_HIS_PATIENT_TYPE_ALTER> GetViewByPatientId(long filter)
        {
            List<V_HIS_PATIENT_TYPE_ALTER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_PATIENT_TYPE_ALTER> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetViewByPatientId(filter);
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

        
        public List<V_HIS_PATIENT_TYPE_ALTER> GetViewByPatientTypeIdAndTreatmentId(long patientTypeId, long treatmentId)
        {
            List<V_HIS_PATIENT_TYPE_ALTER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(treatmentId);
                valid = valid && IsNotNull(patientTypeId);
                List<V_HIS_PATIENT_TYPE_ALTER> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAlterGet(param).GetViewByPatientTypeIdAndTreatmentId(patientTypeId, treatmentId);
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
