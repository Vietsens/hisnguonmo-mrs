using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTreatmentEndType
{
    public partial class HisTreatmentEndTypeManager : BusinessBase
    {
        public HisTreatmentEndTypeManager()
            : base()
        {

        }

        public HisTreatmentEndTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_TREATMENT_END_TYPE> Get(HisTreatmentEndTypeFilterQuery filter)
        {
             List<HIS_TREATMENT_END_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_TREATMENT_END_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentEndTypeGet(param).Get(filter);
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

        
        public  HIS_TREATMENT_END_TYPE GetById(long data)
        {
             HIS_TREATMENT_END_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TREATMENT_END_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentEndTypeGet(param).GetById(data);
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

        
        public  HIS_TREATMENT_END_TYPE GetById(long data, HisTreatmentEndTypeFilterQuery filter)
        {
             HIS_TREATMENT_END_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TREATMENT_END_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentEndTypeGet(param).GetById(data, filter);
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

        
        public  HIS_TREATMENT_END_TYPE GetByCode(string data)
        {
             HIS_TREATMENT_END_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_TREATMENT_END_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentEndTypeGet(param).GetByCode(data);
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

        
        public  HIS_TREATMENT_END_TYPE GetByCode(string data, HisTreatmentEndTypeFilterQuery filter)
        {
             HIS_TREATMENT_END_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_TREATMENT_END_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisTreatmentEndTypeGet(param).GetByCode(data, filter);
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
