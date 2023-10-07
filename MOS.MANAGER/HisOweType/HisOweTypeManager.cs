using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisOweType
{
    public partial class HisOweTypeManager : BusinessBase
    {
        public HisOweTypeManager()
            : base()
        {

        }

        public HisOweTypeManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_OWE_TYPE> Get(HisOweTypeFilterQuery filter)
        {
            List<HIS_OWE_TYPE> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_OWE_TYPE> resultData = null;
                if (valid)
                {
                    resultData = new HisOweTypeGet(param).Get(filter);
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

        
        public HIS_OWE_TYPE GetById(long data)
        {
            HIS_OWE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_OWE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisOweTypeGet(param).GetById(data);
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

        
        public HIS_OWE_TYPE GetById(long data, HisOweTypeFilterQuery filter)
        {
            HIS_OWE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_OWE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisOweTypeGet(param).GetById(data, filter);
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

        
        public HIS_OWE_TYPE GetByCode(string data)
        {
            HIS_OWE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_OWE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisOweTypeGet(param).GetByCode(data);
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

        
        public HIS_OWE_TYPE GetByCode(string data, HisOweTypeFilterQuery filter)
        {
            HIS_OWE_TYPE result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_OWE_TYPE resultData = null;
                if (valid)
                {
                    resultData = new HisOweTypeGet(param).GetByCode(data, filter);
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
