using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExpMestSO : StagingObjectBase
    {
        public HisExpMestSO()
        {
            //listHisExpMestExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisExpMestExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST, bool>>> listHisExpMestExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST, bool>>> listVHisExpMestExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_1, bool>>> listVHisExpMest1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_1, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_CHMS, bool>>> listVHisExpMestChmsExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_CHMS, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_CHMS_1, bool>>> listV1HisExpMestChmsExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_CHMS_1, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_CHMS_2, bool>>> listV2HisExpMestChmsExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_CHMS_2, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MANU, bool>>> listVHisExpMestManuExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MANU, bool>>>();
    }
}
