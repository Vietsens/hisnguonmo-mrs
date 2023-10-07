using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExpMestBloodSO : StagingObjectBase
    {
        public HisExpMestBloodSO()
        {
            listHisExpMestBloodExpression.Add(o => (!o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1));
            listVHisExpMestBloodExpression.Add(o => (!o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1));
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_BLOOD, bool>>> listHisExpMestBloodExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_BLOOD, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_BLOOD, bool>>> listVHisExpMestBloodExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_BLOOD, bool>>>();
    }
}
