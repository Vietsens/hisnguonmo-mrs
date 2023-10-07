using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisHeinApprovalSO : StagingObjectBase
    {
        public HisHeinApprovalSO()
        {
            listHisHeinApprovalExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            listVHisHeinApprovalExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_HEIN_APPROVAL, bool>>> listHisHeinApprovalExpression = new List<System.Linq.Expressions.Expression<Func<HIS_HEIN_APPROVAL, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_HEIN_APPROVAL, bool>>> listVHisHeinApprovalExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_HEIN_APPROVAL, bool>>>();
    }
}
