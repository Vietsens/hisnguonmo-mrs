using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientTypeAllow
{
    public partial class HisPatientTypeAllowManager : BusinessBase
    {
        public HisPatientTypeAllowManager()
            : base()
        {

        }

        public HisPatientTypeAllowManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_PATIENT_TYPE_ALLOW> Get(HisPatientTypeAllowFilterQuery filter)
        {
            List<HIS_PATIENT_TYPE_ALLOW> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PATIENT_TYPE_ALLOW> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAllowGet(param).Get(filter);
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

        
        public HIS_PATIENT_TYPE_ALLOW GetById(long data)
        {
            HIS_PATIENT_TYPE_ALLOW result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PATIENT_TYPE_ALLOW resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAllowGet(param).GetById(data);
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

        
        public HIS_PATIENT_TYPE_ALLOW GetById(long data, HisPatientTypeAllowFilterQuery filter)
        {
            HIS_PATIENT_TYPE_ALLOW result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PATIENT_TYPE_ALLOW resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAllowGet(param).GetById(data, filter);
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

        
        public List<long> GetPatientTypeAllowId(long data)
        {
            List<long> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<long> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAllowGet(param).GetPatientTypeAllowId(data);
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

        
        public List<HIS_PATIENT_TYPE_ALLOW> GetByPatientTypeIdOrPatientTypeAllowId(long data)
        {
             List<HIS_PATIENT_TYPE_ALLOW> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_PATIENT_TYPE_ALLOW> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAllowGet(param).GetByPatientTypeIdOrPatientTypeAllowId(data);
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

        
        public List<HIS_PATIENT_TYPE_ALLOW> GetActive()
        {
            List<HIS_PATIENT_TYPE_ALLOW> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<HIS_PATIENT_TYPE_ALLOW> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeAllowGet(param).GetActive();
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
