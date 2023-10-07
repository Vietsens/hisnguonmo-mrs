using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisExpMestSttSO : StagingObjectBase
    {
        public HisExpMestSttSO()
        {
            //listHisExpMestSttExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_STT, bool>>> listHisExpMestSttExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXP_MEST_STT, bool>>>();
    }
}
