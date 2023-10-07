using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisImpMestTypeUserSO : StagingObjectBase
    {
        public HisImpMestTypeUserSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_TYPE_USER, bool>>> listHisImpMestTypeUserExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_TYPE_USER, bool>>>();
    }
}
