using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCacheMonitor
{
    partial class HisCacheMonitorGet : BusinessBase
    {
        internal HisCacheMonitorGet()
            : base()
        {

        }

        internal HisCacheMonitorGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_CACHE_MONITOR> Get(HisCacheMonitorFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCacheMonitorDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CACHE_MONITOR GetById(long id)
        {
            try
            {
                return GetById(id, new HisCacheMonitorFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CACHE_MONITOR GetById(long id, HisCacheMonitorFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCacheMonitorDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
