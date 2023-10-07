using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisServiceUnitSO : StagingObjectBase
    {
        public HisServiceUnitSO()
        {
            //listHisServiceUnitExpression.Add(o => !o.IS_DELETE.HasValue || o.IS_DELETE.Value != (short)1);
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_UNIT, bool>>> listHisServiceUnitExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_UNIT, bool>>>();
    }
}
