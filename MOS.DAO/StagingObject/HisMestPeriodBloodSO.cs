using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMestPeriodBloodSO : StagingObjectBase
    {
        public HisMestPeriodBloodSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_BLOOD, bool>>> listHisMestPeriodBloodExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_BLOOD, bool>>>();
    }
}
