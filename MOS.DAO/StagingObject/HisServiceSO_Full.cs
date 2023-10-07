using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisServiceSO : StagingObjectBase
    {
        public HisServiceSO()
        {
            //listHisServiceExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisServiceExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERVICE, bool>>> listHisServiceExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE, bool>>> listVHisServiceExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_1, bool>>> listVHisService1Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_1, bool>>>();
    }
}
