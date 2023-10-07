using HTC.DAO.Base;
using HTC.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace HTC.DAO.StagingObject
{
    public class HtcRevenueSO : StagingObjectBase
    {
        public HtcRevenueSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HTC_REVENUE, bool>>> listHtcRevenueExpression = new List<System.Linq.Expressions.Expression<Func<HTC_REVENUE, bool>>>();
    }
}
