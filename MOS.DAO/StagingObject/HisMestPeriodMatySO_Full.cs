using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMestPeriodMatySO : StagingObjectBase
    {
        public HisMestPeriodMatySO()
        {
            //listHisMestPeriodMatyExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMestPeriodMatyExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_MATY, bool>>> listHisMestPeriodMatyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_MATY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_PERIOD_MATY, bool>>> listVHisMestPeriodMatyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_PERIOD_MATY, bool>>>();
    }
}
