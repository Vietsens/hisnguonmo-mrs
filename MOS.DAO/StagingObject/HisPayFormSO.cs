using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisPayFormSO : StagingObjectBase
    {
        public HisPayFormSO()
        {
            //listHisPayFormExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_PAY_FORM, bool>>> listHisPayFormExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PAY_FORM, bool>>>();
    }
}
