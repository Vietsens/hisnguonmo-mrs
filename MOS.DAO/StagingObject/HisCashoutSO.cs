using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisCashoutSO : StagingObjectBase
    {
        public HisCashoutSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_CASHOUT, bool>>> listHisCashoutExpression = new List<System.Linq.Expressions.Expression<Func<HIS_CASHOUT, bool>>>();
    }
}
