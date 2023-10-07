using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisServiceRetyCatSO : StagingObjectBase
    {
        public HisServiceRetyCatSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_RETY_CAT, bool>>> listHisServiceRetyCatExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_RETY_CAT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_RETY_CAT, bool>>> listVHisServiceRetyCatExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_RETY_CAT, bool>>>();
    }
}
