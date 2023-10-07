using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisCacheMonitor
{
    public partial class HisCacheMonitorDAO : EntityBase
    {
        private HisCacheMonitorGet GetWorker
        {
            get
            {
                return (HisCacheMonitorGet)Worker.Get<HisCacheMonitorGet>();
            }
        }
        public List<HIS_CACHE_MONITOR> Get(HisCacheMonitorSO search, CommonParam param)
        {
            List<HIS_CACHE_MONITOR> result = new List<HIS_CACHE_MONITOR>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_CACHE_MONITOR GetById(long id, HisCacheMonitorSO search)
        {
            HIS_CACHE_MONITOR result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
