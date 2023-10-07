using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMestPeriodMateSO : StagingObjectBase
    {
        public HisMestPeriodMateSO()
        {
            //listHisMestPeriodMateExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMestPeriodMateExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_MATE, bool>>> listHisMestPeriodMateExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_MATE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_PERIOD_MATE, bool>>> listVHisMestPeriodMateExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_PERIOD_MATE, bool>>>();
    }
}
