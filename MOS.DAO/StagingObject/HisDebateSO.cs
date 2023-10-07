using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisDebateSO : StagingObjectBase
    {
        public HisDebateSO()
        {
            listHisDebateExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_DEBATE, bool>>> listHisDebateExpression = new List<System.Linq.Expressions.Expression<Func<HIS_DEBATE, bool>>>();
    }
}
