using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisMestPeriodMetySO : StagingObjectBase
    {
        public HisMestPeriodMetySO()
        {
            //listHisMestPeriodMetyExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisMestPeriodMetyExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_METY, bool>>> listHisMestPeriodMetyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEST_PERIOD_METY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_PERIOD_METY, bool>>> listVHisMestPeriodMetyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_MEST_PERIOD_METY, bool>>>();
    }
}
