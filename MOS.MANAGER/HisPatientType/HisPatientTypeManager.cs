using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientType
{
    public partial class HisPatientTypeManager : BusinessBase
    {
        public HisPatientTypeManager()
            : base()
        {

        }

        public HisPatientTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_PATIENT_TYPE> Get(HisPatientTypeFilterQuery filter)
        {
            List<HIS_PATIENT_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_PATIENT_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeGet(param).Get(filter);
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

        
        public HIS_PATIENT_TYPE GetById(long data)
        {
            HIS_PATIENT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PATIENT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeGet(param).GetById(data);
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

        
        public HIS_PATIENT_TYPE GetById(long data, HisPatientTypeFilterQuery filter)
        {
            HIS_PATIENT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PATIENT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeGet(param).GetById(data, filter);
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

        
        public HIS_PATIENT_TYPE GetByCode(string data)
        {
            HIS_PATIENT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_PATIENT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeGet(param).GetByCode(data);
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

        
        public HIS_PATIENT_TYPE GetByCode(string data, HisPatientTypeFilterQuery filter)
        {
            HIS_PATIENT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_PATIENT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeGet(param).GetByCode(data, filter);
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

        
        public List<HIS_PATIENT_TYPE> GetByIds(List<long> data)
        {
            List<HIS_PATIENT_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_PATIENT_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new List<HIS_PATIENT_TYPE>();
                    var skip = 0;
                    while (data.Count - skip > 0)
                    {
                        var Ids = data.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        resultData.AddRange(new HisPatientTypeGet(param).GetByIds(Ids));
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

        
        public List<HIS_PATIENT_TYPE> GetActive()
        {
            List<HIS_PATIENT_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<HIS_PATIENT_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeGet(param).GetActive();
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

        
        public List<HIS_PATIENT_TYPE> GetIsCoPayment()
        {
            List<HIS_PATIENT_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                List<HIS_PATIENT_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisPatientTypeGet(param).GetIsCoPayment();
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
