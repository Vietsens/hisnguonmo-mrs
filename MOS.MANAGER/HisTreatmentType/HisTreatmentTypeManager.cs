using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatmentType
{
    public partial class HisTreatmentTypeManager : BusinessBase
    {
        public HisTreatmentTypeManager()
            : base()
        {

        }

        public HisTreatmentTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_TREATMENT_TYPE> Get(HisTreatmentTypeFilterQuery filter)
        {
             List<HIS_TREATMENT_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TREATMENT_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentTypeGet(param).Get(filter);
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

        
        public  HIS_TREATMENT_TYPE GetById(long data)
        {
             HIS_TREATMENT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TREATMENT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentTypeGet(param).GetById(data);
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

        
        public  HIS_TREATMENT_TYPE GetById(long data, HisTreatmentTypeFilterQuery filter)
        {
             HIS_TREATMENT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TREATMENT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentTypeGet(param).GetById(data, filter);
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

        
        public  HIS_TREATMENT_TYPE GetByCode(string data)
        {
             HIS_TREATMENT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TREATMENT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentTypeGet(param).GetByCode(data);
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

        
        public  HIS_TREATMENT_TYPE GetByCode(string data, HisTreatmentTypeFilterQuery filter)
        {
             HIS_TREATMENT_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TREATMENT_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentTypeGet(param).GetByCode(data, filter);
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
