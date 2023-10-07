using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisRepayReasonSO : StagingObjectBase
    {
        public HisRepayReasonSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_REPAY_REASON, bool>>> listHisRepayReasonExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REPAY_REASON, bool>>>();
    }
}
