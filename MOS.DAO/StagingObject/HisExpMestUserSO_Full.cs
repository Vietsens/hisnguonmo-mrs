using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExpMestUserSO : StagingObjectBase
    {
        public HisExpMestUserSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_USER, bool>>> listHisExpMestUserExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_USER, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_USER, bool>>> listVHisExpMestUserExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_USER, bool>>>();
    }
}
