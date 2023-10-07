using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisServicePatySO : StagingObjectBase
    {
        public HisServicePatySO()
        {
            //listHisServicePatyExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
            //listVHisServicePatyExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_PATY, bool>>> listHisServicePatyExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_PATY, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_PATY, bool>>> listVHisServicePatyExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_PATY, bool>>>();
    }
}
