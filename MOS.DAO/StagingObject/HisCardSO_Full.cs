using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisCardSO : StagingObjectBase
    {
        public HisCardSO()
        {
            //listHisCardExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisCardExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_CARD, bool>>> listHisCardExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CARD, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_CARD, bool>>> listVHisCardExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_CARD, bool>>>();
    }
}
