using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisImpMestUserSO : StagingObjectBase
    {
        public HisImpMestUserSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_USER, bool>>> listHisImpMestUserExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_USER, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_USER, bool>>> listVHisImpMestUserExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_USER, bool>>>();
    }
}
