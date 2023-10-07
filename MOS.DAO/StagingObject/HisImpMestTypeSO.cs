using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisImpMestTypeSO : StagingObjectBase
    {
        public HisImpMestTypeSO()
        {
            //listHisImpMestTypeExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_TYPE, bool>>> listHisImpMestTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_TYPE, bool>>>();
    }
}
