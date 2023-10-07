using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisAccountBookSO : StagingObjectBase
    {
        public HisAccountBookSO()
        {
            //listHisAccountBookExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisAccountBookExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_ACCOUNT_BOOK, bool>>> listHisAccountBookExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ACCOUNT_BOOK, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_ACCOUNT_BOOK, bool>>> listVHisAccountBookExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_ACCOUNT_BOOK, bool>>>();
    }
}
