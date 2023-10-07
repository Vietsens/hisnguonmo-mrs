using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisServiceFollowSO : StagingObjectBase
    {
        public HisServiceFollowSO()
        {
            listHisServiceFollowExpression.Add(o => (!o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1));
            listVHisServiceFollowExpression.Add(o => (!o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1));
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_FOLLOW, bool>>> listHisServiceFollowExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_FOLLOW, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_FOLLOW, bool>>> listVHisServiceFollowExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_FOLLOW, bool>>>();
    }
}
