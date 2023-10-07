using HTC.DAO.Base;
using HTC.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace HTC.DAO.StagingObject
{
    public class HtcExpenseSO : StagingObjectBase
    {
        public HtcExpenseSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HTC_EXPENSE, bool>>> listHtcExpenseExpression = new List<System.Linq.Expressions.Expression<Func<HTC_EXPENSE, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HTC_EXPENSE, bool>>> listVHtcExpenseExpression = new List<System.Linq.Expressions.Expression<Func<V_HTC_EXPENSE, bool>>>();
    }
}
