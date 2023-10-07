using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBhytWhitelist
{
    public partial class HisBhytWhitelistManager : BusinessBase
    {
        public HisBhytWhitelistManager()
            : base()
        {

        }

        public HisBhytWhitelistManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_BHYT_WHITELIST> Get(HisBhytWhitelistFilterQuery filter)
        {
             List<HIS_BHYT_WHITELIST> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BHYT_WHITELIST> resultData = null;
                if (valid)
                {
                    resultData = new HisBhytWhitelistGet(param).Get(filter);
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

        
        public  HIS_BHYT_WHITELIST GetById(long data)
        {
             HIS_BHYT_WHITELIST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BHYT_WHITELIST resultData = null;
                if (valid)
                {
                    resultData = new HisBhytWhitelistGet(param).GetById(data);
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

        
        public  HIS_BHYT_WHITELIST GetById(long data, HisBhytWhitelistFilterQuery filter)
        {
             HIS_BHYT_WHITELIST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BHYT_WHITELIST resultData = null;
                if (valid)
                {
                    resultData = new HisBhytWhitelistGet(param).GetById(data, filter);
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

        
        public  HIS_BHYT_WHITELIST GetByCode(string data)
        {
             HIS_BHYT_WHITELIST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BHYT_WHITELIST resultData = null;
                if (valid)
                {
                    resultData = new HisBhytWhitelistGet(param).GetByCode(data);
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

        
        public  HIS_BHYT_WHITELIST GetByCode(string data, HisBhytWhitelistFilterQuery filter)
        {
             HIS_BHYT_WHITELIST result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BHYT_WHITELIST resultData = null;
                if (valid)
                {
                    resultData = new HisBhytWhitelistGet(param).GetByCode(data, filter);
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
