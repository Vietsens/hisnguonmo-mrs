using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisIcdCm
{
    public partial class HisIcdCmManager : BusinessBase
    {
        public HisIcdCmManager()
            : base()
        {

        }

        public HisIcdCmManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_ICD_CM> Get(HisIcdCmFilterQuery filter)
        {
            List<HIS_ICD_CM> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_ICD_CM> resultData = null;
                if (valid)
                {
                    resultData = new HisIcdCmGet(param).Get(filter);
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

        
        public HIS_ICD_CM GetById(long data)
        {
            HIS_ICD_CM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ICD_CM resultData = null;
                if (valid)
                {
                    resultData = new HisIcdCmGet(param).GetById(data);
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

        
        public HIS_ICD_CM GetById(long data, HisIcdCmFilterQuery filter)
        {
            HIS_ICD_CM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ICD_CM resultData = null;
                if (valid)
                {
                    resultData = new HisIcdCmGet(param).GetById(data, filter);
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

        
        public HIS_ICD_CM GetByCode(string data)
        {
            HIS_ICD_CM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_ICD_CM resultData = null;
                if (valid)
                {
                    resultData = new HisIcdCmGet(param).GetByCode(data);
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

        
        public HIS_ICD_CM GetByCode(string data, HisIcdCmFilterQuery filter)
        {
            HIS_ICD_CM result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_ICD_CM resultData = null;
                if (valid)
                {
                    resultData = new HisIcdCmGet(param).GetByCode(data, filter);
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
