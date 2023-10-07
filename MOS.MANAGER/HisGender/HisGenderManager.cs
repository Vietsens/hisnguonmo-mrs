using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisGender
{
    public partial class HisGenderManager : BusinessBase
    {
        public HisGenderManager()
            : base()
        {

        }

        public HisGenderManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_GENDER> Get(HisGenderFilterQuery filter)
        {
            List<HIS_GENDER> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_GENDER> resultData = null;
                if (valid)
                {
                    resultData = new HisGenderGet(param).Get(filter);
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

        
        public HIS_GENDER GetById(long data)
        {
            HIS_GENDER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_GENDER resultData = null;
                if (valid)
                {
                    resultData = new HisGenderGet(param).GetById(data);
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

        
        public HIS_GENDER GetById(long data, HisGenderFilterQuery filter)
        {
            HIS_GENDER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_GENDER resultData = null;
                if (valid)
                {
                    resultData = new HisGenderGet(param).GetById(data, filter);
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

        
        public HIS_GENDER GetByCode(string data)
        {
            HIS_GENDER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_GENDER resultData = null;
                if (valid)
                {
                    resultData = new HisGenderGet(param).GetByCode(data);
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

        
        public HIS_GENDER GetByCode(string data, HisGenderFilterQuery filter)
        {
            HIS_GENDER result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_GENDER resultData = null;
                if (valid)
                {
                    resultData = new HisGenderGet(param).GetByCode(data, filter);
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
