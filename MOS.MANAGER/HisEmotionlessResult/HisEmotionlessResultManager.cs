using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmotionlessResult
{
    public partial class HisEmotionlessResultManager : BusinessBase
    {
        public HisEmotionlessResultManager()
            : base()
        {

        }

        public HisEmotionlessResultManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_EMOTIONLESS_RESULT> Get(HisEmotionlessResultFilterQuery filter)
        {
             List<HIS_EMOTIONLESS_RESULT> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_EMOTIONLESS_RESULT> resultData = null;
                if (valid)
                {
                    resultData = new HisEmotionlessResultGet(param).Get(filter);
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

        
        public  HIS_EMOTIONLESS_RESULT GetById(long data)
        {
             HIS_EMOTIONLESS_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EMOTIONLESS_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisEmotionlessResultGet(param).GetById(data);
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

        
        public  HIS_EMOTIONLESS_RESULT GetById(long data, HisEmotionlessResultFilterQuery filter)
        {
             HIS_EMOTIONLESS_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EMOTIONLESS_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisEmotionlessResultGet(param).GetById(data, filter);
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

        
        public  HIS_EMOTIONLESS_RESULT GetByCode(string data)
        {
             HIS_EMOTIONLESS_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_EMOTIONLESS_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisEmotionlessResultGet(param).GetByCode(data);
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

        
        public  HIS_EMOTIONLESS_RESULT GetByCode(string data, HisEmotionlessResultFilterQuery filter)
        {
             HIS_EMOTIONLESS_RESULT result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_EMOTIONLESS_RESULT resultData = null;
                if (valid)
                {
                    resultData = new HisEmotionlessResultGet(param).GetByCode(data, filter);
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
