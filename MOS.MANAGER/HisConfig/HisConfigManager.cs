using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisConfig
{
    public partial class HisConfigManager : BusinessBase
    {
        public HisConfigManager()
            : base()
        {

        }

        public HisConfigManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_CONFIG> Get(HisConfigFilterQuery filter)
        {
            List<HIS_CONFIG> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_CONFIG> resultData = null;
                if (valid)
                {
                    resultData = new HisConfigGet(param).Get(filter);
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

        
        public HIS_CONFIG GetById(long data)
        {
            HIS_CONFIG result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_CONFIG resultData = null;
                if (valid)
                {
                    resultData = new HisConfigGet(param).GetById(data);
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

        
        public HIS_CONFIG GetById(long data, HisConfigFilterQuery filter)
        {
            HIS_CONFIG result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_CONFIG resultData = null;
                if (valid)
                {
                    resultData = new HisConfigGet(param).GetById(data, filter);
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
