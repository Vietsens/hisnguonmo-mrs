using HTC.DAO.Base;
using HTC.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace HTC.DAO.StagingObject
{
    public class HtcPeriodDepartmentSO : StagingObjectBase
    {
        public HtcPeriodDepartmentSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HTC_PERIOD_DEPARTMENT, bool>>> listHtcPeriodDepartmentExpression = new List<System.Linq.Expressions.Expression<Func<HTC_PERIOD_DEPARTMENT, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HTC_PERIOD_DEPARTMENT, bool>>> listVHtcPeriodDepartmentExpression = new List<System.Linq.Expressions.Expression<Func<V_HTC_PERIOD_DEPARTMENT, bool>>>();
    }
}
