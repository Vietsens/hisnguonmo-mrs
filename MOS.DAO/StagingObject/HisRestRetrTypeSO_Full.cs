using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisRestRetrTypeSO : StagingObjectBase
    {
        public HisRestRetrTypeSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_REST_RETR_TYPE, bool>>> listHisRestRetrTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REST_RETR_TYPE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_REST_RETR_TYPE, bool>>> listVHisRestRetrTypeExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_REST_RETR_TYPE, bool>>>();
    }
}
