using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBillFundSO : StagingObjectBase
    {
        public HisBillFundSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BILL_FUND, bool>>> listHisBillFundExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BILL_FUND, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_BILL_FUND, bool>>> listVHisBillFundExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_BILL_FUND, bool>>>();
    }
}
