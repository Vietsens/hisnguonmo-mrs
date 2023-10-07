using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisFundSO : StagingObjectBase
    {
        public HisFundSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_FUND, bool>>> listHisFundExpression = new List<System.Linq.Expressions.Expression<Func<HIS_FUND, bool>>>();
    }
}
