using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExpMestReasonSO : StagingObjectBase
    {
        public HisExpMestReasonSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_REASON, bool>>> listHisExpMestReasonExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_REASON, bool>>>();
    }
}
