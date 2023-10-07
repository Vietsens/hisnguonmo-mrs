using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisDeathWithinSO : StagingObjectBase
    {
        public HisDeathWithinSO()
        {
            //listHisDeathWithinExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_DEATH_WITHIN, bool>>> listHisDeathWithinExpression = new List<System.Linq.Expressions.Expression<Func<HIS_DEATH_WITHIN, bool>>>();
    }
}
