using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientCase
{
    public partial class HisPatientCaseManager : BusinessBase
    {
        public HisPatientCaseManager()
            : base()
        {

        }

        public HisPatientCaseManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_PATIENT_CASE> Get(HisPatientCaseFilterQuery filter)
        {
             List<HIS_PATIENT_CASE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PATIENT_CASE> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientCaseGet(param).Get(filter);
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

        
        public  HIS_PATIENT_CASE GetById(long data)
        {
             HIS_PATIENT_CASE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PATIENT_CASE resultData = null;
                if (valid)
                {
                    resultData = new HisPatientCaseGet(param).GetById(data);
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

        
        public  HIS_PATIENT_CASE GetById(long data, HisPatientCaseFilterQuery filter)
        {
             HIS_PATIENT_CASE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PATIENT_CASE resultData = null;
                if (valid)
                {
                    resultData = new HisPatientCaseGet(param).GetById(data, filter);
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

        
        public  HIS_PATIENT_CASE GetByCode(string data)
        {
             HIS_PATIENT_CASE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PATIENT_CASE resultData = null;
                if (valid)
                {
                    resultData = new HisPatientCaseGet(param).GetByCode(data);
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

        
        public  HIS_PATIENT_CASE GetByCode(string data, HisPatientCaseFilterQuery filter)
        {
             HIS_PATIENT_CASE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PATIENT_CASE resultData = null;
                if (valid)
                {
                    resultData = new HisPatientCaseGet(param).GetByCode(data, filter);
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
