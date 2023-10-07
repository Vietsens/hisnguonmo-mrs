using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisImpMestSttSO : StagingObjectBase
    {
        public HisImpMestSttSO()
        {
            //listHisImpMestSttExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_STT, bool>>> listHisImpMestSttExpression = new List<System.Linq.Expressions.Expression<Func<HIS_IMP_MEST_STT, bool>>>();
    }
}
