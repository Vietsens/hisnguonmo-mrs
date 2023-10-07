using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMestPeriodMediSO : StagingObjectBase
    {
        public HisMestPeriodMediSO()
        {
            //listHisMestPeriodMediExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMestPeriodMediExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_MEDI, bool>>> listHisMestPeriodMediExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_MEDI, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_PERIOD_MEDI, bool>>> listVHisMestPeriodMediExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_PERIOD_MEDI, bool>>>();
    }
}
