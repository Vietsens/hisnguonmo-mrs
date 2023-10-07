using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBidSO : StagingObjectBase
    {
        public HisBidSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BID, bool>>> listHisBidExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BID, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_BID, bool>>> listVHisBidExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BID, bool>>>();
    }
}
