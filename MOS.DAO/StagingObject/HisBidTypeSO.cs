using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBidTypeSO : StagingObjectBase
    {
        public HisBidTypeSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BID_TYPE, bool>>> listHisBidTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BID_TYPE, bool>>>();
    }
}
