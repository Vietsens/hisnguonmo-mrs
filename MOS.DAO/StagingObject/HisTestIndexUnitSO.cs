using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisTestIndexUnitSO : StagingObjectBase
    {
        public HisTestIndexUnitSO()
        {
            //listHisTestIndexUnitExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_TEST_INDEX_UNIT, bool>>> listHisTestIndexUnitExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TEST_INDEX_UNIT, bool>>>();
    }
}
