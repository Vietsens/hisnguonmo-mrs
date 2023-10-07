using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmotionlessMethod
{
    public partial class HisEmotionlessMethodManager : BusinessBase
    {
        public HisEmotionlessMethodManager()
            : base()
        {

        }

        public HisEmotionlessMethodManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_EMOTIONLESS_METHOD> Get(HisEmotionlessMethodFilterQuery filter)
        {
             List<HIS_EMOTIONLESS_METHOD> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EMOTIONLESS_METHOD> resultData = null;
                if (valid)
                {
                    resultData = new HisEmotionlessMethodGet(param).Get(filter);
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

        
        public  HIS_EMOTIONLESS_METHOD GetById(long data)
        {
             HIS_EMOTIONLESS_METHOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EMOTIONLESS_METHOD resultData = null;
                if (valid)
                {
                    resultData = new HisEmotionlessMethodGet(param).GetById(data);
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

        
        public  HIS_EMOTIONLESS_METHOD GetById(long data, HisEmotionlessMethodFilterQuery filter)
        {
             HIS_EMOTIONLESS_METHOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EMOTIONLESS_METHOD resultData = null;
                if (valid)
                {
                    resultData = new HisEmotionlessMethodGet(param).GetById(data, filter);
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

        
        public  HIS_EMOTIONLESS_METHOD GetByCode(string data)
        {
             HIS_EMOTIONLESS_METHOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EMOTIONLESS_METHOD resultData = null;
                if (valid)
                {
                    resultData = new HisEmotionlessMethodGet(param).GetByCode(data);
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

        
        public  HIS_EMOTIONLESS_METHOD GetByCode(string data, HisEmotionlessMethodFilterQuery filter)
        {
             HIS_EMOTIONLESS_METHOD result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EMOTIONLESS_METHOD resultData = null;
                if (valid)
                {
                    resultData = new HisEmotionlessMethodGet(param).GetByCode(data, filter);
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
