using HTC.DAO.Base;
using HTC.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace HTC.DAO.StagingObject
{
    public class HtcExpenseTypeSO : StagingObjectBase
    {
        public HtcExpenseTypeSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HTC_EXPENSE_TYPE, bool>>> listHtcExpenseTypeExpression = new List<System.Linq.Expressions.Expression<Func<HTC_EXPENSE_TYPE, bool>>>();
    }
}
