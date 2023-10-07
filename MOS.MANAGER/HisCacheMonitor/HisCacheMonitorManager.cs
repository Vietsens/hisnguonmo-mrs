using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCacheMonitor
{
    public partial class HisCacheMonitorManager : BusinessBase
    {
        public HisCacheMonitorManager()
            : base()
        {

        }

        public HisCacheMonitorManager(CommonParam param)
            : base(param)
        {

        }

        
        public  List<HIS_CACHE_MONITOR> Get(HisCacheMonitorFilterQuery filter)
        {
             List<HIS_CACHE_MONITOR> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<HIS_CACHE_MONITOR> resultData = null;
                if (valid)
                {
                    resultData = new HisCacheMonitorGet(param).Get(filter);
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

        
        public  HIS_CACHE_MONITOR GetById(long data)
        {
             HIS_CACHE_MONITOR result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                HIS_CACHE_MONITOR resultData = null;
                if (valid)
                {
                    resultData = new HisCacheMonitorGet(param).GetById(data);
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

        
        public  HIS_CACHE_MONITOR GetById(long data, HisCacheMonitorFilterQuery filter)
        {
             HIS_CACHE_MONITOR result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                HIS_CACHE_MONITOR resultData = null;
                if (valid)
                {
                    resultData = new HisCacheMonitorGet(param).GetById(data, filter);
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
