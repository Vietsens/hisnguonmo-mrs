using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisServSegrSO : StagingObjectBase
    {
        public HisServSegrSO()
        {
            //listHisServSegrExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisServSegrExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERV_SEGR, bool>>> listHisServSegrExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERV_SEGR, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERV_SEGR, bool>>> listVHisServSegrExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERV_SEGR, bool>>>();
    }
}
