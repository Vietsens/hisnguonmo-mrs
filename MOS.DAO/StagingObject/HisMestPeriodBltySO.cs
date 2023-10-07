using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMestPeriodBltySO : StagingObjectBase
    {
        public HisMestPeriodBltySO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_BLTY, bool>>> listHisMestPeriodBltyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_BLTY, bool>>>();
    }
}
