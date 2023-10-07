using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBranchSO : StagingObjectBase
    {
        public HisBranchSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BRANCH, bool>>> listHisBranchExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BRANCH, bool>>>();
    }
}
