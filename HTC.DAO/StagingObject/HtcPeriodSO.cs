using HTC.DAO.Base;
using HTC.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace HTC.DAO.StagingObject
{
    public class HtcPeriodSO : StagingObjectBase
    {
        public HtcPeriodSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HTC_PERIOD, bool>>> listHtcPeriodExpression = new List<System.Linq.Expressions.Expression<Func<HTC_PERIOD, bool>>>();
    }
}
