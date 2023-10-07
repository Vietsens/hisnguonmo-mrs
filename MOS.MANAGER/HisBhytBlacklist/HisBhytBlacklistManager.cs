using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBhytBlacklist
{
    public partial class HisBhytBlacklistManager : BusinessBase
    {
        public HisBhytBlacklistManager()
            : base()
        {

        }

        public HisBhytBlacklistManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_BHYT_BLACKLIST> Get(HisBhytBlacklistFilterQuery filter)
        {
             List<HIS_BHYT_BLACKLIST> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BHYT_BLACKLIST> resultData = null;
                if (valid)
                {
                    resultData = new HisBhytBlacklistGet(param).Get(filter);
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

        
        public  HIS_BHYT_BLACKLIST GetById(long data)
        {
             HIS_BHYT_BLACKLIST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BHYT_BLACKLIST resultData = null;
                if (valid)
                {
                    resultData = new HisBhytBlacklistGet(param).GetById(data);
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

        
        public  HIS_BHYT_BLACKLIST GetById(long data, HisBhytBlacklistFilterQuery filter)
        {
             HIS_BHYT_BLACKLIST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BHYT_BLACKLIST resultData = null;
                if (valid)
                {
                    resultData = new HisBhytBlacklistGet(param).GetById(data, filter);
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

        
        public  HIS_BHYT_BLACKLIST GetByCode(string data)
        {
             HIS_BHYT_BLACKLIST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BHYT_BLACKLIST resultData = null;
                if (valid)
                {
                    resultData = new HisBhytBlacklistGet(param).GetByCode(data);
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

        
        public  HIS_BHYT_BLACKLIST GetByCode(string data, HisBhytBlacklistFilterQuery filter)
        {
             HIS_BHYT_BLACKLIST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BHYT_BLACKLIST resultData = null;
                if (valid)
                {
                    resultData = new HisBhytBlacklistGet(param).GetByCode(data, filter);
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
