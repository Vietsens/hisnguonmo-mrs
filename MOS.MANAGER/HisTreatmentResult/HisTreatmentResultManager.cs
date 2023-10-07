using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatmentResult
{
    public partial class HisTreatmentResultManager : BusinessBase
    {
        public HisTreatmentResultManager()
            : base()
        {

        }

        public HisTreatmentResultManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_TREATMENT_RESULT> Get(HisTreatmentResultFilterQuery filter)
        {
             List<HIS_TREATMENT_RESULT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TREATMENT_RESULT> resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentResultGet(param).Get(filter);
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

        
        public  HIS_TREATMENT_RESULT GetById(long data)
        {
             HIS_TREATMENT_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TREATMENT_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentResultGet(param).GetById(data);
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

        
        public  HIS_TREATMENT_RESULT GetById(long data, HisTreatmentResultFilterQuery filter)
        {
             HIS_TREATMENT_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TREATMENT_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentResultGet(param).GetById(data, filter);
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

        
        public  HIS_TREATMENT_RESULT GetByCode(string data)
        {
             HIS_TREATMENT_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TREATMENT_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentResultGet(param).GetByCode(data);
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

        
        public  HIS_TREATMENT_RESULT GetByCode(string data, HisTreatmentResultFilterQuery filter)
        {
             HIS_TREATMENT_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TREATMENT_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentResultGet(param).GetByCode(data, filter);
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
