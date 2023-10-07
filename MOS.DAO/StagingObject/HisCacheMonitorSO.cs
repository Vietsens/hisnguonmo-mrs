using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisCacheMonitorSO : StagingObjectBase
    {
        public HisCacheMonitorSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_CACHE_MONITOR, bool>>> listHisCacheMonitorExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CACHE_MONITOR, bool>>>();
    }
}
