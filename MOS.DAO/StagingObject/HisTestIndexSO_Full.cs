using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisTestIndexSO : StagingObjectBase
    {
        public HisTestIndexSO()
        {
            //listHisTestIndexExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisTestIndexExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_TEST_INDEX, bool>>> listHisTestIndexExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TEST_INDEX, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_TEST_INDEX, bool>>> listVHisTestIndexExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_TEST_INDEX, bool>>>();
    }
}
