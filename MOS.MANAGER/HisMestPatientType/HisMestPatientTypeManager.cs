using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPatientType
{
    public partial class HisMestPatientTypeManager : BusinessBase
    {
        public HisMestPatientTypeManager()
            : base()
        {

        }

        public HisMestPatientTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_MEST_PATIENT_TYPE> Get(HisMestPatientTypeFilterQuery filter)
        {
             List<HIS_MEST_PATIENT_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_MEST_PATIENT_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPatientTypeGet(param).Get(filter);
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

        
        public  HIS_MEST_PATIENT_TYPE GetById(long data)
        {
             HIS_MEST_PATIENT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_MEST_PATIENT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMestPatientTypeGet(param).GetById(data);
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

        
        public  HIS_MEST_PATIENT_TYPE GetById(long data, HisMestPatientTypeFilterQuery filter)
        {
             HIS_MEST_PATIENT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_MEST_PATIENT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisMestPatientTypeGet(param).GetById(data, filter);
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

        
        public  List<HIS_MEST_PATIENT_TYPE> GetByMediStockId(long data)
        {
             List<HIS_MEST_PATIENT_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEST_PATIENT_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPatientTypeGet(param).GetByMediStockId(data);
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

        
        public  List<HIS_MEST_PATIENT_TYPE> GetByPatientTypeId(long data)
        {
             List<HIS_MEST_PATIENT_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                List<HIS_MEST_PATIENT_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisMestPatientTypeGet(param).GetByPatientTypeId(data);
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
