using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatmentLogging
{
    public partial class HisTreatmentLoggingManager : BusinessBase
    {
        public HisTreatmentLoggingManager()
            : base()
        {

        }

        public HisTreatmentLoggingManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_TREATMENT_LOGGING> Get(HisTreatmentLoggingFilterQuery filter)
        {
             List<HIS_TREATMENT_LOGGING> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TREATMENT_LOGGING> resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentLoggingGet(param).Get(filter);
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

        
        public  HIS_TREATMENT_LOGGING GetById(long data)
        {
             HIS_TREATMENT_LOGGING result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TREATMENT_LOGGING resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentLoggingGet(param).GetById(data);
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

        
        public  HIS_TREATMENT_LOGGING GetById(long data, HisTreatmentLoggingFilterQuery filter)
        {
             HIS_TREATMENT_LOGGING result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TREATMENT_LOGGING resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentLoggingGet(param).GetById(data, filter);
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
