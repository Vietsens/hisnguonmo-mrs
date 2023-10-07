using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisPtttPrioritySO : StagingObjectBase
    {
        public HisPtttPrioritySO()
        {
            //listHisPtttPriorityExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_PTTT_PRIORITY, bool>>> listHisPtttPriorityExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PTTT_PRIORITY, bool>>>();
    }
}
