using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedLog
{
    public partial class HisBedLogManager : BusinessBase
    {
        public HisBedLogManager()
            : base()
        {

        }

        public HisBedLogManager(CommonParam param)
            : base(param)
        {

        }

        
        public List<HIS_BED_LOG> Get(HisBedLogFilterQuery filter)
        {
            List<HIS_BED_LOG> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_BED_LOG> resultData = null;
                if (valid)
                {
                    resultData = new HisBedLogGet(param).Get(filter);
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

        
        public HIS_BED_LOG GetById(long data)
        {
            HIS_BED_LOG result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_BED_LOG resultData = null;
                if (valid)
                {
                    resultData = new HisBedLogGet(param).GetById(data);
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

        
        public HIS_BED_LOG GetById(long data, HisBedLogFilterQuery filter)
        {
            HIS_BED_LOG result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_BED_LOG resultData = null;
                if (valid)
                {
                    resultData = new HisBedLogGet(param).GetById(data, filter);
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
