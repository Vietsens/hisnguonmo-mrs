using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.StagingObject
{
    public class HisBhytBlacklistSO : StagingObjectBase
    {
        public HisBhytBlacklistSO()
        {
            
        }

        public List<System.Linq.Expressions.Expression<Func<HIS_BHYT_BLACKLIST, bool>>> listHisBhytBlacklistExpression = new List<System.Linq.Expressions.Expression<Func<HIS_BHYT_BLACKLIST, bool>>>();
    }
}
