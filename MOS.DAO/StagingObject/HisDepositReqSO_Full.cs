using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisDepositReqSO : StagingObjectBase
    {
        public HisDepositReqSO()
        {
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_DEPOSIT_REQ, bool>>> listHisDepositReqExpression = new List<System.Linq.Expressions.Expression<Func<HIS_DEPOSIT_REQ, bool>>>();
        public List<System.Linq.Expressions.Expression<Func<V_HIS_DEPOSIT_REQ, bool>>> listVHisDepositReqExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_DEPOSIT_REQ, bool>>>();
    }
}
