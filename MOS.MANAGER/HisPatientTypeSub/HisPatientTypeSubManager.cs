using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientTypeSub
{
    public partial class HisPatientTypeSubManager : BusinessBase
    {
        public HisPatientTypeSubManager()
            : base()
        {

        }

        public HisPatientTypeSubManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_PATIENT_TYPE_SUB> Get(HisPatientTypeSubFilterQuery filter)
        {
             List<HIS_PATIENT_TYPE_SUB> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PATIENT_TYPE_SUB> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeSubGet(param).Get(filter);
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

        
        public  HIS_PATIENT_TYPE_SUB GetById(long data)
        {
             HIS_PATIENT_TYPE_SUB result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PATIENT_TYPE_SUB resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeSubGet(param).GetById(data);
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

        
        public  HIS_PATIENT_TYPE_SUB GetById(long data, HisPatientTypeSubFilterQuery filter)
        {
             HIS_PATIENT_TYPE_SUB result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PATIENT_TYPE_SUB resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeSubGet(param).GetById(data, filter);
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

        
        public  HIS_PATIENT_TYPE_SUB GetByCode(string data)
        {
             HIS_PATIENT_TYPE_SUB result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PATIENT_TYPE_SUB resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeSubGet(param).GetByCode(data);
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

        
        public  HIS_PATIENT_TYPE_SUB GetByCode(string data, HisPatientTypeSubFilterQuery filter)
        {
             HIS_PATIENT_TYPE_SUB result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PATIENT_TYPE_SUB resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeSubGet(param).GetByCode(data, filter);
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
