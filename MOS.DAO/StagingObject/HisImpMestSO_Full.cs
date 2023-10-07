using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisImpMestSO : StagingObjectBase
    {
        public HisImpMestSO()
        {
            //listHisImpMestExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisImpMestExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST, bool>>> listHisImpMestExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST, bool>>> listVHisImpMestExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_1, bool>>> listVHisImpMest1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_1, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MANU, bool>>> listVHisImpMestManuExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_IMP_MEST_MANU, bool>>>();
    }
}
